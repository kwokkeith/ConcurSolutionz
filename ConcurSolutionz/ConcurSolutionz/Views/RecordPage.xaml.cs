namespace ConcurSolutionz.Views;

public partial class RecordPage : ContentPage
{
	public RecordPage()
	{
		InitializeComponent();
	}

    //public async Task<FileResult> PickandShow(PickOptions options)
    //{
    //    try
    //    {
    //        var result = await FilePicker.Default.PickAsync(options);
    //        if (result != null)
    //        {
    //            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
    //                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
    //            {
    //                using var stream = await result.OpenReadAsync();
    //                var image = ImageSource.FromStream(() => stream);
    //            }
    //        }

    //        return result;
    //    }
    //    catch (Exception ex)
    //    {
    //        // The user canceled or something went wrong
            
    //    }

    //    return null;
    //}

    private async void OnFilePickerClicked(object sender, EventArgs e)
    {


        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> { { DevicePlatform.iOS, new[] { ".png" } }, { DevicePlatform.macOS, new[] { ".png" } }, { DevicePlatform.MacCatalyst, new[] { ".png" } } }),
            PickerTitle = "Select an image"
        }); 

        if(result == null)
        {

            return;
        }

        var stream = await result.OpenReadAsync();
        ReceiptImage.Source = ImageSource.FromStream(() => stream);



    }
        
}
