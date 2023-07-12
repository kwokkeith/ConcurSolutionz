using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using ConcurSolutionz.Controllers;
using ConcurSolutionz.Database;
using System.IO;  

namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
    public ObservableCollection<Models.Receipt> ReceiptView { get; set; } // Creation of the Receipts observable collection 


    public EntryPage()
    {
        Database.Database.Instance.Setwd("/Users/hongjing/Downloads");

        InitializeComponent();

        // Instantiate the Receipts collection
        ReceiptView = new ObservableCollection<Models.Receipt>();

        StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
        StudentProjectClaimMetaData md;

        md = studentProjMDBuilder
            .SetEntryName("Entry 1")
            .SetEntryBudget(100)
            .SetClaimName("Claim 1")
            .SetClaimDate(DateTime.ParseExact("12/07/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
            .SetPurpose("Purpose 1")
            .SetTeamName("Team 1")
            .SetProjectClub("Project Club 1")
            .Build();


        Receipt.ReceiptBuilder receiptBuilder = new();
        Receipt receipt1;

        List<ConcurSolutionz.Database.Record> rec = new List<ConcurSolutionz.Database.Record>();

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

        ConcurSolutionz.Database.Entry.EntryBuilder entryBuilder = new();
        ConcurSolutionz.Database.Entry entry;

        //Directory.CreateDirectory("/Users/hongjing/Downloads/File_1");
        // .SetMetaData(md)

        try
        {
            entry = entryBuilder.SetFileName("File_1")
                                .SetCreationDate(DateTime.Now)
                                .SetLastModifiedDate(DateTime.Now)
                                .SetFilePath("/Users/hongjing/Downloads")
                                .SetMetaData(md)
                                .SetRecords(rec)
                                .Build();
            Console.WriteLine("HI1");
            Console.WriteLine(entry.MetaData);
        }
        catch (Exception ex)
        {
            entry = null;
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Database.Database.CreateFile(entry);



        List<ConcurSolutionz.Database.Record> records = entry.GetRecords();
        List<ConcurSolutionz.Database.Receipt> receipts = new List<ConcurSolutionz.Database.Receipt>();

        for (int i = 0; i < records.Count; i++)
        {
            ConcurSolutionz.Database.Receipt receipt = RecordAdaptor.ConvertRecord(records[i]);
            receipts.Add(receipt);
        }

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

    private async void EditEntryName_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("New entry name", "Alphabets and spaces only", keyboard: Keyboard.Text);
            if (result != null)
            {
                EntryName.Text = result;
            }
        }

     async void EditRecord_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RecordPage));
        }

        //private List<Models.Receipt> GetRecords() // need the database to build the receipt

        //{
        //    //return new List<Models.Receipt>
        //    //{
        //    //	new Models.Receipt {RecordName = "Macs", CreationDate="15 June", Amount=100.00},
        //    //	new Models.Receipt {RecordName="Hardware", CreationDate="16 June", Amount=2000 }
        //    //};

        //}





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


