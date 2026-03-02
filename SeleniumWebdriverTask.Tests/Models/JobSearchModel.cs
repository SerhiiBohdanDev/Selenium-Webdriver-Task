namespace SeleniumWebdriverTask.TestLayer.Models;

/// <summary>
/// A model class to be used for TestCaseSource in job search tests.
/// </summary>
internal class JobSearchModel
{
    /// <summary>
    /// Gets or sets an array of language names for the job search.
    /// </summary>
    public required string[] Language { get; set; }

    /// <summary>
    /// Gets or sets the location for the job search.
    /// </summary>
    public required string Location { get; set; }
}
