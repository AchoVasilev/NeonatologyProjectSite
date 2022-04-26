namespace Test.Selenium
{
    using System;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    using Xunit;

    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver driver;
        private const string Url = "https://localhost:5001/";

        public AutomatedUITests()
            => driver = new ChromeDriver();

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Fact]
        public void HomeReturnsIndexView()
        {
            this.driver.Navigate()
                .GoToUrl(Url);

            Assert.Equal("Начало - Педиамед", this.driver.Title);
            Assert.Contains("Добре дошли в Педиамед", this.driver.PageSource);
        }

        [Fact]
        public void DoctorProfileReturnsView()
        {
            this.driver.Navigate()
                .GoToUrl(Url + "Doctor/Profile");

            Assert.Equal("Професионален профил - Педиамед", this.driver.Title);
            Assert.Contains("Биография", this.driver.PageSource);
        }

        [Fact]
        public void OfferAllReturnsView()
        {
            this.driver.Navigate()
               .GoToUrl(Url + "Offer/All");

            Assert.Equal("Нашите услуги - Педиамед", this.driver.Title);
            Assert.Contains("Вид услуга", this.driver.PageSource);
        }

        [Fact]
        public void GaleryAllReturnsView()
        {
            this.driver.Navigate()
               .GoToUrl(Url + "Gallery/All?page=1");

            Assert.Equal("Галерия - Педиамед", this.driver.Title);
            Assert.Contains("Галерия", this.driver.PageSource);
        }
    }
}
