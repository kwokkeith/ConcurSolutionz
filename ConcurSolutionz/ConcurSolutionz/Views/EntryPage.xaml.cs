namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
	public EntryPage()
	{
		InitializeComponent();

        //recordCollection.ItemsSource = GetRecords();
        BindingContext = new Models.Entry();
	}

    private async void EditEntryName_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("New entry name", "Alphabets and spaces only", keyboard:Keyboard.Text);
        if(result != null)
        {
            EntryName.Text = result;
        }
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
