using System;
using System.Globalization;
using ConcurSolutionz.Database;

namespace Unit_Test
{
    public class ReceiptTests
    {

        [Fact(DisplayName = "2.1")]
        public void Build_ReceiptShouldBuildUsingBuilder()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();

            // Assert
            string Expected1 = "Student Event-Others";
            Assert.Equal(Expected1, receipt.ExpenseType);

            DateTime Expected2 = DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Assert.Equal(Expected2, receipt.TransactionDate);

            string Expected3 = "Pizza Hut for bonding activities";
            Assert.Equal(Expected3, receipt.Description);

            string Expected4 = "Pizza Hut";
            Assert.Equal(Expected4, receipt.SupplierName);

            string Expected5 = "Singapore, SINGAPORE";
            Assert.Equal(Expected5, receipt.CityOfPurchase);

            string Expected6 = "Cash";
            Assert.Equal(Expected6, receipt.PaymentType);

            decimal Expected7 = 104.5m;
            Assert.Equal(Expected7, receipt.ReqAmount);

            string Expected8 = "Singapore, Dollar";
            Assert.Equal(Expected8, receipt.Currency);

            string Expected9 = "30355108-C3J1JCMTHEYJGO";
            Assert.Equal(Expected9, receipt.ReceiptNumber);

            string Expected10 = "Tax Receipt";
            Assert.Equal(Expected10, receipt.ReceiptStatus);

            bool Expected11 = false;
            Assert.Equal(Expected11, receipt.IsBillable);

            bool Expected12 = false;
            Assert.Equal(Expected12, receipt.IsPersonalExpense);

            string Expected13 = "";
            Assert.Equal(Expected13, receipt.Comment);

            string Expected14 = "IMG_5669.JPG";
            Assert.Equal(Expected14, receipt.ImgPath);

        }


        [Theory(DisplayName = "2.2")]
        [InlineData("US, Dollar", "104.5", "1.38085", "144.3")]
        [InlineData("US, Dollar", "299.77", "1.38085", "413.94")]
        [InlineData("Ukraine, Hryvnia", "23499.14", "0.0466524385", "1096.29")]
        [InlineData("China, Yuan Renminbi", "43343.39", "0.20892", "9055.30")]
        [InlineData("Malaysia, Ringgit", "39383.43", "0.31697", "12483.37")]
        [InlineData("Malaysia, Ringgit", "0", "0.31697", "0")]
        public void SetCurrencyAmountSGD_ConvertedCurrencyShouldCalculate(
            string Currency, string ReqAmount,
            string ConversionRate, string Expected)
        {
            // Arrange
            decimal reqAmount = Convert.ToDecimal(ReqAmount);
            decimal conversionRate = Convert.ToDecimal(ConversionRate);
            decimal expected = Convert.ToDecimal(Expected);

            Receipt.ReceiptBuilder receiptBuilder = new();

            // Act
            receiptBuilder = receiptBuilder.SetCurrency(Currency)
                .SetReqAmount(reqAmount)
                .SetConversionRate(conversionRate);
            Console.WriteLine(receiptBuilder.CurrencyAmountSGD);

            // Assert
            Assert.InRange(receiptBuilder.CurrencyAmountSGD, expected - 0.005M, expected + 0.005M);
        }


        [Theory(DisplayName = "2.3")]
        [InlineData("-56.2")]
        public void SetReqAmount_ShouldThrowErrorFromNegative(string ReqAmount)
        {
            // Arrange
            decimal reqAmount = Convert.ToDecimal(ReqAmount);
            Receipt.ReceiptBuilder receiptBuilder = new();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => receiptBuilder.SetReqAmount(reqAmount));
        }


        [Theory(DisplayName = "2.4")]
        [InlineData("-0.2344")]
        public void SetConversionRate_ShouldThrowErrorFromNegative(string ConversionRate)
        {
            // Arrange
            decimal conversionRate = Convert.ToDecimal(ConversionRate);
            Receipt.ReceiptBuilder receiptBuilder = new();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => receiptBuilder.SetConversionRate(conversionRate));
        }


        [Fact(DisplayName = "2.5")]
        public void Build_ShouldThrowErrorConversionRateNotSet()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();

            // Act
            receiptBuilder = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetCurrency("US, Dollar")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG");

            // Assert
            Assert.Throws<ArgumentException>(() => receiptBuilder.Build());
        }

        [Fact(DisplayName = "2.6")]
        public void Build_ShouldThrowErrorFromMissingField()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();

            // Act
            receiptBuilder = receiptBuilder
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG");

            // Assert
            Assert.Throws<ArgumentNullException>(() => receiptBuilder.Build());
        }

        [Fact(DisplayName = "2.x")]
        public void ReceiptConstructor_ShouldThrowErrorFromEmptyCompulsoryValue()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();

            // Act
            receiptBuilder = receiptBuilder
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG");

            // Assert
            Assert.Throws<ArgumentNullException>(() => receiptBuilder.Build());
        }

        [Theory(DisplayName = "2.7")]
        [InlineData("56.2")]
        [InlineData("120.34")]
        [InlineData("200.4")]
        [InlineData("100000002.3")]
        [InlineData("0")]
        public void ReceiptSetReqAmount_ShouldReturnNewSetValue(String ReqAmount)
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;
            decimal reqAmount = Convert.ToDecimal(ReqAmount);

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();

            receipt.ReqAmount = reqAmount;

            // Assert
            Assert.Equal(reqAmount, receipt.ReqAmount);
        }

        [Fact(DisplayName = "2.8")]
        public void ReceiptSetReqAmount_ShouldThrowErrorForNegValue()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();

            

            // Assert
            Assert.Throws<ArgumentException>(() => receipt.ReqAmount = -100m);
        }

        [Theory(DisplayName = "2.9")]
        [InlineData("56.2")]
        [InlineData("120.34")]
        [InlineData("200.4")]
        [InlineData("100000002.3")]
        public void ReceiptSetConversionRate_ShouldReturnNewSetValue(String ConversionRate)
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;
            decimal conversionRate = Convert.ToDecimal(ConversionRate);

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetConversionRate(conversionRate)
                .SetCurrency("Singapore, Dollar")
                .SetCurrency("1.38085")
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();


            receipt.ConversionRate = conversionRate;

            // Assert
            Assert.Equal(conversionRate, receipt.ConversionRate);
        }

        [Fact(DisplayName = "2.10")]
        public void ReceiptSetConversionRate_ShouldThrowErrorForNegValue()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetCurrency("US, Dollar")
                .SetReqAmount(104.5m)
                .SetConversionRate(1.38085m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();

            // Assert
            Assert.Throws<ArgumentException>(() => receipt.ConversionRate = -1.5m);
        }


        [Theory(DisplayName = "2.11")]
        [InlineData("US, Dollar", "1.38085", "144.3")]
        [InlineData("Ukraine, Hryvnia", "0.0466524385", "4.88")]
        [InlineData("China, Yuan Renminbi", "0.20892", "21.83")]
        [InlineData("Malaysia, Ringgit", "0.31697", "33.12")]
        [InlineData("UK, Pound Sterling", "1.72916", "180.70")]
        public void ReceiptSetConversionRate_ConvertedCurrencyShouldCalculate(
        string Currency,
        string ConversionRate, string Expected)
        {
            // Arrange
            decimal conversionRate = Convert.ToDecimal(ConversionRate);
            decimal expected = Convert.ToDecimal(Expected);

            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();

            // Assert
            receipt.Currency = Currency;
            receipt.ConversionRate = conversionRate;
            Assert.InRange(receipt.CurrencyAmountSGD, expected - 0.005M, expected + 0.005M);
        }

        [Theory(DisplayName = "2.12")]
        [InlineData("102.34")]
        [InlineData("6443.2")]
        [InlineData("20032.4")]
        [InlineData("10000240002.3")]
        public void ReceiptSetCurrencyAmountSGD_ShouldReturnNewSetValue(string CurrencyAmount)
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;
            decimal currencyAmount = Convert.ToDecimal(CurrencyAmount);

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetReqAmount(104.5m)
                .SetConversionRate(1.38085m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();
            receipt.CurrencyAmountSGD = currencyAmount;

            // Assert
            Assert.Equal(currencyAmount, receipt.CurrencyAmountSGD);
        }

        [Fact(DisplayName = "2.13")]
        public void ReceiptSetCurrencyAmountSGD_ShouldThrowErrorForNegValue()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;

            // Act
            receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                .SetTransactionDate(DateTime.ParseExact("24/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                .SetDescription("Pizza Hut for bonding activities")
                .SetSupplierName("Pizza Hut")
                .SetCityOfPurchase("Singapore, SINGAPORE")
                .SetCurrency("US, Dollar")
                .SetReqAmount(104.5m)
                .SetConversionRate(1.38085m)
                .SetReceiptNumber("30355108-C3J1JCMTHEYJGO")
                .SetReceiptStatus("Tax Receipt")
                .SetImgPath("IMG_5669.JPG")
                .Build();

            // Assert
            Assert.Throws<ArgumentException>(() => receipt.CurrencyAmountSGD = -1242.52m);
        }

        [Fact(DisplayName = "2.14")]
        public void Build_ReceiptShouldBuildUsingBuilder_Fuzz()
        {
            // Arrange
            Receipt.ReceiptBuilder receiptBuilder = new();
            Receipt receipt;
            DateTime transactionDate = Fuzzer.GenerateRandomDateTime();
            string description = Fuzzer.GenerateRandomString(23);
            string supplierName = Fuzzer.GenerateRandomString(20);
            decimal reqAmt = Convert.ToDecimal(Fuzzer.GenerateRandomDouble(3, 2));
            string receiptNumber = Fuzzer.GenerateRandomString(20);
            string receiptStatus = Fuzzer.GenerateRandomString(10);

            if (transactionDate > DateTime.Now)
            {
                Assert.Throws<ArgumentException>(() => receiptBuilder.SetTransactionDate(transactionDate));
            }

            else
            {
                // Act
                receipt = receiptBuilder.SetExpenseType("Student Event-Others")
                    .SetTransactionDate(transactionDate)
                    .SetDescription(description)
                    .SetSupplierName(supplierName)
                    .SetCityOfPurchase("Singapore, SINGAPORE")
                    .SetReqAmount(reqAmt)
                    .SetReceiptNumber(receiptNumber)
                    .SetReceiptStatus(receiptStatus)
                    .SetImgPath("IMG_5669.JPG")
                    .Build();

                // Assert
                string Expected1 = "Student Event-Others";
                Assert.Equal(Expected1, receipt.ExpenseType);

                DateTime Expected2 = transactionDate;
                Assert.Equal(Expected2, receipt.TransactionDate);

                string Expected3 = description;
                Assert.Equal(Expected3, receipt.Description);

                string Expected4 = supplierName;
                Assert.Equal(Expected4, receipt.SupplierName);

                string Expected5 = "Singapore, SINGAPORE";
                Assert.Equal(Expected5, receipt.CityOfPurchase);

                string Expected6 = "Cash";
                Assert.Equal(Expected6, receipt.PaymentType);

                decimal Expected7 = reqAmt;
                Assert.Equal(Expected7, receipt.ReqAmount);

                string Expected8 = "Singapore, Dollar";
                Assert.Equal(Expected8, receipt.Currency);

                string Expected9 = receiptNumber;
                Assert.Equal(Expected9, receipt.ReceiptNumber);

                string Expected10 = receiptStatus;
                Assert.Equal(Expected10, receipt.ReceiptStatus);

                bool Expected11 = false;
                Assert.Equal(Expected11, receipt.IsBillable);

                bool Expected12 = false;
                Assert.Equal(Expected12, receipt.IsPersonalExpense);

                string Expected13 = "";
                Assert.Equal(Expected13, receipt.Comment);

                string Expected14 = "IMG_5669.JPG";
                Assert.Equal(Expected14, receipt.ImgPath);
            }
        }
    }
}