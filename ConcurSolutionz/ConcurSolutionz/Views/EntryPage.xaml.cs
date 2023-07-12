using ConcurSolutionz.Database;
using ConcurSolutionz.Models;
using System.Globalization;

namespace ConcurSolutionz.Views;

public partial class EntryPage : ContentPage
{
	public EntryPage()
	{
		InitializeComponent();

		//recordCollection.ItemsSource = GetRecords();
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


    private async void Concur_Clicked(object sender, EventArgs e)
    {
        string cookie = "OTDefaultLang=en-gb; OTLang=en-gb; akacd_us1=3866593019~rv=17~id=3792605ed67c4bbdc61ac537e0274e54; _abck=07ADA0F4C0BB9818C95B117917CAC9A0~0~YAAQVOgtF/lOFyeJAQAANlOaSApRLYKj+ZLMY+GvyQb+CDUtWtsYnIg9p+ARpejZ4c+j4Vi0SsCOaiPEZebNTKIfeyjBbyjcTYFD6V8LxvRrpnDwYXSyU0Qa3eI8JATiNtxpccztUB+7eeWoVDpAsUhuXUqs1J4BF9KjGKwIHrvh0Lhg1N81fvNz4Vf2OL9Mm5hD42zy4wFXsrAOa10Njr+0yclzJ5302Y82gRKhgk35y4+xQfd69zs8HTlKLhUaevXg55BmZ0JAZeXyKUv+eCpIHx2V59Pfa/GDcMC0rfELCnODM5DmQjoXpS2n/X5En19hQb1XdqCMmF3QkLKNYqDJxrkXiqdbsncmnNRBVRHGMkKvXwoZY9ccwGTsltUQxOhxWOVjZDbydacAq/2a5foBI4vg9DLs7c/4EfgI4Yo2~-1~-1~-1; Login=LoginURL=%2F&LastLoginName=1006345%40mymail.sutd.edu.sg; OTSESSIONAABQRN=101A9C55G9317G409FGBC1AG3C3AC3EBC74D; OTSESSIONAABQRD=101A9C55G9317G409FGBC1AG3C3AC3EBC74D; brandingid=12; bm_mi=F8450C8237C5C5D840EF9CDB82789BE6~YAAQVOgtF/b6GieJAQAAkNwWSRSj1PsWGiNrbim4haW/U0PS4kBoxvz4hnxLdzJIuclUPTq2jGg4QAcYsDWwD99+hgncyGaU4DcTnLoHEBmzphpoVvtsMmpinhVkEZTvNPWJgXwLP1ya+qpv1GuDbo4/IdkK+tTT8uB9ZFo0Sh6tcz1N6mEJrwKmXoZh7l9IhL2vh8+XU5qnlRW2yoSpa5UJC9fhJMBEYVNl15b6DI3rdg3ACqjVhu2W9imV1itgoEgQnZYOXYNAw2xoiIbXZ00XLmPmz6MvJgcGnDYYtk41mElwbwi7sg8mr9ToWgc34nzVzDltThCoxjc=~1; ak_bmsc=49F4CF091FD41E4BB297C07DCDB82CBE~000000000000000000000000000000~YAAQVOgtF4v7GieJAQAACPAWSRQWHWNq2lRptu1j1yB+z3IG7tRIC8uxi5WZBIrcv9VNclKP6cB4eyEGNXtzC4AHMUDFRBTYpUmh5oCupw0qjda7uBVBkbGb4VUlleDS3TNrDekSs8I8uq8rFX4vkzeXuRCp8wX95YouOwIIq8OSCUAXXDuSJFosWFHPqgnnjYJISTiP/7cNY4O2hYHk3GUntBtK4hOEpq3pRUpeL33JAY/SfyYyoFmA3Mo2ifAXSFKxnvXL1CpdQuyzMpsNlo8BKy1kFM4iIpOS3TJdd1mSZcy/aq5u/Up70bI+20+J3Kn7net7cyKR1DKTkMlInUodR2GU5hQ7oziCbqqhTelRnqrW7lZbJp9VFR9yKmzbpu37/mU6UbtGAN0bHBFMksokKgOUQa5LHvhp5SJlvhd44+E6Htb8YpSNN+O6NjekJRqk2oNpSndddySv3+UsVBYGmQz/0ws6a0stTEGhrPMXj4ZK+JxRPICsBhP4482Seaew9RsAFiHn0Y3bm42tYwKJoxIPt3Pa/g==; JWT=eyJraWQiOiIxNDU1NjE0MDIyIiwiYWxnIjoiUlMyNTYiLCJ0eXAiOiJKV1QifQ.eyJjb25jdXIuc2NvcGVzIjpbIioiXSwiYXVkIjoiKiIsImNvbmN1ci5hcHBJZCI6IjRjYjhjYzYyLWE3MTMtNGEyOC04Y2UwLTRkOTZmNzhjOTgwMyIsInN1YiI6ImZmMjQyY2QxLTU3YzctNDdmYi1iYjMzLTlhZDFlZGIxMzFjMCIsImlzcyI6Imh0dHBzOi8vdXMyLmFwaS5jb25jdXJzb2x1dGlvbnMuY29tIiwiY29uY3VyLnByb2ZpbGUiOiJodHRwczovL3VzMi5hcGkuY29uY3Vyc29sdXRpb25zLmNvbS9wcm9maWxlL3YxL3ByaW5jaXBhbHMvZmYyNDJjZDEtNTdjNy00N2ZiLWJiMzMtOWFkMWVkYjEzMWMwIiwiZXhwIjoxNjg5MTU0MzE5LCJjb25jdXIudmVyc2lvbiI6MywiY29uY3VyLnR5cGUiOiJ1c2VyIiwiY29uY3VyLmFwcCI6Imh0dHBzOi8vdXMyLmFwaS5jb25jdXJzb2x1dGlvbnMuY29tL3Byb2ZpbGUvdjEvYXBwcy80Y2I4Y2M2Mi1hNzEzLTRhMjgtOGNlMC00ZDk2Zjc4Yzk4MDMiLCJuYmYiOjE2ODkxNTA3MDksImNvbmN1ci5jb21wYW55IjoiMWQ1MmM0YmYtODQ1ZS00NDc3LTkzM2UtM2NiZjcxM2ExMzIxIiwiaWF0IjoxNjg5MTUwNzE5fQ.FngUg5b5SbRlyiv6YWK2rc1HqTyuCMIpO529MHEF6NtBky5ZQ06SvXg5Wf2Z8xmq-UjjcB4o4gmu5vpWR6MV08oF-aYkBIMK-NcI8VKr-mblFqZSoZgerTjXZh3kLX93-o_saMAoHxfbAsJiNkK5Ggep5awf00RYviP5MxmEJC9y19f6qHzVUlgVYYiuPQtwaZEpoldbcUch3vyqlmTXJJWi7PtTkD1HQ4Ct2NsoXgOw2c87p7X9yfPudfyPAkaerFicmwFpo3KGoHESYZaAuhnmB2eY9nSuVUF8VkFKm5ygRbiZaxphhWRakYYSLQo1Ojq4uqjiJw2jdZBTkGIDDg; bm_sz=A27565527C1ABDA8E5A58A86FBACF454~YAAQFAvGF0hJEgOJAQAAmmdpSRTl0ZuBWmsyHgzJ3nrXo0dv6WVL4tQkMrxylU7OmEAMyPy8oGzAHhrIMDTOxneyan2KYr7G3InMTJcQ1i/A1Nc4ttJ+MApqa0RC6ScjXC7Gl557u3W7HGxSwvmhM0w1mX4ISl8QhdUPZ9mgLe+61nfCG3E/x701n8+hJiLhtVtfjYdt3wwqUCIDXKsHxoSvZHbSJZBMLdjP6bJXAjfJcIZo+RAbn2/q5sttcW9dbq+7Mh74F6MJwaTO/lBNuXEjvDOIPKWw3oduhOzF0yoiAMWhUUslOcPFaW0M0UHSeJa3TUwuw5gsQPTNyjZCN2UmAwE=~4474425~4404791; TAsessionID=952b9586-97f4-49e6-be05-09cfcaa7ed6a|NEW; AWSALBTG=CAZUiNzZJd4x2FdQTlwr06fHJiA3s9KZfFF9Cc1/K3gSbn3/7pYEVbhdXv5NZr2/fy+hokzcsj9WWCi/RXcwGQkANH0D380Bd2kLQzu5bIK7M8664wzZHTgM1tQF56G7tl1YEdUpujgxK7QaObMLVUVa/zO0kN7YWJNewV5THuCY6l/j1IEgrFGahLzUxwEF8jUgV4UAoS+5SZJYpTx8DARVuqVl3JKfzbFQBHfw/nhPrjSuybfWCWs8te4gEevBSy4XVtmcMGXgWQ==; AWSALBTGCORS=CAZUiNzZJd4x2FdQTlwr06fHJiA3s9KZfFF9Cc1/K3gSbn3/7pYEVbhdXv5NZr2/fy+hokzcsj9WWCi/RXcwGQkANH0D380Bd2kLQzu5bIK7M8664wzZHTgM1tQF56G7tl1YEdUpujgxK7QaObMLVUVa/zO0kN7YWJNewV5THuCY6l/j1IEgrFGahLzUxwEF8jUgV4UAoS+5SZJYpTx8DARVuqVl3JKfzbFQBHfw/nhPrjSuybfWCWs8te4gEevBSy4XVtmcMGXgWQ==; bm_sv=4821530B9FDB42DBEB8266C32C8E29FA~YAAQENYsF4OaoEOJAQAAul5vSRT9Ua/q8f1xF5xY58nt4/tGdPs/babWAf6dpxYr+WPLuAWu4PAXhiPtYTv08j2Z76brtmhbdglz5bTNjuilOVH4jIh0iyhHnXT7psa6dSSgYwpyHyZRw8IJSNeWQwdN8JGYH6rw2OtDpepNJsQITTFrZSEsmf+Vxg2bt9vI7S0No1zzYv6sKb0f57EifUyKOqyjkloBuQWNaK0ashKKwKTjYhaifA5zSGeJ9vDhi7lV5OodK+KoAA==~1";


        StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
        StudentProjectClaimMetaData md;
        Receipt.ReceiptBuilder receiptBuilder = new();
        List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();
        Receipt receipt1;
        Database.Entry.EntryBuilder entryBuilder = new();
        Database.Entry entry;

        receipt1 = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("C:/Playmaker.jpg")
                .Build();

        records.Add(receipt1);
        records.Add(receipt1);

        md = studentProjMDBuilder
            .SetEntryName("Entry 1")
            .SetEntryBudget(100)
            .SetClaimName("Claim 1")
            .SetClaimDate(DateTime.ParseExact("10/02/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture))
            .SetPurpose("Purpose 1")
            .SetTeamName("Team 1")
            .SetProjectClub("Project Club 1")
            .Build();

        entry = entryBuilder.SetFileName("File 1")
            .SetCreationDate(DateTime.Now)
            .SetFilePath("C:/ConcurTests")
            .SetMetaData(md)
            .SetRecords(records)
            .Build();
        PushToConcur(cookie, records, entry);

    }
    public async void PushToConcur(string cookie, List<Database.Record> records, Database.Entry entry)
    {
        List<Receipt> receipts = new List<Receipt>();
        foreach (Database.Record record in records)
        {
            try
            {
                receipts.Add(RecordAdaptor.ConvertRecord(record));
            }
            catch (Exception ex)
            {
                break;
            }
        }

        //Initialize API caller
        ConcurAPI concur = new ConcurAPI(cookie);
        string init = concur.Initialize(); // Returns 0 is successful, 1-3 are errors
        debug.Text = "Init status: " + init;
        if (init != "0")
        {
            debug.Text = "Failed to init";
            throw new Exception("Failed to initialize API");
        }
        //Create new claim
        StudentProjectClaimMetaData MD = (StudentProjectClaimMetaData)entry.MetaData;
        Claim claim = new Claim();
        claim.Name = MD.ClaimName;
        claim.Date = MD.ClaimDate.ToString("yyyy-MM-dd");
        claim.Policy = MD.Policy;
        claim.TeamName = MD.TeamName;
        claim.Id = await concur.CreateClaim(claim);
        claim.Key = await concur.GetReportKey(claim.Id);

        //debug.Text = "Claim key: " + claim.Key;
        List<Expense> expenses = new List<Expense>();

        for (int i = 0; i < receipts.Count; i++)
        {
            Expense expense = new Expense();
            expense.Date = receipts[i].TransactionDate.ToString("yyyy-MM-dd");
            expense.Cost = receipts[i].CurrencyAmountSGD;
            expense.Description = receipts[i].Description;
            expense.Supplier = receipts[i].SupplierName;
            expense.ReceiptNo = receipts[i].ReceiptNumber;
            expense.Comment = receipts[i].Comment;
            expense.ReportId = claim.Id;
            expense.FilePath = receipts[i].ImgPath;
            expense.RPEKey = await concur.CreateExpense(expense, claim);

            expenses.Add(expense);
        }
        debug.Text = "RPE Key: ";

        List<Expense> expenseIDs = await concur.GetAllExpenses(claim);

        for (int i = 0; i < expenses.Count; i++)
        {
            for (int j = 0; j < expenseIDs.Count; j++)
            {
                if (expenses[i].RPEKey.Equals(expenseIDs[j].RPEKey))
                {
                    expenses[i].Id = expenseIDs[j].Id;
                    debug.Text = "Expense " + expenses[i].Id + " is in claim " + claim.Id;
                    string filepath = expenses[i].FilePath;
                    string[] split = filepath.Split('/');
                    expenses[i].ImageId = await concur.UploadImage(filepath, split.Last());
                    await concur.LinkImageToRequest(expenses[i]);
                }
            }

        }
        //debug.Text = "image id: " + expense.ImageId;
        //Purpose.Text = await concur.LinkImageToRequest(expense);
    }
}
