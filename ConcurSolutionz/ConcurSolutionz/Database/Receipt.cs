using System.Reflection.Metadata;
using System.Collections.Immutable;
using System;
using System.IO;
using Record = ConcurSolutionz.Database.Record;

namespace ConcurSolutionz.Database
{
    public class Receipt : Record
    {
        public string paymentType { get; set; }        
        public string expenseType { get; set; }
        public DateTime transactionDate { get; set; }
        public string description { get; set; }
        public string supplierName { get; set; }
        public string cityOfPurchase { get; set; }
        public decimal reqAmount { get; set; }
        public decimal conversionRate { get; set; }
        public decimal currencyAmountSGD { get; set; }
        public string receiptNumber { get; set; }
        public string receiptStatus { get; set; }
        public Boolean isBillable { get; set; }
        public Boolean isPersonalExpense { get; set; }
        public string comment { get; set; }
        public string imgPath { get; set; }
    
        private Receipt(ReceiptBuilder builder)
        {
            // Check if attributes have been declared (Mandatory)

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

            Utilities.checkNull(this.paymentType);
            Utilities.checkNull(this.expenseType);
            Utilities.checkNull(this.transactionDate);
            Utilities.checkNull(this.description);
            Utilities.checkNull(this.supplierName);
            Utilities.checkNull(this.cityOfPurchase);
            Utilities.checkNull(this.reqAmount);
            Utilities.checkNull(this.conversionRate);
            Utilities.checkNull(this.currencyAmountSGD);
            Utilities.checkNull(this.receiptNumber);
            Utilities.checkNull(this.receiptStatus);
            Utilities.checkNull(this.isBillable);
            Utilities.checkNull(this.isPersonalExpense);
            Utilities.checkNull(this.comment);
            Utilities.checkNull(this.imgPath);

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
        public class ReceiptBuilder  
        {
            public string paymentType { get; private set; }
            public string expenseType { get; private set; }
            public DateTime transactionDate { get; private set; }
            public string description { get; private set; }
            public string supplierName { get; private set; }
            public string cityOfPurchase { get; private set; }
            public Decimal reqAmount { get; private set; }
            public Decimal conversionRate { get; private set; }
            public Decimal currencyAmountSGD { get; private set; }
            public string receiptNumber { get; private set; }
            public string receiptStatus { get; private set; }
            public bool isBillable { get; private set; }
            public bool isPersonalExpense { get; private set; }
            public string comment { get; private set; }
            public string imgPath { get; private set; }
            
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
                this.cityOfPurchase = cityOfPurchase;
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
            public ReceiptBuilder setIsBillable(Boolean isBillable)
            {
                this.isBillable = isBillable;
                return this;
            }
            public ReceiptBuilder setIsPersonalExpense(Boolean isPersonalExpense)
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