
namespace ConcurSolutionz.Database
{
    public class Receipt : Record
    {
        public string PaymentType { get; set; }        
        public string ExpenseType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public string SupplierName { get; set; }
        public string CityOfPurchase { get; set; }
        public decimal ReqAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal CurrencyAmountSGD { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptStatus { get; set; }
        public bool IsBillable { get; set; }
        public bool IsPersonalExpense { get; set; }
        public string Comment { get; set; }
        public string ImgPath { get; set; }
    
        private Receipt(ReceiptBuilder builder)
        {
            // Check if attributes have been declared (Mandatory)
            Utilities.CheckNull(builder.PaymentType);
            Utilities.CheckNull(builder.ExpenseType);
            Utilities.CheckNull(builder.TransactionDate);
            Utilities.CheckNull(builder.Description);
            Utilities.CheckNull(builder.SupplierName);
            Utilities.CheckNull(builder.CityOfPurchase);
            Utilities.CheckNull(builder.ReqAmount);
            Utilities.CheckNull(builder.ConversionRate);
            Utilities.CheckNull(builder.CurrencyAmountSGD);
            Utilities.CheckNull(builder.ReceiptNumber);
            Utilities.CheckNull(builder.ReceiptStatus);
            Utilities.CheckNull(builder.IsBillable);
            Utilities.CheckNull(builder.IsPersonalExpense);
            Utilities.CheckNull(builder.Comment);
            Utilities.CheckNull(builder.ImgPath);
            
            // Set the attributes
            PaymentType = builder.PaymentType;
            ExpenseType = builder.ExpenseType;
            TransactionDate = builder.TransactionDate;
            Description = builder.Description;
            SupplierName = builder.SupplierName;
            CityOfPurchase = builder.CityOfPurchase;
            ReqAmount = builder.ReqAmount;
            ConversionRate = builder.ConversionRate;
            CurrencyAmountSGD = builder.CurrencyAmountSGD;
            ReceiptNumber = builder.ReceiptNumber;
            ReceiptStatus = builder.ReceiptStatus;
            IsBillable = builder.IsBillable;
            IsPersonalExpense = builder.IsPersonalExpense;
            Comment = builder.Comment;
            ImgPath = builder.ImgPath;
        }

        public override void AssignRecordID()
        {
            // Relook at this calculation of RecordID later
            int recordID = 1;

            while (File.Exists("receipt " + recordID))
            {
                recordID++;
            }

            this.RecordID = recordID;
        }

        public override int GetRecordID()
        {
            return RecordID;
        }

        public override void DelRecord()
        {
            string fileName = "receipt " + RecordID + ".jpg";
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
            public string PaymentType { get; private set; }
            public string ExpenseType { get; private set; }
            public DateTime TransactionDate { get; private set; }
            public string Description { get; private set; }
            public string SupplierName { get; private set; }
            public string CityOfPurchase { get; private set; }
            public decimal ReqAmount { get; private set; }
            public decimal ConversionRate { get; private set; }
            public decimal CurrencyAmountSGD { get; private set; }
            public string ReceiptNumber { get; private set; }
            public string ReceiptStatus { get; private set; }
            public bool IsBillable { get; private set; }
            public bool IsPersonalExpense { get; private set; }
            public string Comment { get; private set; }
            public string ImgPath { get; private set; }
            
            public ReceiptBuilder(){
                // Input any default values
            }
            
            public ReceiptBuilder SetPaymentType(string PaymentType)  
            {
                this.PaymentType = PaymentType;
                return this;
            }
            
            public ReceiptBuilder SetExpenseType(string ExpenseType) 
            {
                this.ExpenseType = ExpenseType;
                return this;    
            }
            
            public ReceiptBuilder SetTransactionDate(DateTime TransactionDate) 
            {
                this.TransactionDate = TransactionDate;  
                return this;   
            }
            
            public ReceiptBuilder SetDescription (string Description)   
            {
                this.Description = Description;
                return this;
            }  
            
            public ReceiptBuilder SetSupplierName(string SupplierName) 
            {
                this.SupplierName = SupplierName;
                return this;
            }
            
            public ReceiptBuilder SetCityOfPurchase(string CityOfPurchase) 
            {
                this.CityOfPurchase = CityOfPurchase;
                return this;
            }
            
            public ReceiptBuilder SetReqAmount(decimal ReqAmount)
            {
                this.ReqAmount = ReqAmount;
                return this;
            }
            
            public ReceiptBuilder SetConversionRate(decimal ConversionRate)
            {
                this.ConversionRate = ConversionRate;
                return this;
            }

            public ReceiptBuilder SetCurrencyAmountSGD(decimal CurrencyAmountSGD)
            {
                this.CurrencyAmountSGD = CurrencyAmountSGD;
                return this;
            }

            public ReceiptBuilder SetReceiptNumber(string ReceiptNumber)
            {
                this.ReceiptNumber = ReceiptNumber;
                return this;
            }

            public ReceiptBuilder SetReceiptStatus(string ReceiptStatus)
            {
                this.ReceiptStatus = ReceiptStatus;
                return this;
            }
            public ReceiptBuilder SetIsBillable(bool IsBillable)
            {
                this.IsBillable = IsBillable;
                return this;
            }
            public ReceiptBuilder SetIsPersonalExpense(bool IsPersonalExpense)
            {
                this.IsPersonalExpense = IsPersonalExpense;
                return this;
            }
            public ReceiptBuilder SetComment(string Comment)
            {
                this.Comment = Comment;
                return this;
            }
            public ReceiptBuilder SetImgPath(string ImgPath)
            {
                this.ImgPath = ImgPath;
                return this;
            }

            public Receipt Build()   
            {
                return new Receipt(this);   
            }
        }
    }
}