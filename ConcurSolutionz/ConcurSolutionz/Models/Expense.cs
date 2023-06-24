using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC_HTTP_Call.Models
{
    internal class Expense
    {
        //Id of the expense reflected on Concur (ExpenseId)
        public string Id { get; set; }
        //ReportId of the claim that the expense is under
        public string ReportId { get; set; } 
        //Id of the image that should be attached to the expense
        public string ImageId { get; set; } 
        public string Date { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public string ReceiptNo { get; set; }
        public string Supplier { get; set; }
        public string Comment { get; set; }

    }
}
