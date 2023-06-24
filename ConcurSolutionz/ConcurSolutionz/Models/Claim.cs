using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC_HTTP_Call.Models
{
    internal class Claim
    {
        //Id of the claim as reflected on Concur(rptId)
        public string Id { get; set; }
        //Key of the claim as reflected on Concur(rptKey)
        public string Key { get; set;
        //Name of the Claim
        public string Name { get; set; }
        public string TeamName { get; set; }
        public string Purpose { get; set; }
        public string Date { get; set; } //As YYYY-MM-DD
        //Policy of the claim, what fifth row/event the claim falls under
        public string Policy { get; set; }

    }
}
