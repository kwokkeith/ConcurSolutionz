
namespace ConcurSolutionz.Database
{
    public class Cookie
    {
        public DateTime expiryDate {get; private set;}
        public string bm_sz {get; private set;}
        public string TAsessionID {get; private set;}
        public string ak_bmsc {get; private set;}
        public string _abck {get; private set;}
        public string OTSESSIONAABQRD {get; private set;}
        public string JWT {get; private set;}
        public string bm_sv {get; private set;}
        
        private Cookie(CookieBuilder cookieBuilder) {
            Utilities.checkNull(cookieBuilder.expiryDate);
            Utilities.checkNull(cookieBuilder.bm_sz);
            Utilities.checkNull(cookieBuilder.TAsessionID);
            Utilities.checkNull(cookieBuilder.ak_bmsc);
            Utilities.checkNull(cookieBuilder._abck);
            Utilities.checkNull(cookieBuilder.OTSESSIONAABQRD);
            Utilities.checkNull(cookieBuilder.JWT);
            Utilities.checkNull(cookieBuilder.bm_sv);

            this.expiryDate = cookieBuilder.expiryDate;
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
            public DateTime expiryDate {get; set;}
            public string bm_sz {get; set;}
            public string TAsessionID {get; set;}
            public string ak_bmsc {get; set;}
            public string _abck {get; set;}
            public string OTSESSIONAABQRD {get; set;}
            public string JWT {get; set;}
            public string bm_sv {get; set;}

            CookieBuilder(){
                // default values
            }
            
            public void setExpiryDate(DateTime expiryDate){
            this.expiryDate = expiryDate;
        }

        public void setBm_sz(string bm_sz){
            this.bm_sz = bm_sz;
        }

        public void setTAsessionID(string TAsessionID){
            this.TAsessionID = TAsessionID;
        }

        public void setAk_bmsc(string ak_bmsc){
            this.ak_bmsc = ak_bmsc;
        }

        public void set_abck(string _abck){
            this._abck = _abck;
        }

        public void setOTSESSIONAABQRD(string OTSESSIONAABQRD){
            this.OTSESSIONAABQRD = OTSESSIONAABQRD;
        }

        public void setJWT(string JWT){
            this.JWT = JWT;
        }

        public void setBm_sv(string bm_sv){
            this.bm_sv = bm_sv;
        }

    protected Cookie Build()
    {
        return new Cookie(this);
    }

        }
    }    
}