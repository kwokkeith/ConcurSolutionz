using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


ChromeOptions options = new ChromeOptions();
ChromeDriver driver;


options.AddArgument("start-maximized");
options.AddArgument("disable-infobars");
options.AddArgument("--incognito");
options.AddUserProfilePreference("credentials_enable_service", false);
options.AddUserProfilePreference("profile.password_manager_enabled", false);
options.AddArgument("--dns-prefetch-disable");

driver = new ChromeDriver(".", options);
driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
driver.Navigate().GoToUrl("https://us.concursolutions.com");

while (!driver.Url.Contains("concursolutions.com/home.asp")) ;
//Console.ReadLine();
var cookies = driver.Manage().Cookies.AllCookies;
string cookie = "";
foreach (Cookie c in cookies)
{
    if (c.Name.Equals("OTSESSIONAABQRN") || c.Name.Equals("OTSESSIONAABQRD") || c.Name.Equals("JWT")) cookie += c.Name + "=" + c.Value + ";";
}
cookie = cookie.Remove(cookie.Length - 1);
Console.WriteLine(cookie);
driver.Quit();
