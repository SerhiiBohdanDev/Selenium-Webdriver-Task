using OpenQA.Selenium;
using SeleniumWebdriverTask.BusinessLayer.Pages;
using SeleniumWebdriverTask.CoreLayer.WebDriver;

namespace SeleniumWebdriverTask.TestLayer.Tests
{
    /// <summary>
    /// Class to contain tests related to downloading a file.
    /// </summary>
    [Category("NunitUiTests")]
    public class DownloadTests : BaseDownloadTest
    {
        /// <summary>
        /// Verifies that Quick Facts file can be downloaded.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>A task object that can be awaited.</returns>
        [TestCase("EPAM_ESG_Quick_Facts.pdf")]
        public async Task EPAMQuickFacts_DownloadFile_FileDownloaded(string fileName)
        {
            Logger.LogInformation($"Starting download test for file '{fileName}'");
            new MainPage(Driver)
                .ClickCorporateResponsibility();

            new CorporateResponsibilityPage(Driver)
                .ScrollToDownloadLink()
                .ClickDownloadLink();

            Logger.LogInformation($"File will be saved at: {DownloadFolderPath}");
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var isFileDownloaded = await DriverWrapper.WaitForFileToFinishChangingContentAsync(DownloadFolderPath, fileName, 1, cancellationTokenSource.Token);

            Assert.That(isFileDownloaded, Is.True);
        }
    }
}
