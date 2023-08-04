using System;
using ConcurSolutionz.Database;
using Newtonsoft.Json.Linq;

namespace Unit_Test
{
	public class UtilitiesTest
	{
		[Fact(DisplayName = "1.1")]
        public void CheckNull_ShouldThrowErrorWhenNull()
        {
            // Arrange
            string? Value = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => Utilities.CheckNull(Value));
        }

        [Fact(DisplayName = "1.2")]
        public void CheckNull_ShouldReturnNothing()
        {
            // Arrange
            string? Value = "Hi!";

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckNull(Value));

            // Assert
            Assert.Null(exception);
        }

        [Theory(DisplayName = "1.3")]
        [InlineData(324)]
        [InlineData(232242)]
        public void CheckIsNumericType_TrueForInt(int Value)
        {
            Assert.True(Utilities.IsNumericType(Value));
        }

        [Theory(DisplayName = "1.3")]
        [InlineData("324.43")]
        [InlineData("423532.42")]
        public void CheckIsNumericType_TrueForDecimal(string value)
        {
            // Arrange
            decimal Value = Convert.ToDecimal(value);

            // Assert
            Assert.True(Utilities.IsNumericType(Value));
        }

        [Theory(DisplayName = "1.3")]
        [InlineData(324.43)]
        [InlineData(423532.42)]
        public void CheckIsNumericType_TrueForDouble(double Value)
        {
            // Assert
            Assert.True(Utilities.IsNumericType(Value));
        }

        [Fact(DisplayName = "1.4")]
        public void CheckIsNumericType_FalseForString()
        {
            // Arrange
            string Value = "HiEveryone!";

            // Assert
            Assert.False(Utilities.IsNumericType(Value));
        }

        [Fact(DisplayName = "1.4")]
        public void CheckIsNumericType_FalseForBoolean()
        {
            // Arrange
            bool Value = false;

            // Assert
            Assert.False(Utilities.IsNumericType(Value));
        }

        [Fact(DisplayName = "1.5")]
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


        [Fact(DisplayName = "1.6")]
        public void CheckIfNegative_ShouldThrowErrorWhenNegVal()
        {
            // Arrange
            decimal Value = -1231231.23m;

            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfNegative(Value));
        }

        [Fact(DisplayName = "1.7")]
        public void CheckIfNegative_ShouldReturnNothing()
        {
            // Arrange
            decimal Value = 1231231.23m;

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckNull(Value));

            // Assert
            Assert.Null(exception);
        }

        [Fact(DisplayName = "1.8")]
        public void CheckIfEmptyString_ThrowErrorForEmptyString()
        {
            // Arrange
            string Value = "";

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfEmptyString(Value));
        }

        [Fact(DisplayName = "1.9")]
        public void CheckIfEmptyString_ThrowErrorForNullString()
        {
            // Arrange
            string Value = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckIfEmptyString(Value));
        }



        [Fact(DisplayName = "1.10")]
        public void CheckIfEmptyString_ShouldReturnNothing()
        {
            // Arrange
            string Value = "HIAndWelcome!";

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckIfEmptyString(Value));

            // Assert
            Assert.Null(exception);
        }

        [Fact(DisplayName = "1.11")]
        public void CheckDateTimeAheadOfNow_ThrowErrorForDateAhead()
        {
            // Arrange
            DateTime Value = DateTime.Now.AddDays(365);

            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckDateTimeAheadOfNow(Value));
        }

        [Fact(DisplayName = "1.12")]
        public void CheckDateTimeAheadOfNow_ShouldReturnNothingForDateBefore()
        {
            // Arrange
            DateTime Value = DateTime.Now.AddDays(-365);

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckDateTimeAheadOfNow(Value));

            // Assert
            Assert.Null(exception);
        }

        [Fact(DisplayName = "1.13")]
        public void CheckDateTimeAheadOfNow_ShouldReturnNothingForCurrentDate()
        {
            // Arrange
            DateTime Value = DateTime.Now;

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckDateTimeAheadOfNow(Value));

            // Assert
            Assert.Null(exception);
        }

        [Fact(DisplayName = "1.14")]
        public void CheckLastModifiedAheadOfCreation_ShouldThrowExceptionForLastModifiedAheadOfCreation()
        {
            // Arrange
            DateTime lastModified = new(2023, 01, 23, 0, 0, 0);
            DateTime creation = new(2022, 12, 23, 0, 0, 0);

            // Assert
            Assert.Throws<ArgumentException>(() => Utilities.CheckLastModifiedAheadOfCreation(creation, lastModified));
        }

        [Fact(DisplayName = "1.15")]
        public void CheckLastModifiedAheadOfCreation_ShouldReturnNothingForLastModifiedBeforeCreation()
        {
            // Arrange
            DateTime lastModified = new(2022, 12, 23, 0, 0, 0);
            DateTime creation = new(2023, 01, 23, 0, 0, 0);

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckLastModifiedAheadOfCreation(creation, lastModified));

            // Assert
            Assert.Null(exception);
        }

        [Fact(DisplayName = "1.16")]
        public void CheckLastModifiedAheadOfCreation_ShouldReturnNothingForLastModifiedEqualCreation()
        {
            // Arrange
            DateTime lastModified = new(2023, 01, 23, 0, 0, 0);
            DateTime creation = new(2023, 01, 23, 0, 0, 0);

            // Act
            var exception = Xunit.Record.Exception(() => Utilities.CheckLastModifiedAheadOfCreation(lastModified, creation));

            // Assert
            Assert.Null(exception);
        }



        [Theory(DisplayName = "1.17")]
        [InlineData("Capstone 2023")]
        [InlineData("HiHongJingTheOneAndOnly")]
        public void ConstEntryMetaDataPath_ShouldAppendString(string path)
        {
            // Arrange
            string entryPath = Path.Combine(path, "EntryMetaData.json");

            //Assert
            Assert.Equal(entryPath, Utilities.ConstEntryMetaDataPath(path));
        }

        [Fact(DisplayName = "1.18")]
        public void ConstEntryMetaDataPath_ThrowErrorForEmptyString()
        {
            // Arrange
            string entryPath = "";

            //Assert
            Assert.Throws<ArgumentException>(() => Utilities.ConstEntryMetaDataPath(entryPath));
        }

        [Theory(DisplayName = "1.19")]
        [InlineData("Capstone 2023")]
        [InlineData("HiAkashJefferson")]
        public void ConstReceiptsFdrPath_ShouldAppendString(string path)
        {
            // Arrange
            string entryPath = Path.Combine(path, "Records.fdr");

            //Assert
            Assert.Equal(entryPath, Utilities.ConstRecordsFdrPath(path));
        }

        [Fact(DisplayName = "1.20")]
        public void ConstReceiptsFdrPath_ThrowErrorForEmptyString()
        {
            // Arrange
            string entryPath = "";

            //Assert
            Assert.Throws<ArgumentException>(() => Utilities.ConstRecordsFdrPath(entryPath));
        }

        [Theory(DisplayName = "1.21")]
        [InlineData("Capstone 2023")]
        [InlineData("HiShaunTheJack")]
        public void ConstReceiptMetaDataPath_ShouldAppendString(string path)
        {
            // Arrange
            string entryPath = Path.Combine(path, "Records.fdr", "RecordJSON.fdr");

            //Assert
            Assert.Equal(entryPath, Utilities.ConstRecordsMetaDataPath(path));
        }

        [Fact(DisplayName = "1.22")]
        public void ConstReceiptMetaDataPath_ThrowErrorForEmptyString()
        {
            // Arrange
            string entryPath = "";

            //Assert
            Assert.Throws<ArgumentException>(() => Utilities.ConstRecordsMetaDataPath(entryPath));
        }


    }
}

