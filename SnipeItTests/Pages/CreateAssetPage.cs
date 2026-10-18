using Microsoft.Playwright;
using SnipeItTests.Components;
using SnipeItTests.Models;

namespace SnipeItTests.Pages;

public sealed class CreateAssetPage(IPage page)
{
    private ILocator AssetTagInput => page.Locator("input[name='asset_tags[1]']");
    private ILocator SerialInput => page.Locator("input[name='serials[1]']");
    private ILocator SaveButton => page.Locator("#create-form").GetByRole(AriaRole.Button, new() { Name = "Save" }).First;
    private ILocator SuccessAlert => page.Locator(".alert-success");

    private Select2Dropdown Model => new(page, "model_select_id");
    private Select2Dropdown Status => new(page, "status_select_id");
    private Select2Dropdown AssignedUser => new(page, "assigned_user_select");

    public async Task GotoAsync() => await page.GotoAsync("/hardware/create");

    /// <summary>
    /// Fills and submits the form. Updates <paramref name="asset"/> with the generated
    /// asset tag and the randomly chosen user so later steps can verify them.
    /// </summary>
    public async Task CreateCheckedOutAssetAsync(AssetData asset)
    {
        // Snipe-IT pre-generates a unique asset tag; keep it and record it.
        asset.AssetTag = (await AssetTagInput.InputValueAsync()).Trim();

        await Model.SelectByTextAsync(asset.ModelName);
        await Status.SelectByTextAsync(asset.StatusName);
        await SerialInput.FillAsync(asset.Serial);

        // Choosing a deployable status reveals the "Checkout to" section (User is the default target).
        var userOption = await AssignedUser.SelectRandomAsync();
        asset.AssignedUserName = ExtractDisplayName(userOption);

        await SaveButton.ClickAsync();
        await Assertions.Expect(SuccessAlert).ToBeVisibleAsync();
    }

    /// <summary>
    /// Select2 user options look like "First Last (username) - #123".
    /// The asset page shows only the display name, so keep the part before " (".
    /// </summary>
    private static string ExtractDisplayName(string optionText)
    {
        var idx = optionText.IndexOf(" (", StringComparison.Ordinal);
        return idx > 0 ? optionText[..idx].Trim() : optionText.Trim();
    }
}
