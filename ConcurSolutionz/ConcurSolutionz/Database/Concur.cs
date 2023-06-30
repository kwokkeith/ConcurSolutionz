
namespace ConcurSolutionz.Database
{
    public class Concur: Settings
    {

        private CookieStorage cookieStorage;
        public CookieStorage CookieStorage { 
            get { return cookieStorage; } 
            set{
                cookieStorage = value;
                cookieStorage.CookieStoragePath = GetRootDirectory() + "\\CookieStorage";
                
                // Check if cookieStorage exist, if not then create it
                if (!File.Exists(cookieStorage.CookieStoragePath)){
                    Directory.CreateDirectory(cookieStorage.CookieStoragePath);
                }
            } }
    }

}