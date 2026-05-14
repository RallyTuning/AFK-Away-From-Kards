using System.Text.Json;

namespace AFKardsMini
{
    internal class Functions
    {
        internal static string GetHelpText()
        {
            return "AFK - Away From Kards Guide\r\n" +
            "--------------------------\r\n\r\n" +

            "# Game ID (Mandatory)\r\n" +
            "-i --id /id [AppID]\r\n\r\n" +
            "# Minutes before closing\r\n" +
            "-d --duration /duration [min]\r\n\r\n" +
            "# Change the window title\r\n" +
            "-t --title /title [text]\r\n\r\n" +
            "# Run in background\r\n" +
            "-s --silent /silent\r\n" +

            "\r\nExample:\r\n" +
            "SteamIdler.exe --id 730 --duration 60";
        }

        // reuse a single HttpClient instance for all calls
        static readonly HttpClient http = InitializeClient();
        static HttpClient InitializeClient()
        {
            HttpClient HC = new() { Timeout = TimeSpan.FromSeconds(60) };
            HC.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("GitHub_RallyTuning", "69"));
            return HC;
        }


        /// <summary>
        /// Downloads game details from the Steam Store API for the specified AppID. If the request fails or the response is invalid, an error image is generated and returned instead.
        /// </summary>
        /// <remarks>
        /// If the API request fails (e.g., due to network issues, invalid AppID, or unexpected response format), a fallback image containing an error message is generated and returned.
        /// The caller is responsible for disposing the returned SteamJson object if it contains valid data.
        /// </remarks>
        /// <param name="AppId">The Steam AppID of the game.</param>
        /// <returns>A SteamJson object containing the game's details.</returns>
        internal static async Task<SteamJson> GetGameDetails(uint AppId)
        {
            try
            {
                using HttpResponseMessage Risp = await http.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}");

                if (Risp.IsSuccessStatusCode)
                {
                    string StrRes2 = await Risp.Content.ReadAsStringAsync();

                    var TempDic = JsonSerializer.Deserialize<Dictionary<string, Object>>(StrRes2);
                    string SubJson = TempDic is null ? string.Empty : TempDic.FirstOrDefault().Value.ToString() ?? string.Empty;
                    SteamJson SteamGameJson = string.IsNullOrWhiteSpace(SubJson) ? new() : JsonSerializer.Deserialize<SteamJson>(SubJson) ?? new();

                    if (SteamGameJson is not null && SteamGameJson.Success && SteamGameJson.Data is not null)
                    {
                        return SteamGameJson;
                    }
                    else
                    {
                        throw new Exception("Success was true but data was null");
                    }
                }
                else
                {
                    throw new Exception("Success was false");
                }
            }
            catch (Exception)
            {
                await GenerateTextImage("// ERROR //\n\nDetails not found");
                return new SteamJson();
            }
        }


        /// <summary>
        /// Downloads a game header image from the specified URI or returns a generated error image if the download fails.
        /// </summary>
        /// <remarks>
        /// If the specified URI is invalid or the download fails, a fallback image containing an error message is returned. The caller is responsible for disposing the returned Image object.
        /// </remarks>
        /// <param name="HeaderUri">The URI of the game header image to download. Cannot be null, empty, or whitespace.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an Image object representing the downloaded game header, or a generated error image if the download fails.</returns>
        internal static async Task<Image> DownloadGameHeader(string HeaderUri)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(HeaderUri))
                    throw new Exception("Header is null");

                // Download the image from Steam's CDN and load it into a Bitmap
                using HttpClient http = new();
                http.Timeout = TimeSpan.FromSeconds(60);
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("GitHub_RallyTuning", "69"));
                byte[] bytes = await http.GetByteArrayAsync(HeaderUri);
                using MemoryStream ms = new(bytes);
                using Bitmap tmp = new(ms);
                return new Bitmap(tmp);
            }
            catch
            {
                return await GenerateTextImage("// ERROR //\n\nNo image");
            }
        }


        /// <summary>
        /// Downloads the icon for a Steam game specified by its application ID and returns it as an Icon object.
        /// </summary>
        /// <remarks>
        /// This method retrieves game details from the Steam Web API and attempts to download and convert the game's icon to an Icon object. If the icon cannot be retrieved or an error occurs during processing, the method returns null.
        /// The caller should check the return value before using the icon.
        /// </remarks>
        /// <param name="AppId">The unique application ID of the Steam game for which to download the icon. Cannot be null, empty, or whitespace.</param>
        /// <returns>An Icon object representing the game's icon if the download and conversion succeed; otherwise, null.</returns>
        internal static async Task<Icon?> DownloadGameIcon(string AppId)
        {
            if (string.IsNullOrWhiteSpace(AppId))
                return null;

            try
            {
                // Request JSON details
                using HttpResponseMessage resp = await http.GetAsync($"https://api.steampowered.com/ICommunityService/GetApps/v1/?appids%5B0%5D={AppId}");
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("Failed to fetch app details");

                string json = await resp.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);

                if (!doc.RootElement.TryGetProperty("response", out JsonElement responseElem) ||
                    !responseElem.TryGetProperty("apps", out JsonElement appsElem) ||
                    appsElem.ValueKind != JsonValueKind.Array ||
                    appsElem.GetArrayLength() == 0)
                {
                    throw new Exception("Invalid JSON structure or no apps found");
                }

                JsonElement firstApp = appsElem[0];
                if (!firstApp.TryGetProperty("icon", out JsonElement iconEl) || iconEl.ValueKind != JsonValueKind.String)
                    throw new NullReferenceException(nameof(iconEl));

                string iconId = iconEl.GetString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(iconId))
                    throw new NullReferenceException(nameof(iconId));

                // Download the image (same HttpClient can be reused across hosts)
                byte[] bytes = await http.GetByteArrayAsync($"https://shared.fastly.steamstatic.com/community_assets/images/apps/{AppId}/{iconId}.jpg");
                using MemoryStream ms = new(bytes);
                using Bitmap tmp = new(ms);

                // Convert to ICO (in-memory) and return as Bitmap
                using MemoryStream pngMs = new();
                tmp.Save(pngMs, System.Drawing.Imaging.ImageFormat.Png);
                byte[] pngBytes = pngMs.ToArray();

                using MemoryStream icoMs = new();
                using (var bw = new BinaryWriter(icoMs, System.Text.Encoding.UTF8, true))
                {
                    // ICONDIR
                    bw.Write((short)0);
                    bw.Write((short)1);
                    bw.Write((short)1);

                    // ICONDIRENTRY
                    bw.Write((byte)(tmp.Width >= 256 ? 0 : tmp.Width));
                    bw.Write((byte)(tmp.Height >= 256 ? 0 : tmp.Height));
                    bw.Write((byte)0);
                    bw.Write((byte)0);
                    bw.Write((short)0);
                    bw.Write((short)32);
                    bw.Write(pngBytes.Length);
                    bw.Write(6 + 16);

                    // image data (PNG)
                    bw.Write(pngBytes);
                }

                icoMs.Position = 0;
                return new(icoMs);
            }
            catch
            {
                // on any error, do not override app icon
                return null;
            }
        }


        /// <summary>
        /// Generates an image containing the specified text message, formatted with a predefined style.
        /// </summary>
        /// <remarks>
        /// The generated image uses a fixed size and style, with the message centered in bold Courier New font. This method is typically used to create a visual representation of error or status messages.
        /// </remarks>
        /// <param name="Message">The text message to display in the generated image. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an image displaying the specified message.</returns>
        internal static async Task<Image> GenerateTextImage(string Message)
        {
            try
            {
                // Generate a simple error image if the download fails (e.g., invalid AppID or network issues)
                Bitmap bmp = new(460, 215);
                using Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Black);
                StringFormat format = new()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(Message, new Font("Courier New", 20, FontStyle.Bold), Brushes.Fuchsia, new RectangleF(0, 0, bmp.Width, bmp.Height), format);
                return bmp;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
