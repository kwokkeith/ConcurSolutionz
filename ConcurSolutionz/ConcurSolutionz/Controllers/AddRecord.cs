<<<<<<< HEAD
//using ConcurSolutionz.Database;
=======
using System;
using System.Globalization;
using System.Threading;
using ConcurSolutionz.Database;
using ConcurSolutionz.Views;
using ConcurSolutionz.OCR;
>>>>>>> AddRecord

//namespace ConcurSolutionz.Controllers {

<<<<<<< HEAD
//    public class AddRecord {

//        public void addReceipt() {
            
//            // Data to get from OCR
//            ReceiptOCR receiptData = new();
//            string ReceiptNumber = receiptData.get();
//            decimal ReqAmount = receiptData.get();
//            int RecordID = receiptData.get();

//            // Remaining data need to get from frontend
//            // Possibly through an Entry class
=======
    public class AddRecord {
        //public string frontEndExpenseType;
        //public string frontEndPaymentType;
        //public string frontEndSupplierName;
        //public DateTime frontEndTransactionDate;
        //public decimal frontEndReqAmount;

        //public string ExpenseType { get; internal set; }
        //public string PaymentType { get; internal set; }
        //public string SupplierName { get; internal set; }
        //public DateTime TransactionDate { get; internal set; }

        public void addReceipt() {

            // Data to get from OCR
            string imgPath = "";
            string tesseractPath = "";
            ConcurSolutionz.OCR.RecieptOCR receiptData = new(imgPath, tesseractPath);
            string ReceiptNumber = receiptData.receiptNumber;
            decimal ReqAmount = receiptData.reqAmount;
            // int RecordID = receiptData.get();
            // string imgPath = receiptData.imgPath;
            imgPath = receiptData.imgPath;

            // Data from RecordPage
            RecordPage recordPageData = new ConcurSolutionz.Views.RecordPage();
            List<string> data = recordPageData.getData();
            string expenseType = data[0];
            string transactionDate = data[1];
            string description = data[2];

            // string expenseType = "Hello";
            // string transactionDate = "10/07/2023";
            // string description = "Trial Claim";
>>>>>>> AddRecord

//            Receipt.ReceiptBuilder ReceiptBuilder = new();
//            Receipt receipt;

<<<<<<< HEAD
//            receipt = ReceiptBuilder.SetExpenseType().
//                      .SetTransactionDate()
//                      .SetDescription()
//                      .SetSupplierName()
//                      .SetCityOfPurchase()
//                      .SetReqAmount(ReqAmount)
//                      .SetReceiptNumber(ReceiptNumber)
//                      .SetReceiptStatus()
//                      .SetImgPath()
//                      .Build();
            
//            // Get an Entry object from frontend
//            Entry entry;
//            entry.AddRecord(receipt);

//        }
//    }   
//}
=======
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

            //frontEndExpenseType = receipt.ExpenseType;
            //frontEndPaymentType = receipt.PaymentType;
            //frontEndSupplierName = receipt.SupplierName;
            //frontEndTransactionDate = receipt.TransactionDate;
            //frontEndReqAmount = receipt.ReqAmount;

        }
    } 
}
>>>>>>> AddRecord
