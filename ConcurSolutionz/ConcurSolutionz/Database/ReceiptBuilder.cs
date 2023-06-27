using System;
using 

class ReceiptBuilder  
{
    private readonly Receipt receipt;
    
    public ReceiptBuilder(
        string paymentType, 
        string expenseType,
        DateTime transactionDate, 
        string description,
        string supplierName,
        string cityOfPurchase,
        decimal reqAmount, 
        decimal conversionRate,
        decimal currencyAmountSGD,
        string receiptNumber,
        string receiptStatus, 
        bool isBillable,  
        bool isPersonalExpense,  
        string comment, 
        string imgPath)   
    {
        receipt = new Receipt();
        receipt.paymentType = paymentType;
        receipt.expenseType = expenseType;
        receipt.transactionDate = transactionDate;
        receipt.description = description;
        receipt.supplierName = supplierName;
        receipt.cityofPurchase = cityOfPurchase;
        receipt.reqAmount = reqAmount;
        receipt.conversionRate = conversionRate;
        receipt.currencyAmountSGD = currencyAmountSGD;
        receipt.receiptNumber = receiptNumber;
        receipt.receiptStatus = receiptStatus;
        receipt.isBillable = isBillable;
        receipt.isPersonalExpense = isPersonalExpense;
        receipt.comment = comment;
        receipt.imgPath = imgPath;
    }
    
     public ReceiptBuilder setPaymentType(string paymentType)  
     {
         receipt.paymentType = paymentType;
         return this;
     }
     
     public ReceiptBuilder setExpenseType(string expenseType) 
     {
        receipt.expenseType = expenseType;
        return this;    
     }
     
     public ReceiptBuilder setTransactionDate(DateTime transactionDate) 
     {
        receipt.transactionDate = transactionDate;  
        return this;   
     }
     
     public ReceiptBuilder setDescription (string description)   
     {
        receipt.description = description;
        return this;
     }  
     
     public ReceiptBuilder setSupplierName(string supplierName) 
     {
        receipt.supplierName = supplierName;
        return this;
     }
     
     public ReceiptBuilder setCityOfPurchase(string cityOfPurchase) 
     {
        receipt.cityofPurchase = cityOfPurchase;
        return this;
     }
     
     public ReceiptBuilder setReqAmount(decimal reqAmount)
     {
        receipt.reqAmount = reqAmount;
        return this;
     }
      
     public ReceiptBuilder setConversionRate(decimal conversionRate)
     {
        receipt.conversionRate = conversionRate;
        return this;
     }

     public ReceiptBuilder setCurrencyAmountSGD(decimal currencyAmountSGD)
     {
        receipt.currencyAmountSGD = currencyAmountSGD;
        return this;
     }

     public ReceiptBuilder setReceiptNumber(string receiptNumber)
     {
        receipt.receiptNumber = receiptNumber;
        return this;
     }

     public ReceiptBuilder setReceiptStatus(string receiptStatus)
     {
        receipt.receiptStatus = receiptStatus;
        return this;
     }
     public ReceiptBuilder setIsBillable(boolean isBillable)
     {
        receipt.isBillable = isBillable;
        return this;
     }
     public ReceiptBuilder setIsPersonalExpense(boolean isPersonalExpense)
     {
        receipt.isPersonalExpense = isPersonalExpense;
        return this;
     }
     public ReceiptBuilder setComment(string comment)
     {
        receipt.comment = comment;
        return this;
     }
     public ReceiptBuilder setImgPath(string imgPath)
     {
        receipt.imgPath = imgPath;
        return this;
     }



     public Receipt Build()   
     {
        return receipt;   
     }
}