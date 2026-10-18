using Microsoft.Playwright;

namespace SnipeItTests.Components;

/// <summary>
/// Reusable wrapper for the Select2 dropdowns Snipe-IT uses instead of native &lt;select&gt; elements.
/// Select2 hides the real select and renders a span with id "select2-{selectId}-container".
/// Options are loaded via AJAX, so we always wait for real results before choosing one.
/// </summary>
public sealed class Select2Dropdown
{
    private readonly IPage _page;
    private readonly ILocator _container;

    public Select2Dropdown(IPage page, string selectId)
    {
        _page = page;
        _container = page.Locator($"#select2-{selectId}-container");
    }

    private ILocator SearchField => _page.Locator(".select2-container--open .select2-search__field");

    // Exclude the "Searching…"/"No results" placeholder rows.
    private ILocator Options => _page.Locator(
        ".select2-container--open .select2-results__option:not(.loading-results):not(.select2-results__message)");

    public async Task SelectByTextAsync(string text)
    {
        await _container.ClickAsync();
        await SearchField.FillAsync(text);
        await Options.Filter(new() { HasText = text }).First.ClickAsync();
    }

    /// <summary>Opens the dropdown, picks a random option and returns its visible text.</summary>
    public async Task<string> SelectRandomAsync()
    {
        await _container.ClickAsync();
        await Options.First.WaitForAsync();

        var count = await Options.CountAsync();
        var option = Options.Nth(Random.Shared.Next(count));
        var text = (await option.InnerTextAsync()).Trim();

        await option.ClickAsync();
        return text;
    }

    public async Task<string> GetSelectedTextAsync() =>
        (await _container.GetAttributeAsync("title") ?? await _container.InnerTextAsync()).Trim();
}
