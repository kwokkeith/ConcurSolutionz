using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using ConcurSolutionz.Controllers; 
using ConcurSolutionz.Database;
using System.IO;

namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
    // ReceiptView collection for storing and displaying Receipt models
    public ObservableCollection<Models.Receipt> ReceiptView { get; set; }

    public EntryPage()
    {
        // Set the working directory for the database instance
        Database.Database.Instance.Setwd("/Users/hongjing/Downloads");

        InitializeComponent();

        // Instantiate the Receipts collection
        ReceiptView = new ObservableCollection<Models.Receipt>();

        // Creating metadata for student project claim
        StudentProjectClaimMDBuilder studentProjMDBuilder = new();
        StudentProjectClaimMetaData md = studentProjMDBuilder
            .SetEntryName("Entry 1")
            .SetEntryBudget(100)
            .SetClaimName("Claim 1")
            .SetClaimDate(DateTime.ParseExact("12/07/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
            .SetPurpose("Purpose 1")
            .SetTeamName("Team 1")
            .SetProjectClub("Project Club 1")
            .Build();

        // Building a receipt with specific details
        Receipt.ReceiptBuilder receiptBuilder = new();
        Receipt receipt1;
        List<ConcurSolutionz.Database.Record> rec = new();

        receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("/Users/hongjing/Downloads/test.jpeg")
                .Build();

        rec.Add(receipt1);

        // Building an Entry instance with specific details
        ConcurSolutionz.Database.Entry.EntryBuilder entryBuilder = new();
        ConcurSolutionz.Database.Entry entry;

        try
        {
            entry = entryBuilder.SetFileName("File_1")
                                .SetCreationDate(DateTime.Now)
                                .SetLastModifiedDate(DateTime.Now)
                                .SetFilePath("/Users/hongjing/Downloads")
                                .SetMetaData(md)
                                .SetRecords(rec)
                                .Build();
        }
        catch (Exception ex)
        {
            entry = null;
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        // Creating a file in the database
        Database.Database.CreateFile(entry);

        // Convert database records into Receipt instances
        List<ConcurSolutionz.Database.Record> records = entry.GetRecords();
        List<ConcurSolutionz.Database.Receipt> receipts = new();

        for (int i = 0; i < records.Count; i++)
        {
            ConcurSolutionz.Database.Receipt receipt = RecordAdaptor.ConvertRecord(records[i]);
            receipts.Add(receipt);
        }

        // Add converted Receipts to ReceiptView collection
        for (int i = 0; i < receipts.Count; i++)
        {
            string frontEndExpenseType = receipts[i].ExpenseType;
            string frontEndPaymentType = receipts[i].PaymentType;
            string frontEndSupplierName = receipts[i].SupplierName;
            DateTime frontEndTransactionDate = receipts[i].TransactionDate;
            decimal frontEndReqAmount = receipts[i].ReqAmount;
            ReceiptView.Add(new Models.Receipt(new Models.ReceiptBuilder().SetPaymentType(frontEndPaymentType)
                                                                          .SetExpenseType(frontEndExpenseType)
                                                                          .SetSupplierName(frontEndSupplierName)
                                                                          .SetTransactionDate(frontEndTransactionDate)
                                                                          .SetReqAmount(frontEndReqAmount)
                                                                          ));
        }


        // Set the BindingContext of the CollectionView
        recordCollection.BindingContext = this;

        // Set the ItemsSource of the CollectionView
        recordCollection.ItemsSource = ReceiptView;
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
    private async void EditRecord_Clicked(object sender, EventArgs e)
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
                    // pass the file over to the record page
                    await Shell.Current.GoToAsync($"{nameof(RecordPage)}?file={Uri.EscapeDataString(result.FullPath)}");
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

}



