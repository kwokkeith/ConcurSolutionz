﻿using ConcurSolutionz.Controllers;
using System;
using System.IO;
using ConcurSolutionz.Database;

namespace ConcurSolutionz.Views
{
    [QueryProperty(nameof(EntryFile), "file")]
    [QueryProperty(nameof(ImagePath), "imagePath")]
    [QueryProperty(nameof(ExistingReceipt), "existingReceipt")]
    public partial class RecordPage : ContentPage
    {
        private string imagePath;
        public Database.Entry entryFile { get; set; }
        private Receipt currentReceipt;

        public Receipt ExistingReceipt
        {
            set
            {
                currentReceipt = value;
                if (value != null)
                {
                    PopulateRecordPage(value);
                }
            }
            get
            {
                return currentReceipt;
            }
        }

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

        private void PopulateRecordPage(Receipt receipt)
        {
            ExpenseType.Text = receipt.ExpenseType;
            TransactionDate.Date = receipt.TransactionDate;
            DescriptionInp.Text = receipt.Description;
            SupplierName.Text = receipt.SupplierName;
            reqAmount.Text = receipt.reqAmount.ToString();
            ReceiptNo.Text = receipt.ReceiptNumber;
            CityOfPurchase.SelectedIndex = 0;
            Currency.SelectedIndex = 0;
            ReceiptStatus.Text = receipt.ReceiptStatus;
            Comment.Text = receipt.Comment;
            IsBillable.IsChecked = receipt.IsBillable;
            PersonalExpense.IsChecked = receipt.IsPersonalExpense;
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

            //// Load receipt image into UI
            //LoadImage(ImagePath);

        }

        // Getting user input 
        // returns: a list of data <expenseType, transactionDate, description>
		public List<string> getData() {
            string expenseType = ExpenseType.Text;
            string transactionDate = TransactionDate.Date.ToString();
            string description = DescriptionInp.Text;

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
                    { DevicePlatform.iOS, new[] { "jpg", "jpeg", "png" } },
                    { DevicePlatform.macOS, new[] { "jpg", "jpeg", "png" } },
                    { DevicePlatform.MacCatalyst, new[] { "jpg", "jpeg", "png" } },
                    { DevicePlatform.Android, new[] { "image/*" } },
                    { DevicePlatform.WinUI, new[] { "jpg", "jpeg", "png" } }
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
                .SetCityOfPurchase((string)CityOfPurchase.ItemsSource[CityOfPurchase.SelectedIndex])
                .SetReqAmount(Convert.ToDecimal(reqAmount.Text))
                .SetCurrency((string)Currency.ItemsSource[Currency.SelectedIndex])
                .SetReceiptNumber(ReceiptNo.Text)
                .SetReceiptStatus(ReceiptStatus.Text)
                .SetSupplierName(SupplierName.Text)
                .SetIsBillable(IsBillable.IsChecked)
                .SetIsPersonalExpense(PersonalExpense.IsChecked)
                .SetImgPath(imagePath)
                .SetComment(Comment.Text);
            return builder.Build();
        }

        public async void OnOCRButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string tesseractPath = "./bin/Debug/net7.0-maccatalyst/maccatalyst-x64/tesseract/tesseract";
                string tessdataPath = "./bin/Debug/net7.0-maccatalyst/maccatalyst-x64/tesseract/tessdata";
                Controllers.ReceiptOCR receiptData = new(imagePath, tesseractPath, tessdataPath);
                string ReceiptNumber = receiptData.receiptNumber;
                string ReqAmount = Convert.ToString(receiptData.reqAmount);

                ReceiptNo.Text = ReceiptNumber;
                reqAmount.Text = ReqAmount;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Falied to call OCR: " + ex, "OK");
                return;
            }
            
        }


        public async void OnSaveDetails_Clicked(object sender, EventArgs e)
        {
            if (ExistingReceipt != null) // If record exist before
            {
                entryFile.DelRecordByID(ExistingReceipt.RecordID);
            }

            Receipt receipt;
            try
            {
                receipt = BuildNewReceipt();
            }

            catch (Exception ex)
            {
                // If error encountered while building receipt (missing fields, etc.)
                await DisplayAlert("Error", "Failed to build receipt, Error msg: " + ex, "OK");
                return;
            }

            // Add new record/receipt to the entry object
            entryFile.AddRecord(receipt);

            // go back to entry page
            await Shell.Current.GoToAsync($"..?fileName={entryFile.FileName}&existingFile={true}");
        }
        public void SpecifyCoords(object sender, TappedEventArgs e)
        {
            Point? coords = e.GetPosition((View)sender);
            //DisplayAlert("DEBUG",coords.ToString(),"OK");
            //DisplayAlert("DEBUG",ReceiptImage.Parent.Bounds.ToString(),"OK");
            var overlay = ReceiptOverlay;
            double rectWidth = 80.0;
            double rectHeight = 50.0;
            PointCollection pc = new PointCollection();
            Point center = (Point) coords;
            Point topLeft = new Point(center.X - rectWidth/2, center.Y - rectHeight/2);
            Point topRight = new Point(topLeft.X + rectWidth, topLeft.Y);
            Point bottomRight = new Point(topRight.X, topRight.Y + rectHeight);
            Point bottomLeft = new Point(topLeft.X, topLeft.Y + rectHeight);
            pc.Add(topLeft);
            pc.Add(topRight);
            pc.Add(bottomRight);
            pc.Add(bottomLeft);
            overlay.Points = pc;
            overlay.IsVisible = true;
        }

    }
}
