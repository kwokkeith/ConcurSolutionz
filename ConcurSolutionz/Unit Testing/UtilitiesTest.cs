using System;
using ConcurSolutionz.Database;
using Newtonsoft.Json.Linq;

namespace Unit_Testing
{
	public class UtilitiesTest
	{
		[Fact]
        public void CheckNull_ShouldThrowErrorWhenNull()
        {
            // Arrange
            string? Value = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => Utilities.CheckNull(Value));
        }

        [Fact]
        public void CheckNull_ShouldReturnNothing()
        {
            // Arrange
            string? Value = "1231";

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckNull(Value));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(324)]
        [InlineData(232242)]
        public void CheckIsNumericType_TrueForInt(int Value)
        {
            Assert.True(Utilities.IsNumericType(Value));
        }

        [Theory]
        [InlineData("324.43")]
        [InlineData("423532.42")]
        public void CheckIsNumericType_TrueForDecimal(string value)
        {
            // Arrange
            decimal Value = Convert.ToDecimal(value);

            // Assert
            Assert.True(Utilities.IsNumericType(Value));
        }

        [Theory]
        [InlineData(324.43)]
        [InlineData(423532.42)]
        public void CheckIsNumericType_TrueForDouble(double Value)
        {
            // Assert
            Assert.True(Utilities.IsNumericType(Value));
        }

        [Fact]
        public void CheckIsNumericType_FalseForString()
        {
            // Arrange
            string Value = "HiEveryone!";

            // Assert
            Assert.False(Utilities.IsNumericType(Value));
        }

        [Fact]
        public void CheckIsNumericType_FalseForBoolean()
        {
            // Arrange
            bool Value = false;

            // Assert
            Assert.False(Utilities.IsNumericType(Value));
        }

        [Fact]
        public void CheckIsNumericType_FalseForNull()
        {
            // Arrange
            double? Value = null;
            int? Value2 = null;
            decimal? Value3 = null;

            // Assert
            Assert.False(Utilities.IsNumericType(Value));
            Assert.False(Utilities.IsNumericType(Value2));
            Assert.False(Utilities.IsNumericType(Value3));
        }


        [Fact]
        public void CheckIfNegative_ShouldThrowErrorWhenNegVal()
        {
            // Arrange
            decimal Value = -1231231.23m;

            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfNegative(Value));
        }

        [Fact]
        public void CheckIfNegative_ShouldReturnNothing()
        {
            // Arrange
            decimal Value = 1231231.23m;

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckNull(Value));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CheckIfEmptyString_ThrowErrorForEmptyString()
        {
            // Arrange
            string Value = "";

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfEmptyString(Value));
        }

        [Fact]
        public void CheckIfEmptyString_ShouldReturnNothing()
        {
            // Arrange
            string Value = "HIAndWelcome!";

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckIfEmptyString(Value));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void CheckDateTimeAheadOfNow_ThrowErrorForDateAhead()
        {
            // Arrange
            DateTime Value = DateTime.Now.AddDays(1);

            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckDateTimeAheadOfNow(Value));
        }

        [Fact]
        public void CheckDateTimeAheadOfNow_ShouldReturnNothing()
        {
            // Arrange
            DateTime Value = DateTime.Now;

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckDateTimeAheadOfNow(Value));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData("HiHongBeiTheConquerer")]
        [InlineData("HiHongJingTheOneAndOnly")]
        public void ConstEntryMetaDataPath_ShouldAppendString(string path)
        {
            // Arrange
            string Expected = Path.Combine(path, "EntryMetaData.json");

            //Assert
            Assert.Equal(Expected, Utilities.ConstEntryMetaDataPath(path));
        }

        [Fact]
        public void ConstEntryMetaDataPath_ThrowErrorForEmptyString()
        {
            // Arrange
            string path = "";

            //Assert
            Assert.Throws<ArgumentException>(() => Utilities.ConstEntryMetaDataPath(path));
        }

        [Theory]
        [InlineData("HiJonPeterson")]
        [InlineData("HiAkashJefferson")]
        public void ConstReceiptsFdrPath_ShouldAppendString(string path)
        {
            // Arrange
            string Expected = Path.Combine(path, "Receipts.fdr");

            //Assert
            Assert.Equal(Expected, Utilities.ConstReceiptsFdrPath(path));
        }

        [Fact]
        public void ConstReceiptsFdrPath_ThrowErrorForEmptyString()
        {
            // Arrange
            string path = "";

            //Assert
            Assert.Throws<ArgumentException>(() => Utilities.ConstReceiptsFdrPath(path));
        }

        [Theory]
        [InlineData("HiSidCamerson")]
        [InlineData("HiShaunTheJack")]
        public void ConstReceiptMetaDataPath_ShouldAppendString(string path)
        {
            // Arrange
            string Expected = Path.Combine(path, "Receipts.fdr", "ReceiptJSON.fdr");

            //Assert
            Assert.Equal(Expected, Utilities.ConstReceiptMetaDataPath(path));
        }

        [Fact]
        public void ConstReceiptMetaDataPath_ThrowErrorForEmptyString()
        {
            // Arrange
            string path = "";

            //Assert
            Assert.Throws<ArgumentException>(() => Utilities.ConstEntryMetaDataPath(path));
        }


    }
}

