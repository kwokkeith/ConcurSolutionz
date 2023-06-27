using System;
using System.IO;
using Record = ConcurSolutionz.Database.Record;

namespace ConcurSolutionz.Database
{
    class Receipt : Record
    {
        int recordID;

        string paymentType { get; set; }        

        string expenseType { get; set; }

        DateTime transactionDate { get; set; }

        string description { get; set; }

        string supplierName { get; set; }

        string cityofPurchase { get; set; }

        decimal reqAmount { get; set; }

        decimal conversionRate { get; set; }

        decimal currencyAmountSGD { get; set; }
        
        string receiptNumber { get; set; }

        string receiptStatus { get; set; }

        Boolean isBillable { get; set; }

        Boolean isPersonalExpense { get; set; }

        string comment { get; set; }

        string imgPath { get; set; }
        
        public override void assignRecordID()
        {
            // Relook at this calculation of RecordID later
            int recordID = 1;

            while (File.Exists("receipt " + recordID))
            {
                recordID++;
            }

            this.recordID = recordID;
        }

        public override int getRecordID()
        {
            return recordID;
        }

        public override void delRecord()
        {
            string fileName = "receipt " + recordID + ".jpg";
            try
            {
                File.Delete(fileName);
                if (!File.Exists(fileName))
                {
                Console.WriteLine($"File {fileName} successfully deleted");
                }
            }

            

            catch (IOException e)
            {
                Console.WriteLine($"File {fileName} could not be deleted");
                Console.WriteLine(e.Message);
            }
        }
     }
}