using System.Dynamic;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public class CookieStorage
    {
        
        public string CookieStoragePath { get; set; }

        /// <summary>Stores a cookie object as JSON in a file.</summary>
        /// <param name="cookie">The cookie object to be stored.</param>
        /// <exception cref="Exception">Thrown when an error occurs during the serialization process.</exception>
        public void StoreCookie(Cookie cookie){
            try{
                string CookiePath = GetCookiePath();
                string json = JsonSerializer.Serialize(cookie);
                File.WriteAllText(CookiePath, json);
            }
            catch (Exception e){
                Console.WriteLine("Error: " + e);
            }
        }

        /// <summary>Retrieves a cookie from the cookie storage.</summary>
        /// <returns>The retrieved cookie, or null if the cookie doesn't exist or has expired.</returns>
        /// <exception cref="Exception">Thrown when an error occurs during the retrieval process.</exception>
        public Cookie RetrieveCookie(){
            if (File.Exists(CookieStoragePath))
            {
                try
                {
                    string CookiePath = GetCookiePath();
                    string json = File.ReadAllText(CookiePath);
                    Cookie cookie = JsonSerializer.Deserialize<Cookie>(json);
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

        private string GetCookiePath(){
            try{
                return CookieStoragePath + "\\cookie.json";
            }
            catch{
                Console.WriteLine("Failed to write to " + CookieStoragePath + "\\cookie.json");
                return null;
            }
        }

        /// <summary>Clears the cookies by deleting the cookie JSON file.</summary>
        /// <remarks>
        /// This method deletes the cookie JSON file located at the specified CookieStoragePath.
        /// </remarks>
        public void ClearCookies()
        {
            string CookiePath = GetCookiePath();
            File.Delete(CookiePath);
            Console.WriteLine("Successfully Cleared Cookies -> Deleted cookie JSON file.");
        }
    }
}