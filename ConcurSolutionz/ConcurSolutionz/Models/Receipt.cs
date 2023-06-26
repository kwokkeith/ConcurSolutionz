//using System;
//using Android.Content;
//using AndroidX.Annotations;

//namespace ConcurSolutionz.Models
//{
//	public class Receipt: Record
//	{
//        //Attributes
//        private string paymentType;
//        private string expenseType;
//        private DateTime transactionDate;
//        private string description;
//        private string supplierName;
//        private string cityOfPurchase;
//        private decimal reqAmount;
//        private decimal conversionRate;
//        private decimal currencyAmountSGD;
//        private string receiptNumber;
//        private string receiptStatus;
//        private bool isBillable;
//        private bool isPersonalExpense;
//        private string comment;
//        private string imgPath;
//        private int recordID;

//        // Methods from Record interface
//        public void AssignRecordId()
//        {
//            // implement method
//        }

//        public int GetRecordId()
//        {
//            return recordID;
//        }

//        public void DelRecord()
//        {
//            // implement method
//        }

//        public int RecordId { get => recordID; }

//        // Receipt specific methods
//        public Receipt(ReceiptBuilder builder)
//        {
//            paymentType = builder.PaymentType;
//            expenseType = builder.ExpenseType;
//            transactionDate = builder.TransactionDate;
//            description = builder.Description;
//            supplierName = builder.SupplierName;
//            cityOfPurchase = builder.CityOfPurchase;
//            reqAmount = builder.ReqAmount;
//            conversionRate = builder.ConversionRate;
//            currencyAmountSGD = builder.CurrencyAmountSGD;
//            receiptNumber = builder.ReceiptNumber;
//            receiptStatus = builder.ReceiptStatus;
//            isBillable = builder.IsBillable;
//            isPersonalExpense = builder.IsPersonalExpense;
//            comment = builder.Comment;
//            imgPath = builder.ImgPath;
//        }
//        // To create a new receipt instance:
//        //ReceiptBuilder builder = new ReceiptBuilder();
//        //builder.SetPaymentType("Cash").SetExpenseType("Food").SetTransactionDate(DateTime.Now).SetDescription("Dinner");

//        //Receipt receipt = builder.Build();  // This receipt now has all the properties set in the builder
//    }
//    public class ReceiptBuilder
//    {
//        public string PaymentType { get; set; }
//        public string ExpenseType { get; set; }
//        public DateTime TransactionDate { get; set; }
//        public string Description { get; set; }
//        public string SupplierName { get; set; }
//        public string CityOfPurchase { get; set; }
//        public decimal ReqAmount { get; set; }
//        public decimal ConversionRate { get; set; }
//        public decimal CurrencyAmountSGD { get; set; }
//        public string ReceiptNumber { get; set; }
//        public string ReceiptStatus { get; set; }
//        public bool IsBillable { get; set; }
//        public bool IsPersonalExpense { get; set; }
//        public string Comment { get; set; }
//        public string ImgPath { get; set; }

//        public ReceiptBuilder SetPaymentType(string paymentType)
//        {
//            PaymentType = paymentType;
//            return this;
//        }

//        public ReceiptBuilder SetExpenseType(string expenseType)
//        {
//            ExpenseType = expenseType;
//            return this; 
//        }

//        public ReceiptBuilder SetTransactionDate(DateTime dateTime)
//        {
//            TransactionDate = dateTime;
//            return this; 
//        }

//        public ReceiptBuilder SetDescription(string description)
//        {
//            Description = description;
//            return this;
//        }

//        public ReceiptBuilder SetSupplierName(string supplierName)
//        {
//            SupplierName = supplierName;
//            return this; 
//        }

//        public ReceiptBuilder SetCityOfPurchase(string cityOfPurchase)
//        {
//            CityOfPurchase = cityOfPurchase;
//            return this; 
//        }

//        public ReceiptBuilder SetReqAmount(decimal reqAmount)
//        {
//            ReqAmount = reqAmount;
//            return this; 
//        }

//        public ReceiptBuilder setConversionRate(decimal conversionRate)
//        {
//            ConversionRate = conversionRate;
//            return this; 
//        }

//        public ReceiptBuilder setCurrencyAmountSGD(decimal currencyAmountSGD)
//        {
//            CurrencyAmountSGD = currencyAmountSGD;
//            return this; 
//        }

//        public ReceiptBuilder SetReceiptNumber(string receiptNumber)
//        {
//            ReceiptNumber = receiptNumber;
//            return this; 
//        }


//        public ReceiptBuilder SetReceiptStatus(string receiptStatus)
//        {
//            ReceiptStatus = receiptStatus;
//            return this;
//        }

//        public ReceiptBuilder SetIsBillable(bool isBillable)
//        {
//            IsBillable = isBillable;
//            return this;
//        }

//        public ReceiptBuilder SetIsPersonalExpense(bool isPersonalExpense)
//        {
//            IsPersonalExpense = isPersonalExpense;
//            return this;
//        }

//        public ReceiptBuilder SetComment(string comment)
//        {
//            Comment = comment;
//            return this;
//        }

//        public ReceiptBuilder SetImgPath(string imgPath)
//        {
//            ImgPath = imgPath;
//            return this;
//        }

//        public Receipt Build()
//        {
//            return new Receipt(this);
//        }
//    }

//    }

