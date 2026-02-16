namespace LocatorsForWebElements.TestLayer.Models;

/// <summary>
/// Allows not having duplicate names for headless/normal mode in Test Explorer.
/// </summary>
internal class FixtureModel
{
    public FixtureModel(bool isHeadless, string name)
    {
        IsHeadless = isHeadless;
        Name = name;
    }

    public bool IsHeadless { get; }

    public string Name { get; }

    public override string ToString() => Name;
}
