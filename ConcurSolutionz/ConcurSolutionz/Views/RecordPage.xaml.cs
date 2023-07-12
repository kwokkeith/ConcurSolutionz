﻿using System;
using System.IO;

namespace ConcurSolutionz.Views
{
    [QueryProperty(nameof(ImageFile), "file")]
    public partial class RecordPage : ContentPage
    {
        public string ImageFile
        {
            set
            {
                LoadImage(value);
            }

        }

        private async void LoadImage(string filePath)
        {
            try
            {
                //using (var stream = File.OpenWrite(filePath))
                //{
                //    if(stream != null)
                //    {
                //        ReceiptImage.Source = ImageSource.FromStream(() => stream);
                //    }
                //}
                ReceiptImage.Source = ImageSource.FromFile(filePath);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred while loading the image: " + ex.Message, "OK");
            }
        }

        // Constructor
        public RecordPage()
        {
            // Initialize the XAML components
            InitializeComponent();
        }

		public List<string> getData() {
            string expenseType = ExpenseType.Text;
            string transactionDate = Date.Text;
            string description = Description.Text;

            List<string> data = new List<string>();
            data.Add(expenseType);
            data.Add(transactionDate);
            data.Add(description);

            return data;
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




    }


}
