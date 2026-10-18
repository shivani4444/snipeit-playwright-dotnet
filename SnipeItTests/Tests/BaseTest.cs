using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using SnipeItTests.Config;

namespace SnipeItTests.Tests;

/// <summary>
/// Common setup: base URL, timeouts and a Playwright trace that is kept only when a test fails.
/// Open a saved trace with:  pwsh bin/Debug/net10.0/playwright.ps1 show-trace &lt;file&gt;
/// </summary>
public abstract class BaseTest : PageTest
{
    public override BrowserNewContextOptions ContextOptions() => new()
    {
        BaseURL = TestSettings.BaseUrl,
        ViewportSize = new() { Width = 1440, Height = 900 },
    };

    [SetUp]
    public async Task BaseSetUp()
    {
        Page.SetDefaultTimeout(TestSettings.DefaultTimeoutMs);
        await Context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true, Sources = true });
    }

    [TearDown]
    public async Task BaseTearDown()
    {
        var failed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;
        var path = failed
            ? Path.Combine(TestContext.CurrentContext.WorkDirectory, "traces",
                $"{TestContext.CurrentContext.Test.Name}-{DateTime.UtcNow:yyyyMMddHHmmss}.zip")
            : null;

        await Context.Tracing.StopAsync(new() { Path = path });

        if (path is not null)
            TestContext.AddTestAttachment(path, "Playwright trace");
    }
}
