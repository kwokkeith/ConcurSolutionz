
namespace ConcurSolutionz.Database
{
    public class Cookie
    {
        public DateTime ExpiryDate {get; private set;}
        public string bm_sz {get; private set;}
        public string TAsessionID {get; private set;}
        public string ak_bmsc {get; private set;}
        public string _abck {get; private set;}
        public string OTSESSIONAABQRD {get; private set;}
        public string JWT {get; private set;}
        public string bm_sv {get; private set;}
        
        private Cookie(CookieBuilder cookieBuilder) {
            Utilities.CheckNull(cookieBuilder.ExpiryDate);
            Utilities.CheckNull(cookieBuilder.bm_sz);
            Utilities.CheckNull(cookieBuilder.TAsessionID);
            Utilities.CheckNull(cookieBuilder.ak_bmsc);
            Utilities.CheckNull(cookieBuilder._abck);
            Utilities.CheckNull(cookieBuilder.OTSESSIONAABQRD);
            Utilities.CheckNull(cookieBuilder.JWT);
            Utilities.CheckNull(cookieBuilder.bm_sv);

            this.ExpiryDate = cookieBuilder.ExpiryDate;
            this.bm_sz = cookieBuilder.bm_sz;
            this.TAsessionID = cookieBuilder.TAsessionID;
            this.ak_bmsc = cookieBuilder.ak_bmsc;
            this._abck = cookieBuilder._abck;
            this.OTSESSIONAABQRD = cookieBuilder.OTSESSIONAABQRD;
            this.JWT = cookieBuilder.JWT;
            this.bm_sv = cookieBuilder.bm_sv;
        }

        public class CookieBuilder
        {
            public DateTime ExpiryDate {get; private set;}
            public string bm_sz {get; private set;}
            public string TAsessionID {get; private set;}
            public string ak_bmsc {get; private set;}
            public string _abck {get; private set;}
            public string OTSESSIONAABQRD {get; private set;}
            public string JWT {get; private set;}
            public string bm_sv {get; private set;}

            public CookieBuilder(){
                // default values
            }
            
            public CookieBuilder SetExpiryDate(DateTime expiryDate){
                this.ExpiryDate = expiryDate;
                return this;
            }

            public CookieBuilder SetBm_sz(string bm_sz){
                this.bm_sz = bm_sz;
                return this;
            }

            public CookieBuilder SetTAsessionID(string TAsessionID){
                this.TAsessionID = TAsessionID;
                return this;
            }

            public CookieBuilder SetAk_bmsc(string ak_bmsc){
                this.ak_bmsc = ak_bmsc;
                return this;
            }

            public CookieBuilder Set_abck(string _abck){
                this._abck = _abck;
                return this;
            }

            public CookieBuilder SetOTSESSIONAABQRD(string OTSESSIONAABQRD){
                this.OTSESSIONAABQRD = OTSESSIONAABQRD;
                return this;
            }

            public CookieBuilder SetJWT(string JWT){
                this.JWT = JWT;
                return this;
            }

            public CookieBuilder SetBm_sv(string bm_sv){
                this.bm_sv = bm_sv;
                return this;
            }
        
            public Cookie Build(){
                return new Cookie(this);
            }
        }    
    }
}