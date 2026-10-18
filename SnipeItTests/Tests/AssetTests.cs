using System.Text.RegularExpressions;
using NUnit.Framework;
using SnipeItTests.Config;
using SnipeItTests.Models;
using SnipeItTests.Pages;

namespace SnipeItTests.Tests;

[TestFixture]
[Parallelizable(ParallelScope.Self)]
public class AssetTests : BaseTest
{
    [Test]
    [Description("Create a Macbook Pro 13\" asset checked out to a random user, then verify it in the list, details and history.")]
    public async Task CreateAsset_CheckedOutToRandomUser_AppearsWithCorrectDetailsAndHistory()
    {
        var asset = new AssetData();

        // 1. Login
        var login = new LoginPage(Page);
        await login.GotoAsync();
        await login.LoginAsync(TestSettings.Username, TestSettings.Password);
        await Expect(Page).Not.ToHaveURLAsync(new Regex("/login"));

        // 2. Create the asset (Ready to Deploy, checked out to a random user)
        var createPage = new CreateAssetPage(Page);
        await createPage.GotoAsync();
        await createPage.CreateCheckedOutAssetAsync(asset);
        TestContext.Out.WriteLine($"Created asset {asset.AssetTag} for {asset.AssignedUserName}");

        // 3. Find the asset in the assets list
        var list = new AssetsListPage(Page);
        await list.GotoAsync();
        await list.SearchAsync(asset.AssetTag);
        var row = list.RowFor(asset.AssetTag);
        await Expect(row).ToHaveCountAsync(1);
        await Expect(row).ToContainTextAsync(asset.ModelName);
        await Expect(row).ToContainTextAsync(asset.AssignedUserName);

        // 4. Open the asset page from the list and validate details
        await list.OpenAssetAsync(asset.AssetTag);
        var details = new AssetDetailsPage(Page);
        await Expect(details.DetailsPanel).ToContainTextAsync(asset.AssetTag);
        await Expect(details.AssetPageContent).ToContainTextAsync(asset.Serial);
        await Expect(details.AssetPageContent).ToContainTextAsync(asset.ModelName);
        await Expect(details.DetailsPanel).ToContainTextAsync(asset.AssignedUserName);
        // Snipe-IT labels a checked-out "Ready to Deploy" asset as Deployed.
        await Expect(details.DetailsPanel).ToContainTextAsync(
            new Regex($"{Regex.Escape(asset.StatusName)}|Deployed", RegexOptions.IgnoreCase));

        // 5. Validate the History tab
        await details.OpenHistoryTabAsync();
        var checkoutEntry = details.HistoryRows.Filter(new() { HasTextRegex = new Regex("checkout", RegexOptions.IgnoreCase) });
        await Expect(checkoutEntry.First).ToContainTextAsync(asset.AssignedUserName);
        await Expect(details.HistoryRows.Filter(new() { HasTextRegex = new Regex("create", RegexOptions.IgnoreCase) }).First)
            .ToBeVisibleAsync();
    }
}
