using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.IO;

namespace Sans
{
    [TestClass]
    public class SansTest
    {
        public ChromeDriver driver;
        public string url;
        public string driverUrl;
        string maxChars = "Send Title Max CharsSend Title Max CharsSend Title Max CharsSend Title Max CharsSend Title Max Chars" +
            "Send Title Max CharsSend Title Max CharsSend Title Max CharsSend Title Max CharsSend Title Max Chars";

        /// <summary>
        /// Navigates driver to URL to be tested.  Note: This URL path might need to be updated.
        /// </summary>
        [TestInitialize] 
        public void BeforeTest()
        {
            // driver location @file:///C:/Projects/Sans/Sans/Index.html
            url = Path.Combine(Environment.CurrentDirectory, @"index.html");
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Test to make sure we are at the correct URL.  Not sure how this test
        /// will behave on a different machine.  Hopefully the paths work and match up.
        /// </summary>
        [TestMethod]
        public void NavigateToUrlTest()
        {
            driverUrl = driver.Url;
            Assert.AreEqual(driverUrl, @"file:///" + url.Replace(@"\", @"/"));
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }


        /// <summary>
        /// Set Title max (200) chars and verify input.
        /// </summary>
        [TestMethod]
        public void SendTitleMaxChars()
        {
            driver.FindElement(By.Id("title")).SendKeys(maxChars);
            string actual = driver.FindElement(By.Id("title")).GetAttribute("value");
            Assert.AreEqual(maxChars, actual);
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }

        /// <summary>
        /// Set Title max + 1 (201) chars and verify input.
        /// </summary>
        [TestMethod]
        public void SendTitleOneOverMaxChars()
        {
            driver.FindElement(By.Id("title")).SendKeys(maxChars + "X");
            string actual = driver.FindElement(By.Id("title")).GetAttribute("value");
            Assert.AreEqual(maxChars, actual);
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }

        /// <summary>
        /// Set Release Date "1/10/2015" and verify input.
        /// </summary>
        [TestMethod]
        public void SetReleaseDateToLowerBound()
        {
            var str = "01/10/2015";

            driver.FindElement(By.Id("releaseDate")).SendKeys(str);
            string actual = driver.FindElement(By.Id("releaseDate")).GetAttribute("value");
            Assert.AreEqual("2015-01-10", actual);
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }

        /// <summary>
        /// Set Release Date "01/09/2015" and verify input.
        /// </summary>
        [TestMethod]
        public void SetReleaseDateToBelowLowerBound()
        {
            driver.FindElement(By.Id("title")).SendKeys(maxChars);
            driver.FindElement(By.Id("releaseDate")).SendKeys("01/09/2015");
            driver.FindElement(By.Id("rating")).SendKeys("1");
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }

        /// <summary>
        /// Set Rating "1" and verify input.
        /// </summary>
        [TestMethod]
        public void SetRatingToMin()
        {
            driver.FindElement(By.Id("rating")).SendKeys("1");
            string actual = driver.FindElement(By.Id("rating")).GetAttribute("value");
            Assert.AreEqual("1", actual);
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }

        /// <summary>
        /// Set Rating "5" and verify input.
        /// </summary>
        [TestMethod]
        public void SetRatingToMax()
        {
            driver.FindElement(By.Id("rating")).SendKeys("5");
            string actual = driver.FindElement(By.Id("rating")).GetAttribute("value");
            Assert.AreEqual("5", actual);
            Assert.IsFalse(driver.FindElement(By.Id("submit")).Enabled);
        }

        /// <summary>
        /// Happy path fill every element out with correct data and check if submit button is enabled.
        /// </summary>
        [TestMethod]
        public void HappyPath()
        {
            driver.FindElement(By.Id("title")).SendKeys(maxChars);
            driver.FindElement(By.Id("releaseDate")).SendKeys("01/10/2015");
            driver.FindElement(By.Id("rating")).SendKeys("1");
            //driver.FindElement(By.Id("rating")).SendKeys(Keys.Tab);

            Assert.IsTrue(driver.FindElement(By.Id("submit")).Enabled);
            driver.FindElement(By.Id("submit")).Click();

            // Validate database information was updated accordingly
        }

        /// <summary>
        /// Close the opened driver if needed.  This would normally be handled in test teardown.
        /// </summary>
        [TestCleanup]
        public void AfterTest()
        {
            driver.Close();
            driver.Dispose();
        }
    }
}
