
namespace ConcurSolutionz.Database
{
    public class CookieBuilder
    {
        private Cookie cookie;
        
        private DateTime expiryDate;
        private string bm_sz;
        private string TAsessionID;
        private string ak_bmsc;
        private string _abck;
        private string OTSESSIONAABQRD;
        private string JWT;
        private string bm_sv;

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
        return cookie;
    }


    }
}