using System;
using System.Globalization;
using System.Threading;
using ConcurSolutionz.Database;
using ConcurSolutionz.Views;

namespace ConcurSolutionz.Controllers {

    public class AddRecord {
        public static void addReceipt(Database.Entry entry, List<String> data, String imgPath) {

            // Data to get from OCR
            string tesseractPath = "";
            string tessdataPath = "";
            ReceiptOCR receiptData = new(imgPath, tesseractPath, tessdataPath);
            string ReceiptNumber = receiptData.receiptNumber;
            decimal ReqAmount = receiptData.reqAmount;
            imgPath = receiptData.imgPath;

            // Data from RecordPage
            string expenseType = data[0];
            string transactionDate = data[1];
            string description = data[2];

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

            entry.AddRecord(receipt);
        }
    } 
}
