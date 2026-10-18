using Microsoft.Playwright;

namespace SnipeItTests.Pages;

public sealed class LoginPage(IPage page)
{
    private ILocator UsernameInput => page.Locator("#username");
    private ILocator PasswordInput => page.Locator("#password-field");
    private ILocator LoginButton => page.GetByRole(AriaRole.Button, new() { Name = "Login" });

    public async Task GotoAsync() => await page.GotoAsync("/login");

    public async Task LoginAsync(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}
