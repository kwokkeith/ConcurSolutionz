//using ESC_HTTP_Call;
//using ESC_HTTP_Call.Models;

//namespace ESC_HTTP_Call
//{
//    internal class Flow_Example
//    {
//        public static async void Example(string cookie, string filepath, string filename)
//        {
//            //Initialize API caller
//            ConcurAPI concur = new ConcurAPI(cookie);
//            string init = concur.Initialize(); // Returns 0 is successful, 1-3 are errors

//            //Create new claim
//            Claim claim = new Claim();
//            claim.Name = "Testing Claim";
//            claim.Date = "2023-06-";
//            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
//            claim.TeamName = "Test Team";
//            claim.Id = await concur.CreateClaim(claim); //Create a claim on concur and obtain the id

//            //Create new expense
//            Expense expense = new Expense();
//            expense.Date = "2023-06-21";
//            expense.Cost = 69.00;
//            expense.Description = "API Test";
//            expense.Supplier = "Test Supplier";
//            expense.ReceiptNo = "TestReceipt";
//            expense.Comment = "API Test";
//            expense.ReportId = claim.Id;
//            expense.RPEKey = await concur.CreateExpense(expense, claim);

//            //Upload image and link to expense
//            expense.ImageId = await concur.UploadImage(filepath, filename);
//        }
//    }
//}
