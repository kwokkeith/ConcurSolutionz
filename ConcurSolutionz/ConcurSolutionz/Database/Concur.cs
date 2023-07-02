
namespace ConcurSolutionz.Database
{
    public class Concur: Settings
    {

        private CookieStorage cookieStorage;
        public CookieStorage CookieStorage { 
            get { return cookieStorage; } 
            set{
                cookieStorage = value;
                cookieStorage.CookieStoragePath = Path.Combine(GetRootDirectory(), "CookieStorage");
            } }
    }

}