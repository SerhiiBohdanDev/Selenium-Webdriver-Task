using SeleniumWebdriverTask.BusinessLayer.Pages;

namespace SeleniumWebdriverTask.TestLayer
{
    internal class NavigationTests : TestBase
    {
        [Test]
        public void CorrectTitle_CompareSlideTitles_Success()
        {
            Logger.LogInformation($"Starting slide title comparison test.");
            new MainPage(Driver)
                .ClickInsights();

            var insightsPage = new InsightsPage(Driver);
            insightsPage
                .WaitUntilPageSwitched()
                .ClickNextSlide()
                .ClickNextSlide();

            var slideTitle = insightsPage.GetActiveSlideTitle().Trim();
            insightsPage.ClickMoreInfo();

            var insightPage = new InsightBasePage(Driver);
            var pageTitle = insightPage.GetTitle().Trim();

            LogComparedTitles(slideTitle, pageTitle);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(pageTitle, Is.Not.Empty);
                Assert.That(slideTitle, Is.Not.Empty);
            }

            Assert.That(pageTitle, Does.Contain(slideTitle));
        }

        private static void LogComparedTitles(string slideTitle, string pageTitle)
        {
            if (pageTitle.Contains(slideTitle))
            {
                Logger.LogInformation($"Titles match \n" +
                    $"Slide title = '{slideTitle}'\n" +
                    $"Page title = '{pageTitle}'");
            }
            else
            {
                Logger.LogError($"Titles DO NOT match \n" +
                    $"Slide title = '{slideTitle}'\n" +
                    $"Page title = '{pageTitle}'");
            }
        }
    }
}
