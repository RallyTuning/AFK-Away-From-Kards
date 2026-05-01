using Microsoft.Web.WebView2.Core;
using System.Net;
using System.Text.Json;

namespace AFK_Away_From_Kards_Master
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            _ = InitializeBrowser();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Lbl_LoginStatus.ForeColor = Color.Firebrick;
                Lbl_LoginStatus.Text = "Log in to Steam to let the app recognize cards and manage drops automatically";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task InitializeBrowser()
        {
            try
            {
                // Folder contains Steam session data (crypted by Windows API)
                string userDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SteamSessionData");

                if (!Directory.Exists(userDataFolder))
                    Directory.CreateDirectory(userDataFolder);

                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(null, userDataFolder);

                // Waiting for the WebView2 to be ready
                await WV2_Login.EnsureCoreWebView2Async(env);

                // Update the URL textbox whenever the source changes
                WV2_Login.SourceChanged += (s, e) => { Txt_Url.Text = WV2_Login.Source.ToString(); };

                // Limit navigation to Steam domains only
                WV2_Login.CoreWebView2.NavigationStarting += (s, e) =>
                {
                    string uri = e.Uri.ToLower();

                    // Just stay on Steam
                    bool isAllowed = uri.Contains("steampowered.com") ||
                                     uri.Contains("steamcommunity.com") ||
                                     uri.Contains("help.steampowered.com");

                    if (!isAllowed)
                    {
                        e.Cancel = true; // Cancel the navigation
                                         // Go back to the login page
                        WV2_Login.CoreWebView2.Navigate("https://store.steampowered.com/login/");
                    }
                };

                WV2_Login.Source = new Uri("https://store.steampowered.com/login/");



                CoreWebView2Settings settings = WV2_Login.CoreWebView2.Settings;

                settings.AreDefaultContextMenusEnabled = false;     // Disable right-click context menu
                settings.AreDevToolsEnabled = false;                // Disable DevTools
                settings.IsStatusBarEnabled = false;                // Hide the status bar at the bottom
                settings.IsZoomControlEnabled = false;              // Prevent zooming with Ctrl+Scroll
                settings.AreBrowserAcceleratorKeysEnabled = false;  // Disable shortcuts like Ctrl+P or Ctrl+F

                WV2_Login.Focus();



                // Check when the user has finished logging in
                WV2_Login.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            }
            catch (Exception ex)
            {
                Lbl_LoginStatus.ForeColor = Color.Firebrick;
                Lbl_LoginStatus.Text = ex.Message;

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                if (!e.IsSuccess) return;

                if (await IsLoginSuccessful())
                {

                    Lbl_LoginStatus.ForeColor = Color.ForestGreen;
                    Lbl_LoginStatus.Text = "You seems logged in, you can close this window now";
                }
            }
            catch (Exception ex)
            {
                Lbl_LoginStatus.ForeColor = Color.Firebrick;
                Lbl_LoginStatus.Text = ex.Message;

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task<bool> IsLoginSuccessful()
        {
            try
            {
                // 1. Javascript check (AccountID e SessionID)
                string script = @"
                                (function() {
                                    return {
                                        accountId: (typeof g_AccountID !== 'undefined') ? g_AccountID : 0,
                                        steamId: (typeof g_steamID !== 'undefined') ? g_steamID : '0',
                                        sessionId: (typeof g_sessionID !== 'undefined') ? g_sessionID : null
                                    };
                                })()";

                string jsonResult = await WV2_Login.CoreWebView2.ExecuteScriptAsync(script);
                if (string.IsNullOrWhiteSpace(jsonResult)) { return false; }

                SteamSessionInfo? session = JsonSerializer.Deserialize<SteamSessionInfo>(jsonResult);

                bool hasJsAccount = session != null && session.AccountId != 0;
                bool hasSession = session != null && !string.IsNullOrEmpty(session.SessionId);

                // 2. Cookie check (more reliable)
                CoreWebView2CookieManager cookieManager = WV2_Login.CoreWebView2.CookieManager;
                List<CoreWebView2Cookie> cookies = await cookieManager.GetCookiesAsync("https://store.steampowered.com");

                bool hasSecureCookie = cookies.Any(c => c.Name.Equals("steamLoginSecure", StringComparison.OrdinalIgnoreCase));

                // 3. Confirmation logic
                // Consider the user logged in if we have the secure cookie 
                // OR if we have a valid AccountID AND a session.
                if (hasSecureCookie || (hasJsAccount && hasSession))
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }

            return false;
        }

        // V1
        //private async Task<bool> IsUserLoggedIn()
        //{
        //    // Get all cookies for the Steam store domain
        //    var cookieManager = WV2_Login.CoreWebView2.CookieManager;
        //    var cookies = await cookieManager.GetCookiesAsync("https://store.steampowered.com");

        //    // Check if any cookie has the name "steamLoginSecure" (case-insensitive)
        //    return cookies.Any(c => c.Name.Equals("steamLoginSecure", StringComparison.OrdinalIgnoreCase));
        //}

        public class SteamSessionInfo
        {
            public long AccountId { get; set; } = 0;
            public string SteamId { get; set; } = string.Empty;
            public string SessionId { get; set; } = string.Empty;
        }

    }
}
