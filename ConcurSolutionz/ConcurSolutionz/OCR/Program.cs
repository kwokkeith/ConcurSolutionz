#pragma warning disable 8600, 8602

public class Program {
    private static Char[] delimiters = new Char[] {' ','\t','\n'};
    private static float accuracyMinPercent = 10.0F;
    private static string[] keyWords = new string[] 
    {"Total", "Amount", "Subtotal", "Grand Total", "Amount Due", "Tax", "Receipt", "Inv", "Invoice", "No", "Bill", "Slip"};

    public static void Main(String[] args) {
        ReceiptOCR r = new ReceiptOCR("../bin/1.jpg","../bin/tesseract");
        Console.WriteLine(r.receiptNumber);
        Console.WriteLine(r.reqAmount);
    }
}
