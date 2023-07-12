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
        string cookie = "OTDefaultLang=en-gb; Login=LoginURL=%2F&LastLoginName=1006345%40mymail.sutd.edu.sg; OTLang=en-gb; akacd_us1=3865976006~rv=52~id=02ea5c609613545bc73c57835d17dbbb; _abck=07ADA0F4C0BB9818C95B117917CAC9A0~0~YAAQLmHBF/+o0gGJAQAAwnPTIwrvE6W/AMrbR0HRMKb0tHN9Inor2FUIa28FmG6KLJUwI7YLil8rAyuBnQ0wtFuLeRMuiFVWnYnk1PORxOiAFpKlUzTlAxi6jrVmY+oaussYM8maNHgcZdpn/LrN1ARVXFdtoGLQBH8bCV57ItilK+J+23zp0B8cI3JZ1GR6NUJMnC5oON6mCmmBs/oKLL8UqYX8xttPTWMxpaAZCdQwGswQAjTGwUjRHL/qYKKx/CcLgUBLBy+EGKznhuuIiPBH/fP4MKHzbaqRAZukQ3cmWNPn+72R7v9pLE7Av8t583cURTIn7JHpmix0OaHyraKmqWaHEWUTGeQIwxk41DSkufSvdyBbFhmXKF/tTE0hTlSI4/qTsFhD4VcA+U+UjY/KADqar7Sc5qUfqa1C6+Hb~-1~-1~-1; bm_sz=DEEF00DA9ED5A2ABB24332ACC1EDD0EB~YAAQLmHBFwKp0gGJAQAAwnPTIxRQ2mq79sCvQ5iBfGRQLSPzAeuOHYcpjJ9625QgV/BqYSqAqmv89zOQbbb5IJm2lGRzqpntkZU3HT0cmTwFsGhm9gmqYjwkAL0SSaG+pRPqQmLLAjXMb23aR/V6NqXb4SMIt6Wwa8q2jUM0INms4N4ZvIOlydMx/E+LHMaw2RpbEjH07tx9CoPZldJZIC5w8FtiAfOgGJOpoIKCxke1yEbP+Fw+Gqj9DtwWEzdIKOy0h+jaet05OHvPnWwExjhCMIrIEgJiBnbuBuXgD0CoF9ZkS4V51vSHjHw=~3618361~3553350; ak_bmsc=5F1822DA657AF884734B79B0CC682CA5~000000000000000000000000000000~YAAQLmHBFyKp0gGJAQAAfHnTIxTZr6hBh4vRCB76oA28T6iDF+Aix8vuldo6r4AAvTwUxAYHsZ/HBxfI0kthZWZyGSJ+fhWz+XN+CwrS9dJilKhiBlVPvTmNpfgYnD+haRoDubiF2ZqPnCSqzn3HpGdV2t5q9JIT+Z4uHK+/yQYW2Bk4dS1PNh/LGK5RwIqUZqlgWOoUOF6B+VCiEnIjrOgAKzFMXMXdfgEepKKGWlqXblaYWU6yBikoVEhQaYDPu5/vTpWxf9k/uTxgAuAKjEymshtydgSWDb8Lj8LPnfb806Yo6wX5XGvTXfZB3FMNBrLjF/dorZfuX3OMghQF0OLf7fX8PQOfFuz9edH7BZpahw3dZHTYM7vyuMLuj0hgz+RpBcAi+qE/seqkUFH8TGwBbjYPX6uoULdBwxbvylPalb8B2eUMpqKuxnn6x9r7wIIvXXrcYLU9U9OzjWAkTBicD++F1i6mcSTne2fbQv9gtxyDYhI0uU0/17G+WdqdK6oe; OTSESSIONAABQRN=3420EC40GBEC7G428CGBF05GF3A2FA85C32D; OTSESSIONAABQRD=3420EC40GBEC7G428CGBF05GF3A2FA85C32D; brandingid=12; AWSALBTG=JXOKCzeIBeDODtWHho/f6HxNLzj6Uunb922UzW7fFw/Bo4Y5w88vOZT7OKACrf6lbVIsFdt+MY4m5p33RmTWagtYC59lCulJy9VheujK5tBdGmRlcAoi1c9l+3nY7//o/WqqGllylA+5NYPflOGzqmhqTJ9p5vzSaMPrVckET7rbPnWH9UA=; AWSALBTGCORS=JXOKCzeIBeDODtWHho/f6HxNLzj6Uunb922UzW7fFw/Bo4Y5w88vOZT7OKACrf6lbVIsFdt+MY4m5p33RmTWagtYC59lCulJy9VheujK5tBdGmRlcAoi1c9l+3nY7//o/WqqGllylA+5NYPflOGzqmhqTJ9p5vzSaMPrVckET7rbPnWH9UA=; JWT=eyJraWQiOiIxNDU1NjE0MDIyIiwiYWxnIjoiUlMyNTYiLCJ0eXAiOiJKV1QifQ.eyJjb25jdXIuc2NvcGVzIjpbIioiXSwiYXVkIjoiKiIsImNvbmN1ci5hcHBJZCI6IjRjYjhjYzYyLWE3MTMtNGEyOC04Y2UwLTRkOTZmNzhjOTgwMyIsInN1YiI6ImZmMjQyY2QxLTU3YzctNDdmYi1iYjMzLTlhZDFlZGIxMzFjMCIsImlzcyI6Imh0dHBzOi8vdXMyLmFwaS5jb25jdXJzb2x1dGlvbnMuY29tIiwiY29uY3VyLnByb2ZpbGUiOiJodHRwczovL3VzMi5hcGkuY29uY3Vyc29sdXRpb25zLmNvbS9wcm9maWxlL3YxL3ByaW5jaXBhbHMvZmYyNDJjZDEtNTdjNy00N2ZiLWJiMzMtOWFkMWVkYjEzMWMwIiwiZXhwIjoxNjg4NTMwMzY2LCJjb25jdXIudmVyc2lvbiI6MywiY29uY3VyLnR5cGUiOiJ1c2VyIiwiY29uY3VyLmFwcCI6Imh0dHBzOi8vdXMyLmFwaS5jb25jdXJzb2x1dGlvbnMuY29tL3Byb2ZpbGUvdjEvYXBwcy80Y2I4Y2M2Mi1hNzEzLTRhMjgtOGNlMC00ZDk2Zjc4Yzk4MDMiLCJuYmYiOjE2ODg1MjY3NTYsImNvbmN1ci5jb21wYW55IjoiMWQ1MmM0YmYtODQ1ZS00NDc3LTkzM2UtM2NiZjcxM2ExMzIxIiwiaWF0IjoxNjg4NTI2NzY2fQ.p9Jy1jY6f7Dp95qinLtl6EG1bOiikr-KPBQBoDe3sxoKRKk44OYverKJTbKeINKLRl22nQuMFjfmwqXshYBHZj84ZBAWWPdRbMlbmnFSihKILEleM9mtes8daYBpF6TkiVIXvuKbThNtbL_K3mTgDZvKWcfDS6IE4-qk7WZYh9gP4xOSKOE0E3kv7E1TxruXMzGMX3tt-4upCUs0CJ23pZVUycR4Y3zWdc-NfZehYtxXMpi_0iffzCzt_f5T6am3Gs4OaGqOphuy9IEN6I43rfzNi5vRollJNWvVlCk6ndEYuxkr5zslfZEnZJdaS5xjqNTQRLEaaHxUu0ApfTnBzA; TAsessionID=e129d468-32e4-420f-b2b5-598e71491341|NEW; bm_sv=932117723425A2F6F004BFF0D92502C4~YAAQnzArFyZijgGJAQAA/wUNJBSO4sJ49ZbTkDdGdndwJHDnFO97EvIMnMoXt79PaKNx+lxfII0hFKFt0WWL8PzA4vhQDERlyPUid/ohRYde+WgGAhMBHRj1q3066eX4dX+8ZFvKY5toVLF3qx29z0YepZcZyeCd8sWgj0TdUvrryKi9SZ5qjhmX0WQGqvKLtkGk67FZy8ryLzpaM0GL+XXXkXxWnTxWdi676cizvuQ5zj0ofGJK50bk+gHm3rlSA07eCoWwxPCNoQ==~1";
        string fileName = "Playmaker.jpg";
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
