    #nullable enable

using System;
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
[QueryProperty(nameof(filePath), "filePath")]
[QueryProperty(nameof(ExistingFile), "existingFile")]
public partial class EntryPage : ContentPage
{
    private StudentProjectClaimMetaData md;
    private List<Database.Receipt> receipts;
    private string fileName;
    Database.Entry entry;
    Database.Receipt selectedReceipt;

    // Dictionary containing the names of projects/clubs, along with their codes
    private IReadOnlyDictionary<string, string> ClubDict { get; }

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
                UpdateMetadataButton.IsVisible = true;
                SetMetadataButton.IsVisible = false;
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
        SetMetadataButton.IsVisible = true;
        AddRecordButton.IsVisible = false;
        EditRecordButton.IsVisible = false;
        DeleteRecordButton.IsVisible = false;
        UpdateMetadataButton.IsVisible = false;

        // Set the BindingContext of the CollectionView
        recordCollection.BindingContext = this;

        // Set the ItemsSource of the CollectionView
        recordCollection.ItemsSource = ReceiptView;

        try
        {
            // Set the project/club names and their corresponding codes
            ClubDict = new Dictionary<string, string>
            {
                {"10TH ANNIVERSARY GALA DINNER", "5d5d08a511f98e4ab32f28ba68a86350"},
                {"3DC, DIGITAL DESIGN & DEVELOPMT CLUB", "a5d930c45bd311438667f59d4307dcc3"},
                //{"3DC, DIGITAL DESIGN & DEVELOPMT CLUB", "b918b6bb80daea489a1b0ebb3f69d94b"},
                {"A C TOH STUDENT AWARD", "ef7bb82c7b410441839f322989a0e39c"},
                {"A CAPPELLA CLUB (VOCOMOTIVES)", "374598392a431940b17a263fcf294505"},
                //{"A CAPPELLA CLUB (VOCOMOTIVES)", "a68358e7a22d3a45b5ccd051fd2874ca"},
                {"A.C. TOH SCHOLARSHIP", "7b725118c5b02b47add3f32a96a28286"},
                {"ABODE", "ecf676aa6df5234eaea80be32e9205f4"},
                {"ABS-SUTD STUDY AWARD", "f4381e9a899d4842b716be6483503957"},
                {"ABWIN SUTD FINANCIAL AID", "5ef56bf44536ce47874c313056a4394d"},
                {"ACADEMY - TFI WORKSHOP", "af0964e83ae822449498174dd2a743e4"},
                {"ACCENTURE ANALYTICS EDGE AWARD", "af5f775184338b48a74b08875ce7884c"},
                {"AD OUTREACH ACTIVITIES", "b428f18397dc394d9edcaf63b44f56f3"},
                {"ADAPTIVE HYBRID-WIRE ARC ADDITIVE MF", "82d47b78b37ace43a18382a128508235"},
                {"ADMISSION LOCAL OUTREACH TEA SESSION", "ec0845d5f06507469ba6ec390e39fc7d"},
                {"ADMISSIONS & CAREER TALK", "628b19a1436e204ead70083326daf553"},
                //{"ADMISSIONS & CAREER TALK", "efedeab8bcffd6439c4dd1bc0345b85f"},
                {"ADMISSIONS LOCAL OUTREACH", "c4f8bc86d655244aa6d96110f349ebfc"},
                //{"ADMISSIONS LOCAL OUTREACH", "18faad748ce19e44a71b0543ca04dc20"},
                {"ADMISSIONS OVERSEAS OUTREACH", "f2c96b19e67f554d9379234efd8c4c47"},
                {"ADMISSIONS WORKSHOPS", "0b7f58bf5dc3264c96f59976e869bfb7"},
                {"ADOBE RESEARCH PROJECT", "fdf907fd24048f4fab012656709b5641"},
                {"ADVANCED FEEDBACK AND CONTROL AWARD", "90a8aac269134c439c9e49adf9dcf57a"},
                {"ADVENTURE ACTIVITY / PROJECT FUND", "baaa26cd3830e44fab1e0015c08ac8fd"},
                {"AEGIS AND AICRIT", "a972fe2cf64e044593f06b2980f3f1c0"},
                {"AGA ARCHITECTS PTE LTD", "09810e1546490c4f8d808d42550ac92e"},
                {"AGUS - SUTD BURSARY", "ee6daf86b5856240ae573184baa5eccf"},
                {"AIRPORT GROUND COLLISION ALERTG SYS", "982c14375c630e488c163bb0bd56c9ee"},
                {"AIRPORT SYS PLANNING & DESIGN AWARD", "b73a1e69ddf6f34ab7cb581684658fc8"},
                {"AM FOR A BETTER WORLD", "e408e37764aab94d9e1c2d7f0ce24022"},
                {"AMERICAN BUREAU OF SHIPPING", "7e00efb08faed6488bce7896dcfc44f7"},
                {"AMP-LBKM-SUTD ENDOWED SCHOLARSHIP", "17cc82368e1efb44be6091835a06975d"},
                {"ANALYTICS EDGE AWARD (ESD)", "7f10d00ce47a7343804ed12400f91c5e"},
                {"ANOMALY DETECTOR WATER TREATMT PLANT", "4fb20d380da6bc40a5b24fb552d53155"},
                {"APEX CLUB", "137308268f434b499a730ab693bd8c33"},
                //{"APEX CLUB", "00a6fb8519a9ce4482196c0800fd9ed7"},
                {"APPLIED MATERIALS - SUTD STUDY GRANT", "6ad2663859879b40abcccc9dc95e3ca7"},
                {"ARCHERY CLUB", "80ddc3ef8cbdbd4d9876ebb110708990"},
                //{"ARCHERY CLUB", "e4e736662d3fef4a9d7b9f8dde392f4a"},
                {"ARCHITECTURAL HISTORY STUDY TRIP", "2913372333ec6c46842c46b1ce1180f6"},
                {"ARCHITECTURAL INTELLIGENCE RSCH LAB", "9e4e6e19ae8c3542990aff8fd5732089"},
                {"ARCHITECTURE & SUSTAINABLE DESIGN", "28dbda93489e2c45858bd0c04b594ba7"},
                {"ARCHITECTURE STUDY TRIP GRANT", "25742afe23cb9e4ca3bde70e536ebffe"},
                {"ARUP GLOBAL RESEARCH CHALLENGE", "2a81d02ae160aa43945f34eedaa360ef"},
                {"AS DARKNESS DESCENDS", "d88d7f0146df07438a4c7440bd254315"},
                {"ASD CORE STUDIO", "795eb9cdb8392b47b33fd4f5a22eb747"},
                //{"ASD CORE STUDIO", "40fae844c1cd9a4d951451c5fa8cf034"},
                {"ASD CORE SUBJECTS", "5cd6c123254c314db17b5db8771d6423"},
                //{"ASD CORE SUBJECTS", "c168d616b77c8540b6475713dc5ad80c"},
                {"ASD DESIGN CHALLENGE", "4b422ce494a3ab4cb59cc58ce814c48b"},
                {"ASD ELECTIVES", "690532e666c66848953d2a0e39d9b700"},
                //{"ASD ELECTIVES", "5d128d03a166cf44b111691bb60c47c5"},
                {"ASD GRADUATES SHOW", "5f82d5267e6b4343b62c2b199e5e0240"},
                //{"ASD GRADUATES SHOW", "55f13b686bdd2e4f81c66ded1e078dfa"},
                {"ASD OPTION STUDIO", "c7c3651134d9204a980d9c447403a142"},
                //{"ASD OPTION STUDIO", "31a07c5c1e891c42be6664964f625f44"},
                {"ASD OTHER STUDENT EXPENSE", "2aa29aa38effa24786050e05d6026529"},
                //{"ASD OTHER STUDENT EXPENSE", "0459e28e2837124d821abe27e8cd7244"},
                {"ASD PUBLICATION", "6b42ddb05d945043a2695efcbf47b435"},
                {"ASD SUTDIO (I.E. CAMPS, WORKSHOPS)", "ca0cddec8aabfb43be57d6ebbedf1dc0"},
                {"ASD SUTDIO (I.E. CAMPUS, WORKSHOPS)", "81a31595598a9d4aad9e6219cbeee29c"},
                {"ASD-DONATION RECEIVED", "0e5d6e6d00a5a7418cab78abc0eceaa4"},
                {"ASD-ILIGHT EVENT (PICO)", "0eabdeb182e8d1448bea304bff75ff9d"},
                {"ASD-NDC EXHIBITION", "7a2b68e9e421884aa3d508587d546e6b"},
                {"ASEM-DUO SINGAPORE FELLOWSHIP (IN)", "ef8d8ef6a41a424092f6252759497ce8"},
                {"ASIA PACIFIC ASSOCIATN FOR INTL EDU", "9a03fccd9b62a847a1d15c307e1f058e"},
                {"ATHLETICS CLUB", "20338885aaa8a14183928fbf8d4ace37"},
                {"AUTODESK AWARDS AT EPD PILLAR", "0a460bc0bfbd6041a5c890145da29efe"},
                {"AUTODESK BEYOND INDUSTRY 4.0 AWARD", "755263003e11464889d7dcf9be5d1eb0"},
                //{"AUTODESK BEYOND INDUSTRY 4.0 AWARD", "6f7a24c08d73c34cb3df584266b383a0"},
                {"AUTODESK DESIGN CENTRE SPONSORSHIP", "40e9abb6eb855243b67d85f777926bee"},
                {"AXIOM DESIGN IN STRATEGIES SEA", "31492dd9a78e3e42944925645e7c3f98"},
                {"BADMINTON CLUB", "8e0b914b1ae66d4faadd70e3e47ed875"},
                //{"BADMINTON CLUB", "9b282dde473b654a8fe79de3862697fc"},
                {"BALLROOM DANCING CLUB", "b94ed6b25f6a5047ab653d5de97d4328"},
                //{"BALLROOM DANCING CLUB", "d2975b44a6473c41b517f2d245fb49bf"},
                {"BANDS CLUB (SUTD BANDS)", "10627cef4ced794da80cdcc9c2d42ee6"},
                //{"BANDS CLUB (SUTD BANDS)", "859d6d6d28a13943bb7634c97828d640"},
                {"BARCLAYS CAPITAL", "6efd37d1db9a244d8db5f5836b946a13"},
                {"BASEBALL CLUB", "1a896e37e9920944a048fb63cf995eec"},
                {"BASKETBALL CLUB", "e1478506d2ee5744965cedeea8e1907f"},
                //{"BASKETBALL CLUB", "7321480a9befd940b99658d34fd600b6"},
                {"BEAT'S LAB", "99abcbb313a21241a12a9475854dfe2a"},
                {"BEDOK SOUTH SS WORKSHOP-APPINVENTOR", "be91596b0a5bb64db176a02124207ee8"},
                {"BEDOK SOUTH SS WORKSHOP-MAKEY MAKEY", "25a4b0f887044c40991f28f3d0a40bea"},
                {"BILLIARDS CLUB", "1fcfaf6ce4c71d458f1bfc1ae819aafc"},
                //{"BILLIARDS CLUB", "47396cf66a7b854abe8dff2040f20fd0"},
                //{"BILLIARDS CLUB", "db6c9b1ed9d66049b357a51e3aefb2d5"},
                {"BOBBY LIM CHYE HUAT", "3f46c46267e822449abceabdc76eefe1"},
                {"BOND ENFORCEMENT", "728a293429fcef44b67636a920261332"},
                {"BOWLING CLUB", "040710a80f06a1429eabe3a031135ac7"},
                //{"BOWLING CLUB", "524b5920d928f947b45be729e4c25aae"},
                {"BOXING CLUB", "d6f0b16d84aa254d9218796f9c48bb50"},
                //{"BOXING CLUB", "24e0b9ba0d4e0944a9cd084b11c51415"},
                {"BP SPORE-INTRO IS PROG COURSE MERIT", "79935e791e9c27469ad5574813f829d6"},
                {"BUDDHA TOOTH RELIC TEMPLE & MUSEUM", "a7f306ec72795549acf1107f1b44cc26"},
                {"CAMPAIGN 50.5", "7950828c0ea6e34ab92664468609ad09"},
                {"CAP21F001-CODOMO_CREATIVITY HUMANITY", "d5b2237ff4e39941be5f8e0904fd27cb"},
                {"CAP21F002-NORGREN_BIG DATA ANALYSIS", "9003c78f89b8a74ea0e1171ce39aa9e7"},
                {"CAP21F003-CGH_PREVENTING FALLS", "5139a832ce1c9e429bb7c5b4ecc4b748"}
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        

        // 
        foreach (KeyValuePair<string, string> kvp in ClubDict)
        {
            ProjectClubInp.Items.Add(kvp.Key);
        }

    }



    // Populate Entry Page (If entry exists)
    private void PopulateEntry()
    {
        // Populate Metadata fields
        ClaimNameInp.Text = md.ClaimName;
        ClaimDateInp.Date = md.ClaimDate;
        //TODO
        //ProjectClubInp.SelectedItem = Clubmd.ProjectClub;
        ProjectClubInp.SelectedItem = ClubDict.FirstOrDefault(x => x.Value == md.ProjectClub).Key;
        TeamNameInp.Text = md.TeamName;
        Purpose.Text = md.Purpose;
        BudgetEditor.Text = md.EntryBudget.ToString();

        // Entry Budget
        decimal entryBudget = md.EntryBudget;
        decimal currentExpense = 0;
        decimal remainingBudget;

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
        
        // Calculate remaining Budget
        remainingBudget = entryBudget - currentExpense;

        // Populate the UI for entry budget
        CurrentExpenseInput.Text = currentExpense.ToString();
        RemainingBudget.Text = remainingBudget.ToString();

        BuildMDPopulate();
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
        Database.Database.DeleteDirectoryByFilePath(filePath);
        await Shell.Current.GoToAsync(nameof(MainPage));
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
            // Remove receipt from collection
            ReceiptView.Remove(selectedReceipt);
            receipts.Remove(receipt);
            entry.DelRecord(rec);

            // Update remaining budget and remove from entry
            PopulateEntry();


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


    // UTILITIES
    private async void BuildMDPopulate()
    {
        string entryName = EntryName.Text;
        string entryBudgetString = BudgetEditor.Text;
        string claimName = ClaimNameInp.Text;
        DateTime claimDate = ClaimDateInp.Date;
        string purpose = Purpose.Text;
        string projectClub = ClubDict[(string)ProjectClubInp.SelectedItem];
        string teamName = TeamNameInp.Text;
        //string policy = (string)Policy.ItemsSource[Policy.SelectedIndex];

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
            UpdateMetadataButton.IsVisible = true;
            SetMetadataButton.IsVisible = false;

            Show_Message();
            
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
    private async void BuildEntry()
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

    // show success message after setting entry metadata
    private async void Show_Message()
    {
        await UpdateMessage.FadeTo(1, 500);
        await UpdateMessage.FadeTo(0, 4000);
    }

    private async void Concur_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("About to start selenium");
        string cookie = "";
        Process process = new Process();
        //Starting chrome driver
        if (entry == null) {await DisplayAlert("Error", "Entry not saved/ is empty", "OK"); return; }
        else if (receipts.Count == 0) {await DisplayAlert("Error", "No receipts added, please add one before sending to concur", "OK"); return; }

        try
        {
            if(DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst) process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "selenium", "SeleniumWrapperV2");
            else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI) process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "SeleniumWrapper\\SeleniumWrapper.exe";
            //process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..","..","..", "selenium", "SeleniumWrapperV2");
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
            Console.WriteLine("cookie:" + cookie);
            PushToConcur(cookie, receipts, entry);
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "Error extracting cookies, please try again.", "OK");
            Console.WriteLine(ex.ToString());
            return;
        }
        
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
            expense.Cost = receipts[i].ReqAmount;
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
        Process process = new Process();
        try
        {
            string[] CookieSplit = cookie.Split(";");
            string jwt = "", bqrn = "", bqrd = "";
            foreach(string param in CookieSplit)
            {
                string[] temp = param.Split("=");
                if (temp[0].Equals("JWT")) jwt = temp[1];
                else if (temp[0].Equals("OTSESSIONAABQRD")) bqrn = temp[1];
                else if (temp[0].Equals("OTSESSIONAABQRN")) bqrd = temp[1];

            }
            Debug.WriteLine("JWT: " + jwt);
            Debug.WriteLine("BQRN: " + bqrn);
            Debug.WriteLine("BQRD: " + bqrd);
            if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst) process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "publish", "SessionHijackBrowser");
            else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI) process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "CookieBrowser\\CookieBrowser.exe";
            //process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"..","..","..", "selenium", "SeleniumWrapperV2");
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
        //Purpose.Text = await concur.LinkImageToRequest(expense);
    }
}