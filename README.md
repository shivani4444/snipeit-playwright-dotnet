# Snipe-IT UI Automation (Playwright + .NET)

End-to-end test for the [Snipe-IT demo](https://demo.snipeitapp.com) written in C# with Playwright for .NET and NUnit.

## What the test does
1. Logs in to the demo
2. Creates a **Macbook Pro 13"** asset with status **Ready to Deploy**, checked out to a **random user**
3. Searches the assets list and confirms the new asset is there
4. Opens the asset page from the list and validates tag, serial, model, status and assigned user
5. Opens the **History** tab and validates the checkout and creation entries

## Tech stack
- .NET 10 (current LTS)
- Microsoft.Playwright.NUnit
- Page Object Model, with a reusable `Select2Dropdown` component for Snipe-IT's searchable dropdowns

## Project structure
```
SnipeItTests/
  Config/TestSettings.cs        # base URL & credentials (env-var overridable)
  Components/Select2Dropdown.cs # wrapper for Select2 widgets
  Models/AssetData.cs           # test data for the asset
  Pages/                        # page objects (Login, CreateAsset, AssetsList, AssetDetails)
  Tests/BaseTest.cs             # browser context, timeouts, trace-on-failure
  Tests/AssetTests.cs           # the end-to-end scenario
  test.runsettings              # browser / headless / expect timeout
.github/workflows/tests.yml     # CI on GitHub Actions
```

## Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PowerShell 7](https://learn.microsoft.com/powershell/scripting/install/installing-powershell) (`pwsh`) – used by Playwright's browser installer

## Run it
```bash
dotnet build
pwsh SnipeItTests/bin/Debug/net10.0/playwright.ps1 install chromium
dotnet test --settings SnipeItTests/test.runsettings
```

Watch the browser while it runs:
```bash
HEADED=1 dotnet test --settings SnipeItTests/test.runsettings          # macOS/Linux
$env:HEADED="1"; dotnet test --settings SnipeItTests/test.runsettings  # Windows PowerShell
```

## Configuration
Defaults are the public demo credentials. Override with environment variables if needed:

| Variable | Default |
|---|---|
| `SNIPEIT_BASE_URL` | `https://demo.snipeitapp.com` |
| `SNIPEIT_USERNAME` | `admin` |
| `SNIPEIT_PASSWORD` | `password` |

## Debugging failures
When a test fails, a Playwright trace is saved under `SnipeItTests/bin/Debug/net10.0/traces/`. Open it with:
```bash
pwsh SnipeItTests/bin/Debug/net10.0/playwright.ps1 show-trace <trace.zip>
```

## Notes
- The demo is a shared public instance that is reset periodically, so the test creates its own uniquely identified data (unique serial, auto-generated asset tag) and doesn't depend on existing records other than the "Macbook Pro 13"" model, the "Ready to Deploy" status and the presence of users.
- The demo can be slow; timeouts are set generously in `TestSettings` and `test.runsettings`.
