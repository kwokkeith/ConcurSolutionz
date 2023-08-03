#nullable enable

using System.Collections.ObjectModel;
using System.Diagnostics;
using ConcurSolutionz.Database;
using ConcurSolutionz.Models;

namespace ConcurSolutionz.Views;
[QueryProperty(nameof(FileName), "fileName")]
[QueryProperty(nameof(filePath), "filePath")]
[QueryProperty(nameof(ExistingFile), "existingFile")]
public partial class EntryPage : ContentPage
{
    private StudentProjectClaimMetaData md;
    private List<Database.Receipt> receipts;
    private string fileName;
    Database.Entry? entry;
    Database.Receipt selectedReceipt;

    // Calculate budget
    private decimal entryBudget { get; set; }
    private decimal currentExpense { get; set; }
    private decimal remainingBudget { get; set; }

    // Dictionary containing the names of projects/clubs, along with their codes
    private IReadOnlyDictionary<string, string> ClubDict { get; set; }

    // ReceiptView collection for storing and displaying Receipt models
    public ObservableCollection<Models.Receipt> ReceiptView { get; set; }

    public string filePath { get; set; }
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
                AddRecordButton.IsVisible = true;
                EditRecordButton.IsVisible = true;
                DeleteRecordButton.IsVisible = true;
                TransferButton.IsVisible = true;
                DeleteEntryButton.IsVisible = true;
                EditEntryNameButton.IsVisible = true;
                AllRecords.IsVisible = true;
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
        if (fileName.EndsWith(".entry"))
        {
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
        InitializeComponent();

        receipts = new();

        // Instantiate the Receipts collection
        ReceiptView = new ObservableCollection<Models.Receipt>();

        AddRecordButton.IsVisible = false;
        EditRecordButton.IsVisible = false;
        DeleteRecordButton.IsVisible = false;
        TransferButton.IsVisible = false;
        DeleteEntryButton.IsVisible = false;
        EditEntryNameButton.IsVisible = false;
        AllRecords.IsVisible = false;

        // Set the BindingContext of the CollectionView
        recordCollection.BindingContext = this;

        // Set the ItemsSource of the CollectionView
        recordCollection.ItemsSource = ReceiptView;

        // Set the project/club names and their corresponding codes
        ClubDict = new Projects().GetProjects();

        foreach (KeyValuePair<string, string> kvp in ClubDict)
        {
            ProjectClubInp.Items.Add(kvp.Key);
        }

    }


    // <Summary>Populate Entry Page (If entry exists)</Summary>
    private void PopulateEntry()
    {
        // Populate Metadata fields
        ClaimNameInp.Text = md.ClaimName;
        ClaimDateInp.Date = md.ClaimDate;
        Policy.SelectedIndex = 0;
        ProjectClubInp.SelectedItem = ClubDict.FirstOrDefault(x => x.Value == md.ProjectClub).Key;
        TeamNameInp.Text = md.TeamName;
        Purpose.Text = md.Purpose;
        BudgetEditor.Text = md.EntryBudget.ToString();

        // Entry Budget
        entryBudget = md.EntryBudget;
        currentExpense = 0;

        // Clear ReceiptView Collection
        ReceiptView.Clear();

        // Add Receipts to ReceiptView collection
        foreach (Database.Receipt receipt in receipts)
        {
            string frontEndExpenseType = receipt.ExpenseType;
            string frontEndPaymentType = receipt.PaymentType;
            string frontEndSupplierName = receipt.SupplierName;
            DateTime frontEndTransactionDate = receipt.TransactionDate;
            decimal frontEndReqAmount = receipt.ReqAmount;

            // Add current Expense of Entry
            currentExpense += receipt.reqAmount;

            Models.ReceiptBuilder builder = new Models.ReceiptBuilder()
               .SetPaymentType(frontEndPaymentType)
               .SetExpenseType(frontEndExpenseType)
               .SetSupplierName(frontEndSupplierName)
               .SetTransactionDate(frontEndTransactionDate)
               .SetReqAmount(frontEndReqAmount)
               .SetSupplierName(receipt.SupplierName);

            Models.Receipt modelReceipt = builder.Build();
            modelReceipt.recordID = receipt.RecordID + 1;

            ReceiptView.Add(modelReceipt);
        }
        CalculateBudget();
        BuildMDPopulate();
    }

    protected override bool OnBackButtonPressed()
    {
        base.OnBackButtonPressed();
        return false;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BuildMDPopulate();
    }
    private void OnBudgetCompleted(object sender, EventArgs e)
    {
        string entryBudgetString = BudgetEditor.Text;

        // change entryBudgetString to decimal datatype
        if (!Decimal.TryParse(entryBudgetString, out decimal budget))
        {
            budgetWarning.IsVisible = true;
            return;
        }
        budgetWarning.IsVisible = false;
        entryBudget = budget;

        CalculateBudget();
        OnEditorsTextChanged(sender, e);
    }



    private void CalculateBudget()
    {
        Application.Current.RequestedThemeChanged += (s, a) =>
        {
            if (RemainingBudget.TextColor != Colors.Red)
            {
                if (Application.Current.RequestedTheme == AppTheme.Light)
                {
                    RemainingBudget.TextColor = Colors.Black;

                }
                else
                {
                    RemainingBudget.TextColor = Colors.White;
                }
            }  
        };

        remainingBudget = entryBudget - currentExpense;
        CurrentExpenseInput.Text = currentExpense.ToString();
        RemainingBudget.Text = remainingBudget.ToString();
        if (remainingBudget < 0)
        {
            RemainingBudget.TextColor = Colors.Red;
        }
        else
        {
            if(Application.Current.RequestedTheme == AppTheme.Light)
            {
                RemainingBudget.TextColor = Colors.Black;

            }
            else
            {
                RemainingBudget.TextColor = Colors.White;
            }
        }
    }

    
    private void OnEditorsTextChanged(object sender, EventArgs e)
    {
        if (SetMetadataButton.IsEnabled == false)
        {
            SetMetadataButton.IsEnabled = true;
        }
    }


    // Click event handler for editing entry name
    private async void EditEntryName_Clicked(object sender, EventArgs e)
    {
        string prevName = EntryName.Text + ".entry";

        string result = await DisplayPromptAsync("New entry name", "Alphabets and spaces only", keyboard: Keyboard.Text);
        string newName = result + ".entry";

        string rootDir = Database.Database.Instance.GetSettings().GetRootDirectory();

        if (!string.IsNullOrWhiteSpace(result))
        {
            try
            {
                string newFilePath = Path.Combine(rootDir, newName);
                string prevFilePath = Path.Combine(rootDir, prevName); 

                // Rename the selected file/folder in the target directory
                Directory.Move(prevFilePath, newFilePath);

                // Create a new FileItem with the updated file name and other properties
                FileItem renamedFile = new FileItem(newName, false);
                renamedFile.CreationDateTime = DateTime.Now;

                // Handle updating of Metadata
                Database.Database.RenameEntry(newFilePath);
                EntryName.Text = result;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.ToString(), "ok");
            }
        }
    }


    // Click event handler for editing record
    private async void EditRecord_Clicked(object sender, EventArgs e)
    {
        if (recordCollection.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a record!", "OK");
        }

        Models.Receipt selectedReceipt = recordCollection.SelectedItem as Models.Receipt;
        Database.Receipt receipt = receipts.FirstOrDefault(r => r.RecordID == selectedReceipt.recordID - 1);

        FileDB file = entry;
        var imagePath = receipt.ImgPath;

        var navigationParameter = new Dictionary<string, object>
        {
            {"file", file },
            {"imagePath", imagePath },
            {"existingReceipt", receipt }
        };

        await Shell.Current.GoToAsync(nameof(RecordPage), navigationParameter);
    }


    private async void DeleteEntry_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(
            "Confirm Deletion",
            $"Are you sure you want to delete Entry {Path.GetFileNameWithoutExtension(entry.FileName)}?",
            "Yes", "No");

        if (answer)
        {
            Database.Database.DeleteDirectoryByFilePath(filePath);
            await Shell.Current.GoToAsync(nameof(MainPage));
        }
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


    private async void DeleteRecord_Clicked(object sender, EventArgs e)
    {
        if (recordCollection.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a record!", "OK");
        }

        Models.Receipt selectedReceipt = recordCollection.SelectedItem as Models.Receipt;
        Database.Record rec = receipts.FirstOrDefault(r => r.RecordID == selectedReceipt.recordID - 1);
        Database.Receipt receipt = receipts.FirstOrDefault(r => r.RecordID == selectedReceipt.recordID - 1);

        if (selectedReceipt != null)
        {
            bool answer = await DisplayAlert("Confirm Deletion", $"Are you sure you want to delete Record {receipt.RecordID + 1}?", "Yes", "No");

            if (answer)
            {
                // Remove receipt from collection
                ReceiptView.Remove(selectedReceipt);
                receipts.Remove(receipt);
                entry.DelRecord(rec);

                // Update remaining budget and remove from entry
                PopulateEntry();
            }
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
                        {"existingReceipt", null }
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

    // *******************************
    // @@@@@@@@@@ UTILITIES @@@@@@@@@@
    // *******************************

    // Create entry from existing file
    private void CreateExistingFile(string name)
    {
        Tuple<MetaData, List<Database.Record>> fileDetail;

        fileDetail = Database.Database.Instance.getFileDetailFromFileName(name);

        // Get existing metadata
        md = MDAdaptor.ConvertMetaData(fileDetail.Item1);

        // Get List of existing Receipts
        receipts.Clear(); // remove all receipts in existing receipts list
        foreach (Database.Record record in fileDetail.Item2)
        {
            receipts.Add(RecordAdaptor.ConvertRecord(record));
        }
    }


    // change the selected record
    void OnRecordSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int? previous = (e.PreviousSelection.FirstOrDefault() as Database.Receipt)?.RecordID;
        int? current = (e.CurrentSelection.FirstOrDefault() as Database.Receipt)?.RecordID;
    }


    // Click event handler for setting metadata of entry
    private void SetMetaData_Clicked(object sender, EventArgs e)
    {
        BuildMDPopulate();
        
    }


    private async void BuildMDPopulate()
    {
        string entryName = EntryName.Text;
        string entryBudgetString = BudgetEditor.Text;
        string claimName = ClaimNameInp.Text;
        DateTime claimDate = ClaimDateInp.Date;
        string purpose = Purpose.Text;
        string projectClub = null;
        if (ProjectClubInp.SelectedIndex > -1)
        {
            projectClub = ClubDict[(string)ProjectClubInp.SelectedItem];

        }
        string teamName = TeamNameInp.Text;

        // Check if any field is left blank
        if (entryName == null || claimName == null || purpose == null || teamName == null || projectClub == null)
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
            AddRecordButton.IsVisible = true;
            EditRecordButton.IsVisible = true;
            DeleteRecordButton.IsVisible = true;
            TransferButton.IsVisible = true;
            DeleteEntryButton.IsVisible = true;
            EditEntryNameButton.IsVisible = true;
            AllRecords.IsVisible = true;

            SetMetadataButton.IsEnabled = false;
            
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


    //Handles initialization of entry and updating of variables md, entry, and receipts
    private void BuildEntry()
    {
        List<Database.Record> records = new();
        // construct an entry path
        string wd = Database.Database.Instance.Getwd();
        string entryName = EntryName.Text;
        string entryPath = Path.Combine(wd, entryName + ".entry");
        Database.Entry.EntryBuilder entryBuilder = new();
        foreach (Database.Receipt receipt in receipts)
        {
            string receiptFileName = Path.GetFileName(receipt.ImgPath);
            FileCreator.CopyFile(receipt.ImgPath,Path.Combine(Path.GetTempPath(), receiptFileName));
            receipt.ImgPath = Path.Combine(Path.GetTempPath(), receiptFileName);
            records.Add(RecordAdaptor.ConvertRecord(receipt));
        }

        //Build entry from scratch (for fresh opening of entry)
        if (entry == null && (existingFile))
        {
            try
            {
                // build entry
                entry = entryBuilder.SetFileName(entryName)
                                    .SetCreationDate(Directory.GetCreationTime(entryPath))
                                    .SetLastModifiedDate(Directory.GetLastAccessTime(entryPath))
                                    .SetFilePath(wd)
                                    .SetMetaData(md)
                                    .SetRecords(records)
                                    .Build();
            }
            catch (Exception ex)
            {
                entry = null;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Rebuild Entry after modifications (while in entry)
        else
        {
            if (Directory.Exists(entryPath))
            {
                Database.Database.DeleteDirectoryByFilePath(entryPath);
            }

            // Rebuild entry
            try
            {
                entry = entryBuilder.SetFileName(entryName)
                                    .SetCreationDate(Directory.GetCreationTime(entryPath))
                                    .SetLastModifiedDate(Directory.GetLastAccessTime(entryPath))
                                    .SetFilePath(wd)
                                    .SetMetaData(md)
                                    .SetRecords(records)
                                    .Build();

                FileCreator.CreateFile(entry);

            }
            catch (Exception ex)
            {
                entry = null;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }
    }

    private async void Concur_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("About to start selenium");
        string cookie = "";
        Process process = new();

        //Starting chrome driver
        if (entry == null)
        {
            await DisplayAlert("Error", "Entry not saved/ is empty", "OK");
            return;
        }
        else if (receipts.Count == 0) {
            await DisplayAlert("Error",
            "No receipts added, please add one before sending to concur",
            "OK");
            return;
        }

        try
        {
            if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
                process.StartInfo.FileName = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "selenium", "SeleniumWrapperV2"
                    );
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                process.StartInfo.FileName =
                    AppDomain.CurrentDomain.BaseDirectory +
                    "SeleniumWrapper\\SeleniumWrapper.exe";
            }
              
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            PageEnabled(false);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to start Selenium");
            await DisplayAlert("Error", "Failed to start Selenium!", "OK");
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

            await DisplayAlert(
                "Progress",
                "Cookies Extracted! Please wait for the next prompt for completion",
                "OK"
                );

            Console.WriteLine("cookie:" + cookie);
            PushToConcur(cookie, receipts, entry);
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "ERROR",
                "Error extracting cookies, please try again.",
                "OK"
                );
            Console.WriteLine(ex.ToString());
            return;
        }

    }


    public async void PushToConcur(string cookie, List<Database.Receipt> receipts, Database.Entry entry)
    {
        //Initialize API caller
        ConcurAPI concur = new(cookie);

        // Returns 0 is successful, 1-3 are errors
        string init = await concur.Initialize(); 

        Console.WriteLine("Init status: " + init);
        if (init != "0")
        {
            Console.WriteLine("Failed to init");
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
            Expense expense = new()
            {
                Date = receipts[i].TransactionDate.ToString("yyyy-MM-dd"),
                Cost = receipts[i].ReqAmount,
                Description = receipts[i].Description,
                Supplier = receipts[i].SupplierName,
                ReceiptNo = receipts[i].ReceiptNumber,
                Comment = receipts[i].Comment,
                ReportId = claim.Id,
                FilePath = receipts[i].ImgPath
            };
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

        Process process = new Process();
        try
        {
            string[] CookieSplit = cookie.Split(";");
            string jwt = "", bqrn = "", bqrd = "";

            foreach (string param in CookieSplit)
            {
                string[] temp = param.Split("=");
                if (temp[0].Equals("JWT")) jwt = temp[1];
                else if (temp[0].Equals("OTSESSIONAABQRD")) bqrn = temp[1];
                else if (temp[0].Equals("OTSESSIONAABQRN")) bqrd = temp[1];
            }

            Debug.WriteLine("JWT: " + jwt);
            Debug.WriteLine("BQRN: " + bqrn);
            Debug.WriteLine("BQRD: " + bqrd);

            if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
                process.StartInfo.FileName = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..",
                    "..",
                    "..",
                    "publish",
                    "SessionHijackBrowser"
                    );
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            {
                process.StartInfo.FileName =
                    AppDomain.CurrentDomain.BaseDirectory +
                    "CookieBrowser\\CookieBrowser.exe";
            }

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.Arguments = jwt + " " + bqrn + " " + bqrd;
            process.Start();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Failed to start Cookie Browser");
            return;
        }
        finally
        {
            PageEnabled(true);
        }
    }

    private void PageEnabled(bool enable)
    {
        EditEntryNameButton.IsEnabled = enable;
        TransferButton.IsEnabled = enable;
        DeleteEntryButton.IsEnabled = enable;
        Policy.IsEnabled = enable;
        ProjectClubInp.IsEnabled = enable;
        ClaimNameInp.IsEnabled = enable;
        TeamNameInp.IsEnabled = enable;
        ClaimDateInp.IsEnabled = enable;
        Purpose.IsEnabled = enable;
        BudgetEditor.IsEnabled = enable;
        AddRecordButton.IsEnabled = enable;
        EditRecordButton.IsEnabled = enable;
        DeleteRecordButton.IsEnabled = enable;
        ConcurLoading.IsRunning = !enable;
    }
}