namespace ESC_HTTP_Call.Models
{
    internal class Expense
    {
        //Id of the expense reflected on Concur (ExpenseId)
        public string Id { get; set; }
        //ReportId of the claim that the expense is under
        public string ReportId { get; set; }
        //Key assigned to expense upon creation used to identify the expense
        public string RPEKey { get; set; }
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
