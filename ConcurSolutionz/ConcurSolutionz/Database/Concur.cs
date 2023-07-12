
namespace ConcurSolutionz.Database
{
    public class Concur: Settings
    {
        public Concur(){
            SubType = "Concur";
        }

        private CookieStorage cookieStorage;
        public CookieStorage CookieStorage { 
            get { return cookieStorage; } 
            set{
                cookieStorage = value;
                cookieStorage.CookieStoragePath = Path.Combine(GetRootDirectory(), "CookieStorage");
            } }
    }

}