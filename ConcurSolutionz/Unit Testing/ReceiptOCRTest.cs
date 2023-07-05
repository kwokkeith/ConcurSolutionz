using System;
using ConcurSolutionz.Database;
namespace Unit_Testing
{
	public class ReceiptOCRTest
	{
		[Theory]
		[InlineData("QWERTY123", "2343.23")]
		public void ReceiptOCRTest_ShouldReturnValidReceiptOCR(
			string? ReceiptNumber, string? ReqAmount)
		{
			// Arrange
			decimal? reqAmount;
			if(ReqAmount != null)
			{
				reqAmount = Convert.ToDecimal(ReqAmount);
            }
			else
			{
				reqAmount = null;
			}

			// Act
			ReceiptOCR receiptOCR = new();
			receiptOCR.ReqAmount = reqAmount;
			receiptOCR.ReceiptNumber = ReceiptNumber;

			// Assert
			Assert.Equal(ReceiptNumber, receiptOCR.ReceiptNumber);
			Assert.Equal(reqAmount, receiptOCR.ReqAmount);
		}
	}
}
