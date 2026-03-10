using SeleniumWebdriverTask.BusinessLayer.Pages;

namespace SeleniumWebdriverTask.TestLayer.Tests
{
    /// <summary>
    /// Tests related to correctly navigating to pages.
    /// </summary>
    public class NavigationTests : BaseUiTest
    {
        /// <summary>
        /// Verifies that slide text on the insights page is contained in the title of the article on that insight's page.
        /// </summary>
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

            var slideTitle = FormatTitle(insightsPage.GetActiveSlideTitle());
            insightsPage.ClickMoreInfo();

            var insightPage = new InsightBasePage(Driver);
            var pageTitle = FormatTitle(insightPage.GetTitle());

            LogComparedTitles(slideTitle, pageTitle);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(pageTitle, Is.Not.Empty);
                Assert.That(slideTitle, Is.Not.Empty);
            }

            Assert.That(pageTitle, Does.Contain(slideTitle));
        }

        private static string FormatTitle(string title)
        {
            // Trimming whitespace and removing non-breaking spaces.
            return title.Trim().Replace('\u00A0', ' ');
        }

        private void LogComparedTitles(string slideTitle, string pageTitle)
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
