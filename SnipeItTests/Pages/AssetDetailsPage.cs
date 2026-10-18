using Microsoft.Playwright;

namespace SnipeItTests.Pages;

public sealed class AssetDetailsPage(IPage page)
{
    public ILocator DetailsPanel => page.Locator("#details");
    public ILocator AssetPageContent => page.Locator(".content-wrapper");
    public ILocator HistoryPanel => page.Locator("#history");

    private ILocator HistoryTab => page.GetByRole(AriaRole.Link, new() { Name = "History" });

    public ILocator HistoryRows => HistoryPanel.Locator("table tbody tr");

    public async Task OpenHistoryTabAsync()
    {
        await HistoryTab.ClickAsync();
        await Assertions.Expect(HistoryPanel).ToBeVisibleAsync();
    }
}
