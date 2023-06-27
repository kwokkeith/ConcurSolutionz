using System.Dynamic;
using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public class CookieStorage
    {
        
        public Cookie Cookie {get; set;}

        public static string StoreCookie(Cookie cookie){
            try{
                string json = JsonSerializer.Serialize(cookie);
                return(json);
            }
            catch (Exception e){
                Console.WriteLine("Error: " + e);
                return(null);
            }
        }

        public static Cookie RetrieveCookie(string json){
            return(System.Text.Json.JsonSerializer.Deserialize<Cookie>(json));
        }

        public void ClearCookies(){
            Cookie = null;
        }        
    }
}