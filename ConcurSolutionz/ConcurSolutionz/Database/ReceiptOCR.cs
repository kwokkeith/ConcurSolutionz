#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConcurSolutionz.Database
{
    public class ReceiptOCR
    {
        // Possible to be empty (i.e. OCR fails to retrieve information)

        public string? ReceiptNumber { get; set; }
        public decimal? ReqAmount { get; set; }

    }
}