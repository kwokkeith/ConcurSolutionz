using ConcurSolutionz.Controllers;
using System;
using System.IO;
using ConcurSolutionz.Database;

namespace ConcurSolutionz.Views
{
    [QueryProperty(nameof(EntryFile), "file")]
    [QueryProperty(nameof(ImagePath), "imagePath")]
    [QueryProperty(nameof(ExistingRecordBool), "existingRecordBool")]
    //[QueryProperty(nameof(ExistingReceipt), "existingRecord")]
    public partial class RecordPage : ContentPage
    {
        private string imagePath;
        public Database.Entry entryFile { get; set; }
        private bool existingRecord;
        //private Receipt currentReceipt;

        //public Receipt ExistingReceipt
        //{
        //    set
        //    {
        //        currentReceipt = value;
        //    }
        //    get
        //    {
        //        return currentReceipt;
        //    }
        //}

        public Database.Entry EntryFile
        {
            set
            {
                CreateExistEntry(value);
            }
            get
            {
                return entryFile;
            }
        }

        public string ImagePath
        {
            set
            {
                imagePath = Convert.ToString(value);
                LoadImage(imagePath);
            }
            get
            {
                return imagePath;
            }
        }

        public bool ExistingRecordBool
        {
            set
            {
                existingRecord = Convert.ToBoolean(value);
            }
            get
            {
                return existingRecord;
            }
        }


        private void CreateExistEntry(object file)
        {
            try
            {
                FileDB fileDb = (FileDB) file;
                entryFile = FileAdaptor.ConvertFileType(fileDb);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Error converting file: " + ex.ToString(), "OK");
            }
        }


        private async void LoadImage(string imagePath)
        {
            try
            {
                ReceiptImage.Source = ImageSource.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred while loading the image: " + ex.ToString(), "OK");
            }
        }

        // Constructor
        public RecordPage()
        {
            // Initialize the XAML components
            InitializeComponent();
            BindingContext = this;

            // Load receipt image into UI
            LoadImage(ImagePath);

        }

        // Getting user input 
        // returns: a list of data <expenseType, transactionDate, description>
		public List<string> getData() {
            string expenseType = ExpenseType.Text;
            string transactionDate = TransactionDate.Date.ToString();
            string description = Description.Text;

            List<string> data = new List<string>
            {
                expenseType,
                transactionDate,
                description
            };

            return data;
        }


        // Event handler for the FilePicker button click event
        public async void OnFilePickerClicked(object sender, EventArgs e)
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
                        ReceiptImage.Source = ImageSource.FromStream(() => stream);
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

                // Change the image path from result of the user's selected option
                ImagePath = result.FullPath;
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

        private Receipt BuildNewReceipt()
        {
            Receipt.ReceiptBuilder builder = new();
            builder.SetExpenseType(ExpenseType.Text)
                .SetTransactionDate(TransactionDate.Date)
                .SetDescription(DescriptionInp.Text)
                .SetCityOfPurchase(CityOfPurchase.Text)
                .SetReqAmount(Convert.ToDecimal(reqAmount.Text))
                .SetCurrency(Currency.Text)
                .SetReceiptNumber(ReceiptNo.Text)
                .SetReceiptStatus(ReceiptStatus.Text)
                .SetSupplierName(SupplierName.Text)
                .SetIsBillable(IsBillable.IsChecked)
                .SetIsPersonalExpense(PersonalExpense.IsChecked)
                .SetComment(Comment.Text);
            return builder.Build();
        }


        public async void OnSaveDetails_Clicked(object sender, EventArgs e)
        {
            if (ExistingRecordBool) // If record exist before
            {
                //entryFile.DelRecordByID(ExistingReceipt.RecordID);
            }
            Receipt receipt = BuildNewReceipt();

            // Add new record/receipt to the entry object
            entryFile.AddRecord(receipt);

            await Shell.Current.GoToAsync("..");
        }


    }


}
