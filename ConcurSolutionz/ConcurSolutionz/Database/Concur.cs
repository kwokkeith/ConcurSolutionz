
namespace ConcurSolutionz.Database
{
    public class Concur: Settings
    {
        private string CookieStoragePath;

        public void SetCookieStoragePath(string path)
        {
            CookieStoragePath = path;
        }
    
        public string GetCookieStoragePath()    
        {
            return CookieStoragePath;    
        }

        public Cookie RequestCookieHandler()
        {
            string CookiePath = GetCookieStoragePath();
            if (File.Exists(CookiePath))
            {
                try
                {
                    string json = File.ReadAllText(CookiePath);
                    Cookie cookie = System.Text.Json.JsonSerializer.Deserialize<Cookie>(json);
                    if (cookie.ExpiryDate < DateTime.Now)
                    {
                        Console.WriteLine("Cookie has expired.");
                        return null;
                    }
                    else
                    {
                        return cookie;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Cookie file does not exist.");
                return null;
            }
        }


    }

}