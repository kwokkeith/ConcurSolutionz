#nullable enable

using System.Text.Json;
using System.Text.Json.Serialization;

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
        public decimal reqAmount;
        public decimal ReqAmount
        {
            get
            {
                return reqAmount;
            }
            set
            {
                Utilities.CheckNull(value);
                Utilities.CheckIfNegative(value);
                reqAmount = value;
            }
        }
        public decimal conversionRate;
        public decimal ConversionRate
        {
            get
            {
                return conversionRate;
            }
            set
            {
                Utilities.CheckNull(value);
                Utilities.CheckIfNegative(value);
                conversionRate = value;

                // Calculate new Currency Amount
                CurrencyAmountSGD = ReqAmount * ConversionRate;
            }
        }
        public decimal currencyAmountSGD;
        public decimal CurrencyAmountSGD
        {
            get
            {
                return currencyAmountSGD;
            }
            set
            {
                Utilities.CheckNull(value);
                Utilities.CheckIfNegative(value);
                currencyAmountSGD = value;
            }
        }
        public string Currency { get; set; }
        public string ReceiptNumber { get; set; }
        public string ReceiptStatus { get; set; }
        public bool IsBillable { get; set; }
        public bool IsPersonalExpense { get; set; }
        public string Comment { get; set; }
        public string ImgPath { get; set; }


        public Receipt(ReceiptBuilder builder)
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
            Utilities.CheckNull(builder.Currency);

            // Set the attributes
            PaymentType = builder.PaymentType;
            ExpenseType = builder.ExpenseType;
            TransactionDate = builder.TransactionDate;
            Description = builder.Description;
            SupplierName = builder.SupplierName;
            CityOfPurchase = builder.CityOfPurchase;
            ReqAmount = builder.ReqAmount;
            ConversionRate = builder.ConversionRate;
            Currency = builder.Currency;
            CurrencyAmountSGD = builder.CurrencyAmountSGD;
            ReceiptNumber = builder.ReceiptNumber;
            ReceiptStatus = builder.ReceiptStatus;
            IsBillable = builder.IsBillable;
            IsPersonalExpense = builder.IsPersonalExpense;
            Comment = builder.Comment;
            ImgPath = builder.ImgPath;
            SubType = GetType().FullName;
        }


        public class ReceiptConverter : JsonConverter<Receipt>
        {
            public override void Write(Utf8JsonWriter writer, Receipt value, JsonSerializerOptions options)
            {
                // Write JSON
            }


            public override Receipt Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Manually override parsing of JSON and return a Receipt object
                JsonDocument doc = JsonDocument.ParseValue(ref reader);
                string paymentType = doc.RootElement.GetProperty("PaymentType").GetString();
                string expenseType = doc.RootElement.GetProperty("ExpenseType").GetString();
                string transactionDate = doc.RootElement.GetProperty("TransactionDate").GetString();
                string description = doc.RootElement.GetProperty("Description").GetString();
                string supplierName = doc.RootElement.GetProperty("SupplierName").GetString();
                string cityOfPurchase = doc.RootElement.GetProperty("CityOfPurchase").GetString();
                decimal reqAmount = doc.RootElement.GetProperty("ReqAmount").GetDecimal();
                decimal conversionRate = doc.RootElement.GetProperty("ConversionRate").GetDecimal();
                string currency = doc.RootElement.GetProperty("Currency").GetString();
                decimal currencyAmountSGD = doc.RootElement.GetProperty("CurrencyAmountSGD").GetDecimal();
                string receiptNumber = doc.RootElement.GetProperty("ReceiptNumber").GetString();
                string receiptStatus = doc.RootElement.GetProperty("ReceiptStatus").GetString();
                bool isBillable = doc.RootElement.GetProperty("IsBillable").GetBoolean();
                bool isPersonalExpense = doc.RootElement.GetProperty("IsPersonalExpense").GetBoolean();
                string comment = doc.RootElement.GetProperty("Comment").GetString();
                string imgPath = doc.RootElement.GetProperty("ImgPath").GetString();
                string subType = doc.RootElement.GetProperty("SubType").GetString();
                int recordID = doc.RootElement.GetProperty("RecordID").GetInt32();

                ReceiptBuilder builder = new ReceiptBuilder();

                builder.SetPaymentType(paymentType);
                builder.SetExpenseType(expenseType);
                builder.SetTransactionDate(DateTime.Parse(transactionDate));
                builder.SetDescription(description);
                builder.SetSupplierName(supplierName);
                builder.SetCityOfPurchase(cityOfPurchase);
                builder.SetReqAmount(reqAmount);
                builder.SetConversionRate(conversionRate);
                builder.SetCurrency(currency);
                builder.SetReceiptNumber(receiptNumber);
                builder.SetReceiptStatus(receiptStatus);
                builder.SetIsBillable(isBillable);
                builder.SetIsPersonalExpense(isPersonalExpense);
                builder.SetComment(comment);
                builder.SetImgPath(imgPath);

                Receipt receipt = builder.Build();
                receipt.RecordID = recordID;
                return receipt;
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
            public string Currency { get; private set; }

            public const string SINGAPORE_CURRENCY = "Singapore, Dollar";
            public const string SINGAPORE_CITY = "Singapore, SINGAPORE";
            public const string DEFAULT_PAYMENT = "Cash";
            public const string DEFAULT_RECEIPT_STATUS = "Tax Receipt";


            public ReceiptBuilder()
            {
                // Input any default values (Non-compulsory fields)
                SupplierName = "";
                Comment = "";
                IsBillable = false;
                IsPersonalExpense = false;
                PaymentType = DEFAULT_PAYMENT;
                CityOfPurchase = SINGAPORE_CITY;
                Currency = SINGAPORE_CURRENCY;
                ReceiptStatus = DEFAULT_RECEIPT_STATUS;

                // Default Value (Only matters when City of Purchase
                // is not Singapore)
                ReqAmount = 0m;
                CurrencyAmountSGD = 0m;
                ConversionRate = 0m;
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


            public ReceiptBuilder SetDescription(string Description)
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
                Utilities.CheckIfNegative(ReqAmount);
                this.ReqAmount = ReqAmount;
                return SetCurrencyAmountSGD();
            }


            public ReceiptBuilder SetConversionRate(decimal ConversionRate)
            {
                Utilities.CheckIfNegative(ConversionRate);
                this.ConversionRate = ConversionRate;
                return SetCurrencyAmountSGD();
            }


            private ReceiptBuilder SetCurrencyAmountSGD()
            {
                if (ReqAmount != 0m || ConversionRate != 0)
                {
                    CurrencyAmountSGD = ReqAmount * ConversionRate;
                }
                return this;
            }


            public ReceiptBuilder SetCurrency(string Currency)
            {
                this.Currency = Currency;
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
                if (Currency != SINGAPORE_CURRENCY)
                {
                    if (CurrencyAmountSGD == 0m || ConversionRate == 0)
                    {
                        throw new ArgumentException("If currency set is " +
                            "not Singapore SGD, please set ConversionRate.");
                    }
                }
                return new Receipt(this);
            }
        }
    }
}