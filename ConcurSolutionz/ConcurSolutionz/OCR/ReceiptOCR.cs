using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;

public class ReceiptOCR {
    public string receiptNumber;
    public Decimal reqAmount;
    public string imgPath;
    private string tesseractPath;

    private static Char[] delimiters = new Char[] {' ','\t','\n'};
    private static float accuracyMinPercent = 10.0F;
    private static string[] reqAmountKeywords = new string[] 
    {"Total", "Amount", "Grand Total", "Amount Due"};
    private static string[] receiptNumberKeywords = new string[] 
    {"Tax", "Receipt", "Inv", "Invoice", "No", "Slip"};

    private Regex moneyFormat = new Regex("[0-9]+\\.\\d\\d", RegexOptions.IgnoreCase);

    public ReceiptOCR(string imgPath, string tesseractPath) {
        this.imgPath = imgPath;
        this.tesseractPath = tesseractPath;
        OCR();
    }

    private void OCR() {

        using (Process process = new Process()) {
            process.StartInfo.FileName = tesseractPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.Arguments = imgPath  + " - tsv quiet";

            process.Start();

            StreamReader reader = process.StandardOutput;

            string output = reader.ReadLine();

            string[] headers = output.Split(delimiters);

            Dictionary<string, int> colIndex = new Dictionary<string, int>();

            int i = 0;
            foreach (var h in headers) {
                colIndex.Add(h, i);
                i++;
            }

            var rows = new ArrayList();

            output = reader.ReadLine();
            while (output != null) { 
                string[] row = output.Split(delimiters);
                float conf = float.Parse(row[colIndex["conf"]]);
                if (conf >= accuracyMinPercent) {
                    rows.Add(row);
                }
                output = reader.ReadLine();
            }

            i = 0;

            foreach (string[] row in rows) {
                string top = row[colIndex["top"]];
                string left = row[colIndex["left"]];
                string width = row[colIndex["width"]];
                string height = row[colIndex["height"]];
                string text = row[colIndex["text"]];

                if (i > 1 && Array.Exists(reqAmountKeywords, element 
                            => ((string[]) rows[i-1])[colIndex["text"]]
                            .IndexOf(element, StringComparison.OrdinalIgnoreCase) >= 0 )) {
                    if (moneyFormat.IsMatch(text) && reqAmount == 0) this.reqAmount = new Decimal(double.Parse(text));
                }

                else if (i > 1 && Array.Exists(receiptNumberKeywords, element 
                            => ((string[]) rows[i-1])[colIndex["text"]]
                            .IndexOf(element, StringComparison.OrdinalIgnoreCase) >= 0 )) {
                    if(receiptNumber == null) this.receiptNumber = text;
                }

                i++;
            }
            process.WaitForExit();
        }

    }
}
