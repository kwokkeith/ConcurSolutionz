using OpenQA.Selenium.Chrome;
using Cookie = OpenQA.Selenium.Cookie;

namespace CookieBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Received the following arguments:\n");

            if (args.Length != 3)
            {
                Console.WriteLine("Not all arguments sent");
                Environment.Exit(0);
            }

            Console.WriteLine($"JWT: {args[0]}");
            Console.WriteLine($"OTSESSIONAABQRD: {args[1]}");
            Console.WriteLine($"OTSESSIONAABQRN: {args[2]}");

            ChromeOptions options = new ChromeOptions();
            ChromeDriver driver;

            string JWT = args[0];
            string BRQD = args[1];
            string BQRN = args[2];

            options.AddArgument("start-maximized");
            options.AddArgument("disable-infobars");
            options.AddArgument("--incognito");
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddArgument("--dns-prefetch-disable");
            driver = new ChromeDriver(".", options);
            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            driver.Navigate().GoToUrl("https://www.concursolutions.com/");

            Cookie CookieJWT = new Cookie("JWT", JWT, ".concursolutions.com", "/", null, true, true, "Lax");
            Cookie CookieBQRD = new Cookie("OTSESSIONAABQRD", BRQD, ".concursolutions.com", "/", null, true, true, "Lax");
            Cookie CookieBQRN = new Cookie("OTSESSIONAABQRN", BQRN, ".concursolutions.com", "/", null, true, true, "Lax");

            driver.Manage().Cookies.AddCookie(CookieJWT);
            driver.Manage().Cookies.AddCookie(CookieBQRD);
            driver.Manage().Cookies.AddCookie(CookieBQRN);
            driver.Navigate().GoToUrl("https://www.concursolutions.com/home.asp");

            //On browser closer, exit
            try
            {
                string temp;
                while (true) temp = driver.Title;
            }
            catch (Exception ex)
            {
                driver.Quit();
                Environment.Exit(0);
            }

        }
    }
}

