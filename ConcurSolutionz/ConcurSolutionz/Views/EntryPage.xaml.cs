#nullable enable

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ConcurSolutionz.Controllers;
using ConcurSolutionz.Database;
// using System.IO;  

namespace ConcurSolutionz.Views;

[QueryProperty(nameof(FileName), "fileName")]
[QueryProperty(nameof(ExistingFile), "existingFile")]
public partial class EntryPage : ContentPage
{
    private StudentProjectClaimMetaData md;
    private List<Receipt> receipts;
    private string fileName;

    private bool existingFile; // To determine if an existing entry boolean passed
    public bool ExistingFile
    {
        set
        {
            existingFile = value;
            OnPropertyChanged();
            if (existingFile)
            {
                CreateExistingFile(FileName + ".entry");
                PopulateEntry();
            }
        }
    } 

    public string FileName
    {
        set
        {
            LoadFile(value);
        }
        get
        {
            return fileName;
        }

    }

    private void LoadFile(string fileName)
    {
        if (fileName.EndsWith(".entry")){
            fileName = fileName.Substring(0, fileName.Length - 6);
        }
        EntryName.Text = fileName; // To change the Title of the page

        this.fileName = fileName;
    }

    // ReceiptView collection for storing and displaying Receipt models
    public ObservableCollection<Models.Receipt> ReceiptView { get; set; }
    Database.Entry entry;

    public string InitName
    {
        set
        {
            EntryName.Text = value;
        }
    }

    public EntryPage()
    {
        // Set the working directory for the database instance
        //Database.Database.Instance.Setwd("/Users/hongjing/Downloads");

        InitializeComponent();

        receipts = new();

        // Instantiate the Receipts collection
        ReceiptView = new ObservableCollection<Models.Receipt>();


        // Check if there is an existing file
        if (existingFile) // There exist a file passed
        {
            CreateExistingFile(FileName + ".entry");
            PopulateEntry();
        }
        else // No existing file passed
        {
            // TODO: Disable record UI features until user creates a file
            AddRecordButton.IsEnabled = false;
            EditRecordButton.IsEnabled = false;
            DeleteRecordButton.IsEnabled = false;

        }

        // Set the BindingContext of the CollectionView
        recordCollection.BindingContext = this;

        // Set the ItemsSource of the CollectionView
        //recordCollection.ItemsSource = ReceiptView;
    }

    

    // Populate Entry Page (If entry exists)
    private void PopulateEntry()
    {
        // Populate Metadata fields
        ClaimNameInp.Text = md.ClaimName;
        ClaimDateInp.Date = md.ClaimDate;
        ProjectClubInp.Text = md.ProjectClub;
        TeamNameInp.Text = md.TeamName;
        Purpose.Text = md.Purpose;
        
        // Add Receipts to ReceiptView collection
        foreach(Receipt receipt in receipts)
        {
            string frontEndExpenseType = receipt.ExpenseType;
            string frontEndPaymentType = receipt.PaymentType;
            string frontEndSupplierName = receipt.SupplierName;
            DateTime frontEndTransactionDate = receipt.TransactionDate;
            decimal frontEndReqAmount = receipt.ReqAmount;

            ReceiptView.Add(new Models.Receipt(new Models.ReceiptBuilder()
                .SetPaymentType(frontEndPaymentType)
                .SetExpenseType(frontEndExpenseType)
                .SetSupplierName(frontEndSupplierName)
                .SetTransactionDate(frontEndTransactionDate)
                .SetReqAmount(frontEndReqAmount)
                ));
        }
    }

    
    // Click event handler for editing entry name
    private async void EditEntryName_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New entry name", "Alphabets and spaces only", keyboard: Keyboard.Text);
        if (result != null)
        {
            EntryName.Text = result;
        }
    }

    // Click event handler for editing record
    async void EditRecord_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RecordPage));
    }

    
    // Click event handler for adding new record
    private async void AddRecord_Clicked(object sender, EventArgs e)
    {
       
        string action = await DisplayActionSheet("Upload an image of your receipt", "Cancel", null, "Upload");
        if (action == "Upload")
        {
            // Call the PickAndShow method with the options for picking an image file
            await PickAndShow(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
                { DevicePlatform.iOS, new[] { "jpg", "jpeg", "png" } },
                { DevicePlatform.macOS, new[] { "jpg", "jpeg", "png" } },
                { DevicePlatform.MacCatalyst, new[] { "jpg", "jpeg", "png" } },
                { DevicePlatform.Android, new[] { "image/*" } },
                { DevicePlatform.WinUI, new[] { "jpg", "jpeg", "png" } }
            }),
                PickerTitle = "Select an image"
            });
        }

    }

    // Method to pick and show image file
    public async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            // Attempt to pick the file
            var result = await FilePicker.PickAsync(options);

            // If the result is not null, a file was picked
            if (result != null)
            {
                // If the picked file is a jpg or png
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    // Convert receipt list to record list
                    List<Database.Record> records = new();
                    foreach(Receipt receipt in receipts)
                    {
                        records.Add(receipt);
                    }
                    
                    // Create a File instance
                    Database.Entry.EntryBuilder builder = new();
                    builder.SetMetaData(md)
                        .SetCreationDate(DateTime.Now)
                        .SetFileName(fileName)
                        .SetFilePath(Database.Database.Instance.Getwd())
                        .SetLastModifiedDate(DateTime.Now)
                        .SetRecords(records);
                    Database.Entry entry = builder.Build();
                    FileDB file = entry;

                    // Declare object-based navigation data
                    var navigationParameter = new Dictionary<string, object>
                    {
                        {"file", file },
                        {"imagePath", result.FullPath },
                        {"existingRecordBool", false }
                        //{"existingRecord", null }
                    };

                    // pass the file over to the record page
                    await Shell.Current.GoToAsync(
                        $"{nameof(RecordPage)}", navigationParameter);
                }
                else
                {
                    // Not a supported image type
                    await DisplayAlert("Error", "Selected file is not a supported image type", "OK");
                }
            }
            else
            {
                // User canceled file picker
                return null;
            }

            // Return the file result
            return result;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
            await DisplayAlert("Error", "An error occurred while picking the file: " + ex.ToString(), "OK");

            // Return null because an exception was thrown
            return null;
        }
    }


    // UTILITIES
    // Create entry from existing file
    private async void CreateExistingFile(string name)
    {
        Tuple<MetaData, List<Database.Record>> fileDetail = Database.Database.Instance.getFileDetailFromFileName(name);

        // Get existing metadata
        try
        {
            md = MDAdaptor.ConvertMetaData(fileDetail.Item1);
        }
        catch (Exception e)
        {
            await DisplayAlert("Failure!", $"Failed to convert MetaData when loading from existing file! Error is: {e}", "OK");
        }

        // Get List of existing Receipts
        receipts.Clear(); // remove all receipts in existing receipts list
        foreach(Database.Record record in fileDetail.Item2)
        {
            // Convert the record to receipt and add to list of receipts
            try
            {
                receipts.Add(RecordAdaptor.ConvertRecord(record));
            }
            catch (Exception e)
            {
                await DisplayAlert("Failure!", "Failed to convert record instance to receipt when loading existing file!", "OK");
            }
        }
    }

    // Click event handler for setting metadata of entry
    private async void SetMetaData_Clicked(object sender, EventArgs e)
    {
        string entryName = EntryName.Text;
        string entryBudgetString = BudgetEditor.Text;
        string claimName = ClaimNameInp.Text;
        DateTime claimDate = ClaimDateInp.Date;
        string purpose = Purpose.Text;
        string projectClub = ProjectClubInp.Text;
        string teamName = TeamNameInp.Text;
        //string policy = (string)Policy.ItemsSource[Policy.SelectedIndex];

        // Check if any field is left blank
        if ((entryName ?? claimName ?? purpose ?? projectClub ?? teamName) == "")
        {
            // display error and quit the function
            await DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

        // change entryBudgetString to decimal datatype
        if (!Decimal.TryParse(entryBudgetString, out decimal entryBudget))
        {
            await DisplayAlert("Error", "Invalid budget entered", "OK");
            return;
        }

        // build metadata if nothing is wrong
        StudentProjectClaimMDBuilder studentProjMDBuilder = new();
        try
        {
            md = studentProjMDBuilder
                .SetEntryName(entryName)
                .SetEntryBudget(entryBudget)
                .SetClaimName(claimName)
                .SetClaimDate(claimDate)
                .SetPurpose(purpose)
                .SetTeamName(teamName)
                .SetProjectClub(projectClub)
                .Build();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Unable to create metadata\n" +
                ex, "OK");
        }
        // Display success
        await DisplayAlert("Success", "Entry details successfully recorded", "OK");

        // enable the disabled buttons
        AddRecordButton.IsEnabled = true;
        EditRecordButton.IsEnabled = true;
        DeleteRecordButton.IsEnabled = true;

        //Build entry

        BuildEntry();
    }

    private async void BuildEntry()
    {
        if (entry == null)
        {
            try
            {
                // construct an entry path
                string wd = Database.Database.Instance.Getwd();
                string entryName = EntryName.Text;
                string entryPath = Path.Combine(wd, entryName);

                // if the file already exists, delete and create new one
                if (Directory.Exists(entryPath))
                {
                    Directory.Delete(entryPath, true);
                }

                // build entry
                Database.Entry.EntryBuilder entryBuilder = new();
                entry = entryBuilder.SetFileName(entryName)
                                    .SetCreationDate(DateTime.Now)
                                    .SetLastModifiedDate(DateTime.Now)
                                    .SetFilePath(wd)
                                    .SetMetaData(md)
                                    .SetRecords(new List<Database.Record>())
                                    .Build();

                await DisplayAlert("Success", "Entry successfully made", "OK");
            }
            catch (Exception ex)
            {
                entry = null;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Creating a file in the database
            Database.Database.CreateFile(entry);
        }
    }

}

