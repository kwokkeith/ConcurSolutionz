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
        private List<View> boundingBoxes = new List<View>();
        private double imgWidth = 1;
        private double imgHeight = 1;
        private Microsoft.Maui.Controls.Entry lastClicked;
        private Controllers.ReceiptOCR receiptData;

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

        /// <summary>
        /// Method <c>PopulateRecordPage</c> Populates the page with information from the provided Receipt object.
        /// </summary>
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

        /// <summary>
        /// Method <c>CreateExistEntry</c> Converts the provided object into an Entry and assigns it to entryFile.
        /// </summary>
        private void CreateExistEntry(object file)
        {
            try
            {
                FileDB fileDb = (FileDB)file;
                entryFile = FileAdaptor.ConvertFileType(fileDb);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Error converting file: " + ex.ToString(), "OK");
            }
        }

        /// <summary>
        /// Method <c>LoadImage</c> Loads an image from the given path and assigns it as the ReceiptImage's source.
        /// </summary>
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
        }

        /// <summary>
        /// Method <c>GetData</c> Retrieves the user's entered data and returns it as a list of data <expenseType, transactionDate, description>
        /// </summary>
        public List<string> GetData()
        {
            string expenseType = ExpenseType.Text;
            string transactionDate = TransactionDate.Date.ToString();
            string description = DescriptionInp.Text;

            List<string> data = new()
            {
                expenseType,
                transactionDate,
                description
            };

            return data;
        }

        /// <summary>
        /// Method <c>OnFilePickerClicked</c> Event handler for the FilePicker button click event. Opens a file picker to allow the user to select an image file.
        /// </summary>
        public async void OnFilePickerClicked(object sender, EventArgs e)
        {
            // Call the PickAndShow method with the options for picking an image file
            await PickAndShow(
                new PickOptions
                {
                    FileTypes = new FilePickerFileType(
                        new Dictionary<DevicePlatform, IEnumerable<string>>
                        {
                            { DevicePlatform.iOS, new[] { "jpg", "jpeg", "png" } },
                            { DevicePlatform.macOS, new[] { "jpg", "jpeg", "png" } },
                            { DevicePlatform.MacCatalyst, new[] { "jpg", "jpeg", "png" } },
                            { DevicePlatform.Android, new[] { "image/*" } },
                            { DevicePlatform.WinUI, new[] { "jpg", "jpeg", "png" } }
                        }),
                    PickerTitle = "Select an image"
                });
        }

        /// <summary>
        /// Method <c>PickAndShow</c> Picks and shows an image file based on the provided PickOptions.
        /// </summary>
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
                if (ExistingReceipt != null) // If record exist before
                {
                    // Update the receipt image path
                    ExistingReceipt.ImgPath = ImagePath;
                }

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

        /// <summary>
        /// Method <c>BuildNewReceipt</c> Builds and returns a Receipt object using the builder pattern, taking information from the user input.
        /// </summary>
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

        /// <summary>
        /// Method <c>OnOCRButton_Clicked</c> Event handler for the OCR button click event. Uses OCR to extract information from the receipt image and populates the respective fields.
        /// </summary>
        public async void OnOCRButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                OCRButton.IsEnabled = false;
                string tesseractPath = "/Users/pe3nu7/Documents/tesseract/tesseract/tesseract";
                string tessdataPath = "/Users/pe3nu7/Documents/tesseract/tesseract/tessdata";
                receiptData = new(imagePath, tesseractPath, tessdataPath);
                string ReceiptNumber = receiptData.receiptNumber;
                string ReqAmount = Convert.ToString(receiptData.reqAmount);
                List<(Point[], string)> textBoxes = receiptData.textBoxes;
                Grid receiptGrid = ReceiptGrid;
                imgWidth = receiptData.imgWidth;
                imgHeight = receiptData.imgHeight;

                double scaleFactor = ReceiptImage.Width / imgWidth;

                foreach (View v in this.boundingBoxes)
                {
                    receiptGrid.Remove(v);
                }

                foreach ((PointCollection pc, string text) in textBoxes)
                {
                    Microsoft.Maui.Controls.Shapes.Polygon rect = new()
                    {
                        Points = pc,
                        Stroke = Brush.Red,
                        HorizontalOptions = LayoutOptions.Start,
                        IsVisible = true,
                        AnchorX = 0,
                        AnchorY = 0,
                        Scale = scaleFactor
                    };

                    var gestureRecognizer = new TapGestureRecognizer
                    {
                        NumberOfTapsRequired = 1,
                    };
                    gestureRecognizer.Tapped += async (s, e) =>
                    {
                        // If user has yet to select an input field before clicking an ocr scan box
                        if (lastClicked is null)
                        {
                            // Display alert to let user know to select an input field first
                            await DisplayAlert("Select", "Please select an input field first", "OK");
                        }
                        else
                        {
                            Point coords = (PointF)e.GetPosition((View)s);
                            receiptData.RefineOCR(coords.X, coords.Y);
                            lastClicked.Text = receiptData.customRequestText;
                        }
                    };

                    rect.GestureRecognizers.Add(gestureRecognizer);
                    receiptGrid.Add(rect, 0, 2);
                    boundingBoxes.Add(rect);
                }

                ReceiptNo.Text = ReceiptNumber;
                reqAmount.Text = ReqAmount;
                TipLabel.IsVisible = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Falied to call OCR: " + ex, "OK");
                return;
            }
            finally
            {
                OCRButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Method <c>OnSaveDetails_Clicked</c> Event handler for the save details button click event. Saves the current record to the database and navigates back to the entry page.
        /// </summary>
        public async void OnSaveDetails_Clicked(object sender, EventArgs e)
        {
            Receipt receipt;

            // If record exist before
            if (ExistingReceipt != null)
            {
                string receiptFileName;
                receiptFileName = Path.GetFileName(ExistingReceipt.ImgPath);

                FileCreator.CopyFile(
                    ExistingReceipt.ImgPath,
                    Path.Combine(Path.GetTempPath(), receiptFileName));

                entryFile.DelRecordByID(ExistingReceipt.RecordID);
                try
                {
                    receipt = BuildNewReceipt();
                }
                catch (FormatException ex)
                {
                    await DisplayAlert("Invalid Format", ex.Message, "OK");
                    return;

                }
                catch (Exception ex)
                {
                    // If error encountered while building receipt (missing fields, etc.)
                    await DisplayAlert("Error", "Failed to build receipt.\nError msg: " + ex.Message, "OK");
                    return;
                }
                receipt.ImgPath = Path.Combine(Path.GetTempPath(), receiptFileName);
            }
            else // if receipt doesnt exist
            {
                try
                {
                    receipt = BuildNewReceipt();
                }
                catch (Exception ex)
                {
                    // If error encountered while building receipt (missing fields, etc.)
                    await DisplayAlert("Error", "Failed to build receipt.\nError msg: " + ex.Message, "OK");
                    return;
                }
            }


            // Add new record/receipt to the entry object
            entryFile.AddRecord(receipt);

            // go back to entry page
            await Shell.Current.GoToAsync($"..?fileName={entryFile.FileName}&existingFile={true}");
        }

        /// <summary>
        /// Method <c>SpecifyCoords</c> Displays the coordinates of the clicked area in a box
        /// </summary>
        public void SpecifyCoords(object sender, TappedEventArgs e)
        {
            Point? coords = e.GetPosition((View)sender);
            DisplayAlert("DEBUG", coords.ToString(), "OK");
        }

        /// <summary>
        /// Method <c>OnSizeAllocated</c> To handle the event of changing the size of the page. Resizes the bounding boxes of OCR results if present.
        /// </summary>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            Grid receiptGrid = ReceiptGrid;
            if (boundingBoxes.Count > 0)
            {
                try
                {
                    double scaleFactor = ReceiptImage.Width / imgWidth;
                    foreach (View rect in this.boundingBoxes)
                    {
                        rect.Scale = scaleFactor;
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.ToString(), "OK");
                }
            }
        }

        /// <summary>
        /// Method <c>LastClicked</c> Event handler that sets the last clicked entry field in the form. This is used for populating the field with OCR result on user interaction.
        /// </summary>
        void LastClicked(object sender, EventArgs args)
        {
            Microsoft.Maui.Controls.Entry e = (Microsoft.Maui.Controls.Entry)sender;
            this.lastClicked = e;
        }
    }
}
