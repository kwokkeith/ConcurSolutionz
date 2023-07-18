#nullable enable

using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using ConcurSolutionz.Controllers;
using ConcurSolutionz.Database;
using ConcurSolutionz.Models;
// using System.IO;  

namespace ConcurSolutionz.Views;

[QueryProperty(nameof(FileName), "fileName")]
[QueryProperty(nameof(ExistingFile), "existingFile")]
public partial class EntryPage : ContentPage
{
    private StudentProjectClaimMetaData md;
    private List<Database.Receipt> receipts;
    private string fileName;
    Database.Entry entry;

    // ReceiptView collection for storing and displaying Receipt models
    public ObservableCollection<Models.Receipt> ReceiptView { get; set; }


    private bool existingFile; // To determine if an existing entry boolean passed
    public bool ExistingFile
    {
        set
        {
            existingFile = value;
            //entry = Database.Entry.
            OnPropertyChanged();
            if (existingFile)
            {
                CreateExistingFile(FileName + ".entry");
                PopulateEntry();
                AddRecordButton.IsEnabled = true;
                EditRecordButton.IsEnabled = true;
                DeleteRecordButton.IsEnabled = true;
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
            string[] path = fileName.Split("/");
            fileName = path.Last().Substring(0, path.Last().Length - 6);
        }
        EntryName.Text = fileName; // To change the Title of the page

        this.fileName = fileName;
    }


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
        //Database.Database.Instance.Setwd("/Users/hongjing/Downloads")

        InitializeComponent();

        receipts = new();

        // Instantiate the Receipts collection
        ReceiptView = new ObservableCollection<Models.Receipt>();


        // Check if there is an existing file
        //      if (existingFile) // There exist a file passed
        //        {
        //            CreateExistingFile(FileName + ".entry");
        //            PopulateEntry();
        //        }
        //        else // No existing file passed
        //        {
        //            // TODO: Disable record UI features until user creates a file
        //            AddRecordButton.IsEnabled = false;
        //            EditRecordButton.IsEnabled = false;
        //            DeleteRecordButton.IsEnabled = false;

        //        }

        AddRecordButton.IsEnabled = false;
        EditRecordButton.IsEnabled = false;
        DeleteRecordButton.IsEnabled = false;

        // Set the BindingContext of the CollectionView
        recordCollection.BindingContext = this;

        // Set the ItemsSource of the CollectionView
        recordCollection.ItemsSource = ReceiptView;
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
        BudgetEditor.Text = md.EntryBudget.ToString();
        
        // Add Receipts to ReceiptView collection
        foreach(Database.Receipt receipt in receipts)
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
                    foreach(Database.Receipt receipt in receipts)
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
        Tuple<StudentProjectClaimMetaData, List<Database.Record>> fileDetail = Database.Database.Instance.getFileDetailFromFileName(name);

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
        if (entryName == "" || claimName == "" || purpose == "" || projectClub == "" ||  teamName == "")
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

            // enable the disabled buttons
            AddRecordButton.IsEnabled = true;
            EditRecordButton.IsEnabled = true;
            DeleteRecordButton.IsEnabled = true;

            //Build entry
            if (md != null)
            {
                BuildEntry();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Unable to create metadata\n" +
                ex, "OK");
        }
        
        
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

    private async void Concur_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("About to start selenium");
        string cookie = "";
        Process process = new Process();
        //Starting chrome driver
        try
        {

            //process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "SeleniumWrapper\\SeleniumWrapper.exe";
            process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "selenium", "SeleniumWrapperV2");
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to start Selenium");
            return;
        }
        //Attempting to read from chrome reader
        try
        {
            process.StandardInput.WriteLine("");
            StreamReader reader = process.StandardOutput;
            while (true)
            {
                string newline = reader.ReadLine()!;
                if (newline == null) break;
                cookie += newline;
            }
            cookie = "JWT" + cookie.Split("JWT")[1];
            await DisplayAlert("Progress", "Cookies Extracted! Please wait for the next prompt for completion", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "Error extracting cookies, please try again.", "OK");
            Console.WriteLine(ex.ToString());
            return;
        }
        Console.WriteLine("cookie:" + cookie);
        PushToConcur(cookie, receipts, entry);
    }

    public async void PushToConcur(string cookie, List<Database.Receipt> receipts, Database.Entry entry)
    {
        //Initialize API caller
        ConcurAPI concur = new ConcurAPI(cookie);
        string init = await concur.Initialize(); // Returns 0 is successful, 1-3 are errors
        Console.WriteLine("Init status: " + init);
        if (init != "0")
        {
            Console.WriteLine("Failed to init");
            //return;
            throw new Exception("Failed to initialize API");
        }
        //Create new claim
        StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)entry.MetaData;
        Claim claim = new Claim();
        claim.Name = MD.ClaimName;
        claim.Date = MD.ClaimDate.ToString("yyyy-MM-dd");
        claim.Policy = MD.ProjectClub;  //TODO: REPLACE With actual policy
        claim.TeamName = MD.TeamName;
        claim.Purpose = MD.Purpose;

        claim.Id = await concur.CreateClaim(claim);
        claim.Key = await concur.GetReportKey(claim.Id);

        Console.WriteLine("Claim key: " + claim.Key);
        List<Expense> expenses = new List<Expense>();

        for (int i = 0; i < receipts.Count; i++)
        {
            Expense expense = new Expense();
            expense.Date = receipts[i].TransactionDate.ToString("yyyy-MM-dd");
            expense.Cost = receipts[i].CurrencyAmountSGD;
            expense.Description = receipts[i].Description;
            expense.Supplier = receipts[i].SupplierName;
            expense.ReceiptNo = receipts[i].ReceiptNumber;
            expense.Comment = receipts[i].Comment;
            expense.ReportId = claim.Id;
            expense.FilePath = receipts[i].ImgPath;
            expense.RPEKey = await concur.CreateExpense(expense, claim);

            expenses.Add(expense);
        }

        List<Expense> expenseIDs = await concur.GetAllExpenses(claim);

        for (int i = 0; i < expenses.Count; i++)
        {
            for (int j = 0; j < expenseIDs.Count; j++)
            {
                if (expenses[i].RPEKey.Equals(expenseIDs[j].RPEKey))
                {
                    expenses[i].Id = expenseIDs[j].Id;
                    Console.WriteLine("Expense " + expenses[i].Id + " is in claim " + claim.Id);
                    string filepath = expenses[i].FilePath;
                    string[] split = filepath.Split('/');
                    expenses[i].ImageId = await concur.UploadImage(filepath, split.Last());
                    await concur.LinkImageToRequest(expenses[i]);
                }
            }

        }
        await DisplayAlert("Complete", "Claim has been made on Concur, please double check the contents and submit on the SAP Concur Portal", "OK");
        //Purpose.Text = await concur.LinkImageToRequest(expense);
    }
}