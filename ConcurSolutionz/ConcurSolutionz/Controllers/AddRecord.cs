using ConcurSolutionz.Database;

namespace ConcurSolutionz.Controllers {

    public class AddRecord {

        public void addReceipt() {
            
            // Data to get from OCR
            ReceiptOCR receiptData = new();
            string ReceiptNumber = receiptData.get();
            decimal ReqAmount = receiptData.get();
            int RecordID = receiptData.get();

            // Remaining data need to get from frontend
            // Possibly through an Entry class

            Receipt.ReceiptBuilder ReceiptBuilder = new();
            Receipt receipt;

            receipt = ReceiptBuilder.SetExpenseType().
                      .SetTransactionDate()
                      .SetDescription()
                      .SetSupplierName()
                      .SetCityOfPurchase()
                      .SetReqAmount(ReqAmount)
                      .SetReceiptNumber(ReceiptNumber)
                      .SetReceiptStatus()
                      .SetImgPath()
                      .Build();
            
            // Get an Entry object from frontend
            Entry entry;
            entry.AddRecord(receipt);

        }
    }   
}