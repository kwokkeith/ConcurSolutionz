using ConcurSolutionz;
using ConcurSolutionz.Database;
using ConcurSolutionz.Models;
using static ConcurSolutionz.Database.Receipt;

namespace ConcurSolutionz
{
    internal class Flow_Example
    {
        public static async void Example(string cookie, string filepath, string filename)
        {
            //Initialize API caller
            ConcurAPI concur = new ConcurAPI(cookie);
            string init = concur.Initialize(); // Returns 0 is successful, 1-3 are errors

            //Create new claim
            Claim claim = new Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            claim.Id = await concur.CreateClaim(claim); //Create a claim on concur and obtain the id

            //Create new expense
            Expense expense = new Expense();
            expense.Date = "2023-06-21";
            expense.Cost = 69.00;
            expense.Description = "Test Description";
            expense.Supplier = "Test Supplier";
            expense.ReceiptNo = "Test Receipt";
            expense.Comment = "Test Comment";
            expense.ReportId = claim.Id;
            expense.RPEKey = await concur.CreateExpense(expense, claim);

            //Upload image and link to expense
            expense.ImageId = await concur.UploadImage(filepath, filename);
            await concur.LinkImageToRequest(expense);

            //Create new Receipt (Updated expense)
            ReceiptBuilder rBuilder = new ReceiptBuilder();

            //Convert to Receipt
            string[] tempDate = expense.Date.Split('-');
            DateTime dt = new DateTime(Convert.ToInt16(tempDate[0]), Convert.ToInt16(tempDate[1]), Convert.ToInt16(tempDate[2])); //dt.ToString("yyyy-MM-dd")
            Receipt receipt = new Receipt(new ReceiptBuilder()
                .SetTransactionDate(dt)
                //.SetCurrencyAmountSGD((decimal)expense.Cost)
                .SetDescription(expense.Description)
                .SetSupplierName(expense.Supplier)
                .SetReceiptNumber(expense.ReceiptNo)
                .SetComment(expense.Comment)
                .SetReportId(expense.ReportId)
                .SetRPEKey(expense.RPEKey));


            //rBuilder = rBuilder.SetTransactionDate(dt);
            //rBuilder = rBuilder.SetCurrencyAmountSGD((decimal)69.00);
            //string description = "Test Description";
            //rBuilder = rBuilder.SetDescription(description);
            //rBuilder = rBuilder.SetSupplierName("Test Supplier");
            //rBuilder = rBuilder.SetReceiptNumber("Test Receipt No");
            //rBuilder = rBuilder.SetComment("Test Comment");
            //rBuilder = rBuilder.SetId(claim.Id);
            //Receipt tempReceipt = new Receipt(rBuilder);
            //rBuilder = rBuilder.SetRPEKey(await concur.CreateExpense(tempReceipt, claim));
            //rBuilder = rBuilder.SetImgId(await concur.UploadImage(filepath, filename));
        }
    }
}
