using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;

namespace ConcurSolutionz.Controllers
{
    public class ReceiptOCR
    {
        public string receiptNumber;
        public Decimal reqAmount;
        public string imgPath;
        public List<(Point[],string)> textBoxes = new List<(Point[],string)>();
        public double imgWidth;
        public double imgHeight;

        private string tesseractPath;
        private string tessdataPath;

        private static Char[] delimiters = new Char[] {' ','\t','\n'};
        private static float accuracyMinPercent = 0.0F;
        private static string[] reqAmountKeywords = new string[] 
        {"Total", "Amount", "Grand Total", "Amount Due"};
        private static string[] receiptNumberKeywords = new string[] 
        {"Receipt", "Inv", "Invoice", "No", "Slip", "Rcpt"};

        private Regex moneyFormat = new Regex("([A-Za-z$]?)+[0-9]*\\.[0-9]+", RegexOptions.IgnoreCase);
        private Regex nonNumeric = new Regex("[^0-9.]");

        public ReceiptOCR(string imgPath, string tesseractPath, string tessdataPath) {
            this.imgPath = imgPath;
            this.tesseractPath = tesseractPath;
            this.tessdataPath = tessdataPath;
            OCR();
        }

        public void RefineOCR(double x, double y) {
            try {
                foreach((Point[] pc, string text) in this.textBoxes) {
                    Point topLeft = pc[0];
                    Point bottomRight = pc[2];
                    if (topLeft.X <= x && bottomRight.X >= x && topLeft.Y <= y && bottomRight.Y >= y) {
                        if (y > this.imgHeight/2) {
                            this.reqAmount = new Decimal(double.Parse(text));
                        } else {
                            this.receiptNumber = text;
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }

        private void OCR() {

            using (Process process = new Process()) {
                process.StartInfo.FileName = tesseractPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.StartInfo.Arguments = "--tessdata-dir " + tessdataPath + " \"" + imgPath + "\" - tsv quiet";

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
                    string level = row[colIndex["level"]];
                    if (conf >= accuracyMinPercent || level == "1") {
                        rows.Add(row);
                    }
                    output = reader.ReadLine();
                }

                i = 0;

                foreach (string[] row in rows) {
                    string level = row[colIndex["level"]];
                    double height = double.Parse(row[colIndex["width"]]);
                    double width = double.Parse(row[colIndex["height"]]);
                    if (level == "1") {
                        this.imgWidth = height;
                        this.imgHeight = width;
                    }
                    double left = double.Parse(row[colIndex["top"]]);
                    double top = double.Parse(row[colIndex["left"]]);
                    string text = row[colIndex["text"]];

                    if (text != "") {
                        //PointCollection pc = new PointCollection();
                        Point[] pc = new Point[4];

                        pc[0] = new Point(top, left);
                        pc[1] = new Point(top, left+width);
                        pc[2] = new Point(top+height, left+width);
                        pc[3] = new Point(top+height, left);

                        //Point topLeft = new Point(top, left);
                        //Point topRight = new Point(top, left+width);
                        //Point bottomRight = new Point(top+height, left+width);
                        //Point bottomLeft = new Point(top+height, left);

                        //pc.Add(topLeft);
                        //pc.Add(topRight);
                        //pc.Add(bottomRight);
                        //pc.Add(bottomLeft);
                        textBoxes.Add((pc,text));
                    }

                    if (i > 1 && Array.Exists(reqAmountKeywords, element 
                                => ((string[]) rows[i-1])[colIndex["text"]]
                                .IndexOf(element, StringComparison.OrdinalIgnoreCase) >= 0 )) {
                        if (moneyFormat.IsMatch(text)) {
                            text = nonNumeric.Replace(text,"");
                            this.reqAmount = new Decimal(double.Parse(text));
                        }
                    }

                    else if (i > 1 && Array.Exists(receiptNumberKeywords, element 
                                => ((string[]) rows[i-1])[colIndex["text"]]
                                .IndexOf(element, StringComparison.OrdinalIgnoreCase) >= 0 )) {
                        if(receiptNumber == null) this.receiptNumber = text;
                    }

                    //Console.WriteLine(text);
                    i++;
                }
                process.WaitForExit();
            }

        }

    }
}
