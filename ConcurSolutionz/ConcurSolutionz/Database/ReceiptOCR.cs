using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class ReceiptOCR : OCR
    {
        public string ReceiptNumber { get; set; }
        public Decimal ReqAmount { get; set; }

    }
}