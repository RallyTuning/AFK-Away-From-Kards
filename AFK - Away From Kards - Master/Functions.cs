using System.Text.Json;

namespace AFK_Away_From_Kards_Master
{
    internal class Functions
    {
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
                await Task.CompletedTask;
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
                return new Bitmap(1, 1);
            }
        }




        // Script multi-livello per trovare il nome
        internal static string ScriptGetUsername()
        {
            return @"(function() {
                        // 1. Test the global variable (seems not to work anymore, but we try it anyway)
                        if (typeof g_rgPersonaData !== 'undefined' && g_rgPersonaData.personaname) 
                            return g_rgPersonaData.personaname;
            
                        // 2. Old fashion way to find the name in the DOM (works)
                        let el = document.querySelector('#account_pulldown') || document.querySelector('.persona_name');
                        if (el) return el.innerText.trim();

                        return null;
                    })()";
        }

        internal static string ScriptGetPages()
        {
            return @"(function() {
                        let links = [];
                        // Prendiamo tutti i link con classe 'pagelink'
                        document.querySelectorAll('.profile_paging .pagelink').forEach(a => {
                            if (a.href && !links.includes(a.href)) {
                                links.push(a.href);
                            }
                        });
                        return links;
                    })();";
        }

        internal static string ScriptScrapeBadges()
        {
            return @"(function() {
                        let results = [];
                        document.querySelectorAll('.badge_row').forEach(row => {
                            try {
                                // Filtro immediato: se non c'è il div dei drop o dice 'No card drops', saltiamo tutto
                                let dropsText = row.querySelector('.progress_info_bold')?.innerText.toLowerCase() || """";
                                if (!dropsText.includes('remaining') || dropsText.includes('no')) return;

                                let appIdMatch = row.querySelector('a.badge_row_overlay')?.href.match(/gamecards\/(\d+)\//);
                                if (!appIdMatch) return;

                                let title = row.querySelector('.badge_title')?.innerText.replace(/View details|Visualizza dettagli/i, '').trim();
                                let count = dropsText.match(/\d+/);

                                results.push({
                                    AppId: appIdMatch[1],
                                    Title: title || ""Unknown"",
                                    Drops: count ? parseInt(count[0]) : 0
                                });
                            } catch (e) {}
                        });
                        return results;
                    })();";
        }
    }
}
