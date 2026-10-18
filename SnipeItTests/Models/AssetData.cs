namespace SnipeItTests.Models;

/// <summary>
/// Data describing the asset the test creates. Values that are only known after
/// interacting with the UI (asset tag, chosen user) are filled in during the test.
/// </summary>
public sealed record AssetData
{
    public string ModelName { get; init; } = "Macbook Pro 13\"";
    public string StatusName { get; init; } = "Ready to Deploy";

    /// <summary>Unique serial so the asset is easy to identify on a shared demo instance.</summary>
    public string Serial { get; init; } = $"AUTO-{Guid.NewGuid():N}"[..17].ToUpperInvariant();

    public string AssetTag { get; set; } = string.Empty;
    public string AssignedUserName { get; set; } = string.Empty;
}
