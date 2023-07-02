using ConcurSolutionz.Database;
using ESC_HTTP_Call.Models;
using static ConcurSolutionz.Database.Receipt;

  namespace ESC_HTTP_Call
  {
      internal class ConcurAPIExample
      {
        public static async void Example(string cookie, string filepath, string filename)
        {
            //Initialize API caller
            ConcurAPI concur = new ConcurAPI(cookie);
            string init = await concur.Initialize(); // Returns 0 is successful, 1-3 are errors
            Console.WriteLine("Init status: " + init);
            //Create new claim
            Claim claim = new Claim();
            claim.Name = "Testing Claim";
            claim.Date = "2023-06-";
            claim.Policy = "5d5d08a511f98e4ab32f28ba68a86350"; //Policy codes can be obtained from concur.ClaimCreateDD();
            claim.TeamName = "Test Team";
            Task<string> createClaim = concur.CreateClaim(claim); //Create a claim on concur and obtain the id
            createClaim.Wait();
            claim.Id = createClaim.Result;
            //Console.WriteLine(claim.Id);
            Task<string> claimKey = concur.GetReportKey(claim.Id);
            claimKey.Wait();
            claim.Key = claimKey.Result;


            //Create new expense
            //Expense expense = new Expense();
            //expense.Date = "2023-06-21";
            //expense.Cost = 69.00;
            //expense.Description = "Test Description";
            //expense.Supplier = "Test Supplier";
            //expense.ReceiptNo = "Test Receipt";
            //expense.Comment = "Test Comment";
            //Expense expense = new Expense();
            //Create Expense
            Expense expense = new Expense();
            expense.Date = "2023-06-21";
            expense.Cost = 69.00;
            expense.Description = "API Test";
            expense.Supplier = "Test Supp";
            expense.ReceiptNo = "0";
            expense.Comment = "API Test";
            expense.ReportId = claim.Id;

            Task<string> createExpense = concur.CreateExpense(expense, claim);
            createExpense.Wait();
            expense.RPEKey = createExpense.Result;
            Console.WriteLine("RPE Key: " + expense.RPEKey);

            Task<List<Expense>> allExpense = concur.GetAllExpenses(claim);
            allExpense.Wait();
            List<Expense> expenses = allExpense.Result;
            for (int i = 0; i < expenses.Count; i++)
            {
                if (expenses[i].RPEKey.Equals(expense.RPEKey)) expense.Id = expenses[i].Id;
                Console.WriteLine("Expense " + expenses[i].RPEKey + " is in claim " + claim.Id);
            }

            //Upload image and link to expense
            Task<string> uploadImage = concur.UploadImage(filepath, filename);
            uploadImage.Wait();
            expense.ImageId = uploadImage.Result;
            Console.WriteLine("image id: " + expense.ImageId);
            Task<string> linkImage = concur.LinkImageToRequest(expense);
            linkImage.Wait();
            Console.WriteLine(linkImage.Result);

            ////Create new Receipt (Updated expense)
            //ReceiptBuilder rBuilder = new ReceiptBuilder();

            ////Convert to Receipt
            //string[] tempDate = expense.Date.Split('-');
            //DateTime dt = new DateTime(Convert.ToInt16(tempDate[0]), Convert.ToInt16(tempDate[1]), Convert.ToInt16(tempDate[2])); //dt.ToString("yyyy-MM-dd")
            //Receipt receipt = new Receipt(new ReceiptBuilder()
            //    .SetTransactionDate(dt)
            //    .SetDescription(expense.Description)
            //    .SetSupplierName(expense.Supplier)
            //    .SetReceiptNumber(expense.ReceiptNo)
            //    .SetComment(expense.Comment)
            //    .SetReportId(expense.ReportId)
            //    .SetRPEKey(expense.RPEKey));
            //receipt.CurrencyAmountSGD = (decimal)expense.Cost;

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
