using ConcurSolutionz;
using ConcurSolutionz.Database;
using ConcurSolutionz.Models;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Net;
using static ConcurSolutionz.Database.Receipt;
using Entry = ConcurSolutionz.Database.Entry;

namespace ConcurSolutionz
  {
      internal class Flow_Example
      {
          public static async void Example(string cookie, string filepath, string filename)
          {
                

              //Initialize API caller
              ConcurAPI concur = new ConcurAPI(cookie);
              string init = await concur.Initialize(); // Returns 0 is successful, 1-3 are errors

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
            expense.Cost = Convert.ToDecimal(69.00);
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
          }
        public async void Initialize(string cookie, List<Database.Record> records, Entry entry)
        {
            List<Receipt> receipts = new List<Receipt>();
            foreach(Database.Record record in records)
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
            string init = await concur.Initialize(); // Returns 0 is successful, 1-3 are errors
            //debug.Text = "Init status: " + init;
            if (init != "0")
            {
                //debug.Text = "Failed to init";
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

            for(int i = 0;  i < receipts.Count; i++)
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
            //debug.Text = "RPE Key: " + expense.RPEKey;

            List<Expense> expenseIDs = await concur.GetAllExpenses(claim);

            for (int i = 0; i < expenses.Count; i++)
            {
                for (int j = 0; j < expenseIDs.Count; j++)
                    {
                        if (expenses[i].RPEKey.Equals(expenseIDs[j].RPEKey))
                        {
                            expenses[i].Id = expenseIDs[j].Id;
                            //debug.Text = "Expense " + expense.Id + " is in claim " + claim.Id;
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
}
