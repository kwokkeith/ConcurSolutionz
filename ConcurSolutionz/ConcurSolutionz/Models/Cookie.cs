using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESC_HTTP_Call.Models
{
    public sealed class Cookie
    {
        private static readonly Lazy<Cookie> lazy = new Lazy<Cookie>(() => new Cookie());

        public string RawCookie { get; set; }
        private string AWSALBTG = "";
        private string AWSALBTGCORS = "";
        private string JWT = "";
        private string OTSESSIONAABQRD = "";
        private string OTSESSIONAABQRN = "";
        private string TAsessionID = "";
        private string _abck = "";
        private string ak_bmsc = "";
        private string akacd_us1 = "";
        private string bm_sv = "";
        private string bm_sz = "";

        public static Cookie Instance{get{return lazy.Value;}}

        public void SetCookie(string rawCookie)
        {
            RawCookie = rawCookie;
            string[] CookieFields = RawCookie.Split(';');

            for (int i = 0; i < CookieFields.Length; i++)
            {
                string CurrentLine = CookieFields[i].Trim();
                string temp = CurrentLine.Split('=')[0];
                if (CurrentLine.Contains("AWSALBTG=")) { AWSALBTG = CurrentLine + ';';}
                else if (CurrentLine.Contains("AWSALBTGCORS=")) { AWSALBTGCORS = CurrentLine + ';'; }
                else if (CurrentLine.Contains("JWT=")) { JWT = CurrentLine + ';'; }
                else if (CurrentLine.Contains("OTSESSIONAABQRD=")) { OTSESSIONAABQRD = CurrentLine + ';'; }
                else if (CurrentLine.Contains("OTSESSIONAABQRN=")) { OTSESSIONAABQRN = CurrentLine + ';'; }
                else if (CurrentLine.Contains("TAsessionID=")) { TAsessionID = CurrentLine + ';'; }
                else if (CurrentLine.Contains("_abck=")) { _abck = CurrentLine + ';'; }
                else if (CurrentLine.Contains("ak_bmsc=")) { ak_bmsc = CurrentLine + ';'; }
                else if (CurrentLine.Contains("akacd_us1=")) { akacd_us1 = CurrentLine + ';'; }
                else if (CurrentLine.Contains("bm_sv=")) { bm_sv = CurrentLine + ';'; }
                else if (CurrentLine.Contains("bm_sz=")) { bm_sz = CurrentLine + ';'; }
            }
        }

        public void PrintAll()
        {
            Console.WriteLine("PRINTING ALL \n\n");

            if (AWSALBTG == "") Console.WriteLine("AWSALBTG is null");
            else Console.WriteLine(AWSALBTG);
            if (AWSALBTGCORS == "") Console.WriteLine("AWSALBTGCORS is null");
            else  Console.WriteLine(AWSALBTGCORS);
            if (JWT == "") Console.WriteLine("JWT is null"); 
            else Console.WriteLine(JWT);
            if (OTSESSIONAABQRD == "") Console.WriteLine("OTSESSIONAABQRD is null");
            else Console.WriteLine(OTSESSIONAABQRD);
            if (OTSESSIONAABQRN == "") Console.WriteLine("OTSESSIONAABQRN is null");
            else Console.WriteLine(OTSESSIONAABQRN);
            if (TAsessionID == "") Console.WriteLine("TAsessionID is null");
            else Console.WriteLine(TAsessionID);
            if (_abck == "") Console.WriteLine("_abck is null");
            else Console.WriteLine(_abck);
            if (ak_bmsc  == "") Console.WriteLine("ak_bmsc is null");
            else Console.WriteLine(ak_bmsc);
            if (akacd_us1 == "") Console.WriteLine("akacd_us1 is null");
            else Console.WriteLine(akacd_us1);
            if (bm_sv == "") Console.WriteLine("bm_sv is null");
            else Console.WriteLine(bm_sv);
            if (bm_sz == "") Console.WriteLine("bm_sz is null");
            else Console.WriteLine(bm_sz);
        }
    }
}
