using Microsoft.Playwright;

namespace SnipeItTests.Pages;

public sealed class AssetsListPage(IPage page)
{
    private ILocator SearchInput => page.Locator(".bootstrap-table .search input");
    private ILocator Rows => page.Locator("table#assetsListingTable tbody tr");

    public async Task GotoAsync() => await page.GotoAsync("/hardware");

    public async Task SearchAsync(string term)
    {
        await SearchInput.FillAsync(term);
        await SearchInput.PressAsync("Enter");
    }

    public ILocator RowFor(string assetTag) => Rows.Filter(new() { HasText = assetTag });

    public async Task OpenAssetAsync(string assetTag) =>
        await RowFor(assetTag).GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true }).First.ClickAsync();
}
