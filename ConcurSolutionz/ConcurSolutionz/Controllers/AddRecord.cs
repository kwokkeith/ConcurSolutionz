using System;
using System.Globalization;
using System.Threading;
using ConcurSolutionz.Database;
using ConcurSolutionz.Views;

namespace ConcurSolutionz.Controllers {

    public class AddRecord {

        public void addReceipt() {

            // Data to get from OCR
            ConcurSolutionz.Database.RecieptOCR receiptData = new();
            string ReceiptNumber = receiptData.receiptNumber;
            decimal ReqAmount = receiptData.reqAmount;
            // int RecordID = receiptData.get();
            string imgPath = receiptData.imgPath;

            // Data from RecordPage
            List<string> data = ConcurSolutionz.Views.RecordPage.getData();
            string expenseType = data[0];
            string transactionDate = data[1];
            string description = data[2];

            // string expenseType = "Hello";
            // string transactionDate = "10/07/2023";
            // string description = "Trial Claim";

            Receipt.ReceiptBuilder ReceiptBuilder = new();
            Receipt receipt;

            receipt = ReceiptBuilder.SetExpenseType(expenseType)
                      .SetTransactionDate(DateTime.ParseExact(transactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                      .SetDescription(description)
                      .SetSupplierName("")
                      .SetCityOfPurchase("Singapore, SINGAPORE")
                      .SetReqAmount(ReqAmount)
                      .SetReceiptNumber(ReceiptNumber)
                      .SetReceiptStatus("")
                      .SetImgPath(imgPath)
                      .Build();

            // Creating an Entry
            StudentProjectClaimMDBuilder studentProjMDBuilder = new StudentProjectClaimMDBuilder();
            StudentProjectClaimMetaData md;

            md = studentProjMDBuilder
                .SetEntryName("Entry 1")
                .SetEntryBudget(100)
                .SetClaimName("Claim 1")
                .SetClaimDate(DateTime.ParseExact(transactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetPurpose("Purpose 1")
                .SetTeamName("Team 1")
                .SetProjectClub("Project Club 1")
                .Build();

            ConcurSolutionz.Database.Entry.EntryBuilder entryBuilder = new();
            ConcurSolutionz.Database.Entry entry;

            List<ConcurSolutionz.Database.Record> records = new List<ConcurSolutionz.Database.Record>();

            entry = entryBuilder.SetFileName("File 1")
                .SetCreationDate(DateTime.Now)
                .SetFilePath("C:/ConcurTests/EntryTest.fdr")
                .SetMetaData(md)
                .SetRecords(records)
                .Build();
            FileCreator.CreateFile(entry);

            entry.AddRecord(receipt);

        }
    } 
}