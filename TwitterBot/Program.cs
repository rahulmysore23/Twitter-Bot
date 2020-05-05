using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace TwitterBot
{
    class BotEngine
    {
        public static IWebDriver driver;
        public static int noOfPosts { get; set; }
        public static string hashTag { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the Hash Tag you want to like posts for: ");
            hashTag = Console.ReadLine();

            Console.WriteLine("Enter then no.of posts to be liked: ");
            noOfPosts = Int32.Parse(Console.ReadLine());

            driver = new ChromeDriver
            {
                Url = "https://twitter.com/login"
            };

            driver.Manage().Window.Maximize();

            Login();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            Search();

            Thread.Sleep(5000);

            GetPosts();
        }

        public static void Login()
        {
            driver.FindElement(By.CssSelector("#react-root > div > div > div.css-1dbjc4n.r-13qz1uu.r-417010 > main > div > div > form > div > div:nth-child(6) > label > div > div.css-1dbjc4n.r-18u37iz.r-16y2uox.r-1wbh5a2.r-1udh08x > div > input")).SendKeys("");
            driver.FindElement(By.CssSelector("#react-root > div > div > div.css-1dbjc4n.r-13qz1uu.r-417010 > main > div > div > form > div > div:nth-child(7) > label > div > div.css-1dbjc4n.r-18u37iz.r-16y2uox.r-1wbh5a2.r-1udh08x > div > input")).SendKeys("");
            driver.FindElement(By.CssSelector("#react-root > div > div > div.css-1dbjc4n.r-13qz1uu.r-417010 > main > div > div > form > div > div:nth-child(8) > div > div")).Click();
        }

        public static void Search()
        {
            driver.FindElement(By.CssSelector("#react-root > div > div > div.css-1dbjc4n.r-18u37iz.r-13qz1uu.r-417010 > main > div > div > div > div.css-1dbjc4n.r-aqfbo4.r-zso239.r-1hycxz > div > div.css-1dbjc4n.r-gtdqiz.r-1hycxz > div > div > div > div.css-1dbjc4n.r-1awozwy.r-aqfbo4.r-yfoy6g.r-18u37iz.r-1h3ijdo.r-15d164r.r-1vsu8ta.r-1xcajam.r-ipm5af.r-1hycxz.r-136ojw6 > div > div > div > form > div.css-1dbjc4n.r-1wbh5a2 > div > div > div.css-901oao.r-jwli3a.r-6koalj.r-16y2uox.r-1qd0xha.r-a023e6.r-16dba41.r-ad9z0x.r-bcqeeo.r-qvutc0 > input")).SendKeys(hashTag);
            driver.FindElement(By.CssSelector("#react-root > div > div > div.css-1dbjc4n.r-18u37iz.r-13qz1uu.r-417010 > main > div > div > div > div.css-1dbjc4n.r-aqfbo4.r-zso239.r-1hycxz > div > div.css-1dbjc4n.r-gtdqiz.r-1hycxz > div > div > div > div.css-1dbjc4n.r-1awozwy.r-aqfbo4.r-yfoy6g.r-18u37iz.r-1h3ijdo.r-15d164r.r-1vsu8ta.r-1xcajam.r-ipm5af.r-1hycxz.r-136ojw6 > div > div > div > form > div.css-1dbjc4n.r-1wbh5a2 > div > div > div.css-901oao.r-jwli3a.r-6koalj.r-16y2uox.r-1qd0xha.r-a023e6.r-16dba41.r-ad9z0x.r-bcqeeo.r-qvutc0 > input")).SendKeys(Keys.Enter);
        }

        public static void GetPosts()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            
            int count = 0;
            bool flag = false;

            while (!flag)
            {
                IList<IWebElement> HeartCollection = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("div[data-testid='like']")));
                var length = HeartCollection.Count;

                for (int i = 0; i < length; i++)
                {
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                    try
                    {
                        executor.ExecuteScript("arguments[0].click();", HeartCollection[i]);
                        Thread.Sleep(1000);
                        executor.ExecuteScript("arguments[0].scrollIntoView(true);", HeartCollection[i]);
                        count++;
                    }
                    catch { }

                    if (count == noOfPosts)
                    {
                        flag = true;
                        break;
                    }
                }

                IJavaScriptExecutor d = (IJavaScriptExecutor)driver;
                d.ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
            }

            Console.WriteLine($"Hurray {noOfPosts} Posts Liked!");
        }
    }
}
