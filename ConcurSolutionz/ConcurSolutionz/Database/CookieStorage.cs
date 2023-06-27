using System.Text.Json;

namespace ConcurSolutionz.Database
{
    public class CookieStorage
    {
        
        private Cookie Cookie;

        private void setCookie(Cookie cookie){
            this.Cookie = cookie;
        }


        private Cookie getCookie(){
            return(this.Cookie);
        }

        private String storeCookie(Cookie cookie){
            try{
                String json = JsonSerializer.Serialize(cookie);
                return(json);
            }
            catch (Exception e){
                Console.WriteLine("Error: " + e);
                return(null);
            }
        }

        private Cookie retrieveCookie(String json){
            return(System.Text.Json.JsonSerializer.Deserialize<Cookie>(json));
        }

        private void clearCookies(){
            setCookie(null);
        }        
    }
}