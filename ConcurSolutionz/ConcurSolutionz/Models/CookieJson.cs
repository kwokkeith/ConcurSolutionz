using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurSolutionz.Models
{
    internal class CookieJson
    {
        public string domain {  get; set; }
        public double expirationDate { get; set; }
        public bool HostOnly { get; set; }

        public bool httpOnly { get; set; }

        public string name { get; set; }

        public string path { get; set; }

        public string sameSite { get; set; }

        public bool secure { get; set; }

        public bool session { get; set; }

        public string storeId { get; set; }

        public string value { get; set; }
    }
}
