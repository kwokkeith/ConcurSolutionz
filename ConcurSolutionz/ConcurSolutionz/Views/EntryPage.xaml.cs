using ConcurSolutionz.Database;
using ConcurSolutionz.Models;
using System.Globalization;

namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
	public EntryPage()
	{
		InitializeComponent();

		//recordCollection.ItemsSource = GetRecords();
	}

	private async void EditRecord_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(RecordPage));
	}

	//private List<Models.Receipt> GetRecords()
	//{
 //       //return new List<Models.Receipt>
 //       //{
 //       //	new Models.Receipt {RecordName = "Macs", CreationDate="15 June", Amount=100.00},
 //       //	new Models.Receipt {RecordName="Hardware", CreationDate="16 June", Amount=2000 }
 //       //};
        
	//}

    private void GetRecords() { }


	private async void AddRecord_Clicked(object sender, EventArgs e)
	{
        string action = await DisplayActionSheet("Upload an image of your receipt", "Cancel", null, "Upload");
        if(action == "Upload")
        {
            // Call the PickAndShow method with the options for picking an image file
            await PickAndShow(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
                    { DevicePlatform.iOS, new[] { ".jpg", ".jpeg", ".png" } },
                    { DevicePlatform.macOS, new[] { ".jpg", "jpeg", ".png" } },
                    { DevicePlatform.MacCatalyst, new[] { ".jpg", "jpeg", ".png" } },
                    { DevicePlatform.Android, new[] { "image/*" } },
                    { DevicePlatform.WinUI, new[] { ".jpg", "jpeg", ".png" } }
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
                    // Open a stream to the file
                    var stream = await result.OpenReadAsync();

                    // Set the ReceiptImage's source to the picked image
                    //ReceiptImage.Source = ImageSource.FromStream(() => stream);
                    await Shell.Current.GoToAsync(nameof(RecordPage));
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
            await DisplayAlert("Error", "An error occurred while picking the file: " + ex.Message, "OK");

            // Return null because an exception was thrown
            return null;
        }
    }

    //// Event handler for the FilePicker button click event
    //public async void OnFilePickerClicked(object sender, EventArgs e)
    //{
    //    // Call the PickAndShow method with the options for picking an image file
    //    await PickAndShow(new PickOptions
    //    {
    //        FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
    //                { DevicePlatform.iOS, new[] { ".jpg", ".jpeg", ".png" } },
    //                { DevicePlatform.macOS, new[] { ".jpg", "jpeg", ".png" } },
    //                { DevicePlatform.MacCatalyst, new[] { ".jpg", "jpeg", ".png" } },
    //                { DevicePlatform.Android, new[] { "image/*" } },
    //                { DevicePlatform.WinUI, new[] { ".jpg", "jpeg", ".png" } }
    //            }),
    //        PickerTitle = "Select an image"
    //    });
    //}


    private async void Concur_Clicked(object sender, EventArgs e)
    {
        string cookie = "";
        try
        {
            cookie = await DisplayPromptAsync("Cookie", "Please copy the cookie from your browser extension when logged into Concur to proceed");

        }
        catch(Exception ex)
        {
            return;
        }

        StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
        StudentProjectClaimMetaData md;
        Receipt.ReceiptBuilder receiptBuilder = new();
        List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
        Receipt receipt1;
        Database.Entry.EntryBuilder entryBuilder = new();
        Database.Entry entry;

        receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("C:/Playmaker.png")
                .Build();

        records.Add(receipt1);
        records.Add(receipt1);

        md = studentProjMDBuilder
            .SetEntryName("Entry 1")
            .SetEntryBudget(100)
            .SetClaimName("Claim 1")
            .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
            .SetPurpose("Purpose 1")
            .SetTeamName("Team 1")
            .SetProjectClub("5d5d08a511f98e4ab32f28ba68a86350")
            .Build();

        entry = entryBuilder.SetFileName("File 1")
            .SetCreationDate(DateTime.Now)
            .SetFilePath("C:/ConcurTests")
            .SetMetaData(md)
            .SetRecords(records)
            .Build();
        PushToConcur(cookie, records, entry);

    }
    public async void PushToConcur(string cookie, List<Database.Record> records, Database.Entry entry)
    {
        List<Receipt> receipts = new List<Receipt>();
        foreach (Database.Record record in records)
        {
            try
            {
                receipts.Add(RecordAdaptor.ConvertRecord(record));
            }
            catch (Exception ex)
            {
                break;
            }
        }

        //Initialize API caller
        ConcurAPI concur = new ConcurAPI(cookie);
        string init = await concur.Initialize(); // Returns 0 is successful, 1-3 are errors
        debug.Text = "Init status: " + init;
        if (init != "0")
        {
            debug.Text = "Failed to init";
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

        //debug.Text = "Claim key: " + claim.Key;
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
        debug.Text = "RPE Key: ";

        List<Expense> expenseIDs = await concur.GetAllExpenses(claim);

        for (int i = 0; i < expenses.Count; i++)
        {
            for (int j = 0; j < expenseIDs.Count; j++)
            {
                if (expenses[i].RPEKey.Equals(expenseIDs[j].RPEKey))
                {
                    expenses[i].Id = expenseIDs[j].Id;
                    debug.Text = "Expense " + expenses[i].Id + " is in claim " + claim.Id;
                    string filepath = expenses[i].FilePath;
                    string[] split = filepath.Split('/');
                    expenses[i].ImageId = await concur.UploadImage(filepath, split.Last());
                    await concur.LinkImageToRequest(expenses[i]);
                }
            }

        }
        //debug.Text = "image id: " + expense.ImageId;
        //Purpose.Text = await concur.LinkImageToRequest(expense);
    }
}
