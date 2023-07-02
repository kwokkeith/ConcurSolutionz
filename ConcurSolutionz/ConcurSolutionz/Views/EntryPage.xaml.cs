using ESC_HTTP_Call;
using ESC_HTTP_Call.Models;
using System.Diagnostics;
using System.Net;

namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
	public EntryPage()
	{
		InitializeComponent();

		
	}

	private async void EditRecord_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(RecordPage));
	}

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
    private async void Concur_Clicked(object sender, EventArgs e)
    {
        string cookie = Purpose.Text;
        string fileName = "Playmaker.png";
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName;
        debug.Text = filePath;
        CreateDummyRequest(cookie, filePath, fileName);
        //ConcurAPIExample.Example(cookie, filePath, fileName);
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
    public async void CreateDummyRequest(string cookie, string filepath, string filename)
    {
        //Initialize API caller
        ConcurAPI concur = new ConcurAPI(cookie);
        string init = await concur.Initialize(); // Returns 0 is successful, 1-3 are errors
        debug.Text = "Init status: " + init;
        if (init !="0") 
        {
            debug.Text = "Failed to init";
            return;
        }
        //Create new claim
        Claim claim = new Claim();
        claim.Name = "Testing Claim";
        claim.Date = "2023-06-";
        claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
        claim.TeamName = "Test Team";
        claim.Id = await concur.CreateClaim(claim);
        claim.Key = await concur.GetReportKey(claim.Id);

        debug.Text = "Claim key: " + claim.Key;

        Expense expense = new Expense();
        expense.Date = "2023-06-21";
        expense.Cost = 69.00;
        expense.Description = "API Test";
        expense.Supplier = "Test Supp";
        expense.ReceiptNo = "0";
        expense.Comment = "API Test";
        expense.ReportId = claim.Id;
        expense.RPEKey = await concur.CreateExpense(expense, claim);

        debug.Text = "RPE Key: " + expense.RPEKey;

        List<Expense> expenses = await concur.GetAllExpenses(claim);
        for (int i = 0; i < expenses.Count; i++)
        {
            if (expenses[i].RPEKey.Equals(expense.RPEKey)) 
            {
                expense.Id = expenses[i].Id;
                debug.Text = "Expense " + expense.Id + " is in claim " + claim.Id;
            } 
        }
        if (string.IsNullOrEmpty(expense.Id)) throw new Exception("Cannot find id");

        //Upload image and link to expense
        expense.ImageId = await concur.UploadImage(filepath, filename);
        debug.Text = "image id: " + expense.ImageId;
        Purpose.Text = await concur.LinkImageToRequest(expense);
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


}
