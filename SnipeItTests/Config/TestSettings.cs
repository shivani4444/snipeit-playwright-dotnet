namespace SnipeItTests.Config;

/// <summary>
/// Central place for environment-specific settings.
/// Values can be overridden with environment variables so no secrets need to live in code
/// (the defaults are the public credentials published on the Snipe-IT demo login page).
/// </summary>
public static class TestSettings
{
    public static string BaseUrl =>
        Environment.GetEnvironmentVariable("SNIPEIT_BASE_URL") ?? "https://demo.snipeitapp.com/login";

    public static string Username =>
        Environment.GetEnvironmentVariable("SNIPEIT_USERNAME") ?? "admin";

    public static string Password =>
        Environment.GetEnvironmentVariable("SNIPEIT_PASSWORD") ?? "password";

    /// <summary>Default timeout (ms) for actions such as click/fill. The public demo can be slow.</summary>
    public static float DefaultTimeoutMs => 30_000;
}
