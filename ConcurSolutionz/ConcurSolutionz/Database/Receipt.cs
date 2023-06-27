using System.Reflection.Metadata;
using System.Collections.Immutable;
using System;
using System.IO;
using Record = ConcurSolutionz.Database.Record;

namespace ConcurSolutionz.Database
{
    class Receipt : Record
    {
        private string paymentType { get; set; }        
        private string expenseType { get; set; }
        private DateTime transactionDate { get; set; }
        private string description { get; set; }
        private string supplierName { get; set; }
        private string cityOfPurchase { get; set; }
        private decimal reqAmount { get; set; }
        private decimal conversionRate { get; set; }
        private decimal currencyAmountSGD { get; set; }
        private string receiptNumber { get; set; }
        private string receiptStatus { get; set; }
        private Boolean isBillable { get; set; }
        private Boolean isPersonalExpense { get; set; }
        private string comment { get; set; }
        private string imgPath { get; set; }
    
        private Receipt(ReceiptBuilder builder)
        {
            // Check if attributes have been declared (Mandatory)
            Utilities.checkNull(builder.paymentType);
            Utilities.checkNull(builder.expenseType);
            Utilities.checkNull(builder.transactionDate);
            Utilities.checkNull(builder.description);
            Utilities.checkNull(builder.supplierName);
            Utilities.checkNull(builder.cityOfPurchase);
            Utilities.checkNull(builder.reqAmount);
            Utilities.checkNull(builder.conversionRate);
            Utilities.checkNull(builder.currencyAmountSGD);
            Utilities.checkNull(builder.receiptNumber);
            Utilities.checkNull(builder.receiptStatus);
            Utilities.checkNull(builder.isBillable);
            Utilities.checkNull(builder.isPersonalExpense);
            Utilities.checkNull(builder.comment);
            Utilities.checkNull(builder.imgPath);

            // Set the attributes
            paymentType = builder.paymentType;
            expenseType = builder.expenseType;
            transactionDate = builder.transactionDate;
            description = builder.description;
            supplierName = builder.supplierName;
            cityOfPurchase = builder.cityOfPurchase;
            reqAmount = builder.reqAmount;
            conversionRate = builder.conversionRate;
            currencyAmountSGD = builder.currencyAmountSGD;
            receiptNumber = builder.receiptNumber;
            receiptStatus = builder.receiptStatus;
            isBillable = builder.isBillable;
            isPersonalExpense = builder.isPersonalExpense;
            comment = builder.comment;
            imgPath = builder.imgPath;
        }

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

        // Builder for the Receipt class
        static class ReceiptBuilder  
        {
            private string paymentType;
            private string expenseType;
            private DateTime transactionDate;
            private string description;
            private string supplierName;
            private string cityOfPurchase;
            private Decimal reqAmount;
            private Decimal conversionRate;
            private Decimal currencyAmountSGD;
            private string receiptNumber;
            private string receiptStatus;
            private bool isBillable;
            private bool isPersonalExpense;
            private string comment;
            private string imgPath;
            
            ReceiptBuilder(){
                // Input any default values
            }
            
            public ReceiptBuilder setPaymentType(string paymentType)  
            {
                this.paymentType = paymentType;
                return this;
            }
            
            public ReceiptBuilder setExpenseType(string expenseType) 
            {
                this.expenseType = expenseType;
                return this;    
            }
            
            public ReceiptBuilder setTransactionDate(DateTime transactionDate) 
            {
                this.transactionDate = transactionDate;  
                return this;   
            }
            
            public ReceiptBuilder setDescription (string description)   
            {
                this.description = description;
                return this;
            }  
            
            public ReceiptBuilder setSupplierName(string supplierName) 
            {
                this.supplierName = supplierName;
                return this;
            }
            
            public ReceiptBuilder setCityOfPurchase(string cityOfPurchase) 
            {
                this.cityofPurchase = cityOfPurchase;
                return this;
            }
            
            public ReceiptBuilder setReqAmount(decimal reqAmount)
            {
                this.reqAmount = reqAmount;
                return this;
            }
            
            public ReceiptBuilder setConversionRate(decimal conversionRate)
            {
                this.conversionRate = conversionRate;
                return this;
            }

            public ReceiptBuilder setCurrencyAmountSGD(decimal currencyAmountSGD)
            {
                this.currencyAmountSGD = currencyAmountSGD;
                return this;
            }

            public ReceiptBuilder setReceiptNumber(string receiptNumber)
            {
                this.receiptNumber = receiptNumber;
                return this;
            }

            public ReceiptBuilder setReceiptStatus(string receiptStatus)
            {
                this.receiptStatus = receiptStatus;
                return this;
            }
            public ReceiptBuilder setIsBillable(boolean isBillable)
            {
                this.isBillable = isBillable;
                return this;
            }
            public ReceiptBuilder setIsPersonalExpense(boolean isPersonalExpense)
            {
                this.isPersonalExpense = isPersonalExpense;
                return this;
            }
            public ReceiptBuilder setComment(string comment)
            {
                this.comment = comment;
                return this;
            }
            public ReceiptBuilder setImgPath(string imgPath)
            {
                this.imgPath = imgPath;
                return this;
            }

            public Receipt Build()   
            {
                return new Receipt(this);   
            }
        }
    }
}