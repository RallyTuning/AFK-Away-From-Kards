using System.Text.Json;
using static AFK_Away_From_Kards_Master.Functions;

namespace AFK_Away_From_Kards_Master
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

        }

        private Microsoft.Web.WebView2.WinForms.WebView2? WV2Hidden = null;
        private string? CachedUsername = null;
        private bool IsWV2HiddenBusy = false;

        private CancellationTokenSource? CtsBackground = null;
        private CancellationTokenSource? CtsScraping = null;

        List<GameBadgeInfo> AllGamesWithCards = [];
        ImageList ImgListGames = new()
        {
            ImageSize = new Size(171, 80)
        };

        private async Task InitializeHiddenBrowser()
        {
            try
            {
                WV2Hidden = new Microsoft.Web.WebView2.WinForms.WebView2();

                // Same user data folder to maintain session
                string userDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SteamSessionData");
                var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, userDataFolder);

                await WV2Hidden.EnsureCoreWebView2Async(env);

                // Ultra-light configuration to minimize resource usage
                WV2Hidden.CoreWebView2.IsMuted = true;

                //var settings = WV2Hidden.CoreWebView2.Settings;
                //settings.AreDefaultScriptDialogsEnabled = false;
                //settings.IsStatusBarEnabled = false;
                //settings.AreDevToolsEnabled = false;
                //settings.AreDefaultContextMenusEnabled = false;
                //settings.IsZoomControlEnabled = false;
                //settings.AreBrowserAcceleratorKeysEnabled = false;
                //settings.IsGeneralAutofillEnabled = false;
                //settings.IsPasswordAutosaveEnabled = false;
                //settings.IsPinchZoomEnabled = false;
                //settings.IsSwipeNavigationEnabled = false;

                WV2Hidden.Source = new Uri("https://steamcommunity.com/my/badges/?sort=a&l=english");

#if DEBUG
                // In debug i need to see the browser
                Form FrmTemp = new()
                {
                    Text = "Debug - WebView2",
                    Size = new Size(1200, 800),
                };
                FrmTemp.Controls.Add(WV2Hidden);
                WV2Hidden.Width = 800;
                WV2Hidden.Height = 600;
                WV2Hidden.Top = 0;
                WV2Hidden.Left = 0;
                WV2Hidden.Visible = true;
                WV2Hidden.Dock = DockStyle.Fill;
                WV2Hidden.BringToFront();

                FrmTemp.Show();
#else
    WV2Hidden.Visible = false;
#endif

                // Colleghiamo l'evento di gestione navigazione
                WV2Hidden.NavigationCompleted += OnNavigationCompleted;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> WaitForPageReady(int timeoutSeconds = 15)
        {
            try
            {
                if (WV2Hidden == null) return false;

                DateTime TimeStart = DateTime.Now;
                while ((DateTime.Now - TimeStart).TotalSeconds < timeoutSeconds)
                {
                    // 1. Controllo base del browser
                    string ReadyState = await Invoke(() => WV2Hidden.CoreWebView2.ExecuteScriptAsync("document.readyState"));

                    if (ReadyState == "\"complete\"")
                    {
                        // 2. Controllo specifico per Steam: cerchiamo almeno una riga delle medaglie
                        // Usiamo querySelector per vedere se esiste la classe 'badge_row'
                        string ElementCheck = await Invoke(() => WV2Hidden.CoreWebView2.ExecuteScriptAsync(
                            "document.querySelector('.badge_row') !== null"));

                        if (ElementCheck.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                            return true; // La pagina è pronta E ci sono i dati
                    }

                    await Task.Delay(500); // Aspetta mezzo secondo prima di riprovare
                }

                return false; // Timeout raggiunto
            }
            catch
            {
                return false;
            }
        }


        

        private async void OnNavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            //if (!e.IsSuccess) return;

            //bool isReady = await WaitForPageReady();

            //if (isReady)
            //{

            //}
        }

        // Modifica FetchUsernameFromSteam per supportare cancellazione e aggiornamento UI sicuro
        private Task RunOnUiThreadAsync(Action action)
        {
            if (action is null) return Task.CompletedTask;

            // Se il controllo è già stato smaltito o non creato, ignoriamo l'azione in modo sicuro
            if (this.IsDisposed || !this.IsHandleCreated) return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            try
            {
                if (this.InvokeRequired)
                {
                    // BeginInvoke non blocca il thread chiamante; completiamo il TCS quando l'azione termina
                    this.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            if (!this.IsDisposed) action();
                            tcs.SetResult(null);
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }
                    }));
                }
                else
                {
                    try
                    {
                        action();
                        tcs.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        }

        private async Task FetchUsernameFromSteam(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    if (WV2Hidden == null) return;

                    string result = await Invoke(() => WV2Hidden.CoreWebView2.ExecuteScriptAsync(ScriptGetUsername()));

                    if (!string.IsNullOrEmpty(result) && result != "null")
                    {
                        string name = result.Trim('"');

                        Invoke(() => Lbl_Logged.ForeColor = Color.ForestGreen);
                        Invoke(() => Lbl_Logged.Text = name);
                    }
                    else
                    {
                        Invoke(() => Lbl_Logged.ForeColor = Color.Firebrick);
                        Invoke(() => Lbl_Logged.Text = "NO");
                    }

                    await Task.Delay(3000, ct);
                }
            }
            catch (OperationCanceledException) {/* Nothing */ }
            catch (Exception)
            {
                try
                {
                    Invoke(() => Lbl_Logged.ForeColor = Color.Firebrick);
                    Invoke(() => Lbl_Logged.Text = "// Error //");
                }
                catch { /* Nothing */ }
            }
        }

        private static async Task CheckCardDropsTask()
        {
            try
            {

                //// Download game details
                //SteamJson IdleGameDetails = await GetGameDetails(TheConfig.AppId);

                //if (IdleGameDetails is null)
                //{
                //    MessageBox.Show($"Can't fetch details for AppID {TheConfig.AppId}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Close();
                //    return;
                //}

                //// Update UI with game details
                //string AppName = IdleGameDetails.Data.Name;

                //Image AppHeader = await DownloadGameHeader(IdleGameDetails.Data.HeaderImage);

            }
            catch (Exception)
            {

                throw;
            }
        }



        // Classe per deserializzare i dati dello scraping
        public class GameBadgeInfo
        {
            public string AppId { get; set; } = "";
            public string Title { get; set; } = "";
            public int Drops { get; set; }
        }





        private async Task AddGameToListView(GameBadgeInfo game)
        {
            try
            {
                // 1. Download e Ridimensionamento Immagine
                string imageUrl = $"https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/{game.AppId}/header.jpg";
                using var http = new HttpClient();
                var bytes = await http.GetByteArrayAsync(imageUrl);

                using var ms = new MemoryStream(bytes);
                using var original = Image.FromStream(ms);

                ImageList imgListGames = new()
                {
                    ImageSize = new Size(171, 80)
                };

                // Creiamo la miniatura proporzionata (171x80)
                Bitmap thumb = new(171, 80);
                using (Graphics g = Graphics.FromImage(thumb))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(original, 0, 0, 171, 80);
                }

                // 2. Aggiunta all'ImageList della ListView
                // Assicurati che listView1.LargeImageList sia settata a 171x80
                imgListGames.Images.Add(game.AppId, thumb);

                // 3. Creazione Item
                ListViewItem item = new(game.Title)
                {
                    ImageKey = game.AppId // Collega l'immagine tramite ID
                };
                item.SubItems.Add($"{game.Drops} drops remaining"); // SubItem 1: Carte
                item.SubItems.Add(game.AppId); // SubItem 2: ID (Colonna nascosta)

                // Aggiungiamo alla ListView (usa Invoke se chiami da un Task)
                this.Invoke(() => LsvGames.Items.Add(item));
            }
            catch { /* Gestione errore immagine mancante */ }
        }




        public event Action<List<GameBadgeInfo>>? OnGamesScraped;

        private async Task StartFullScraping()
        {
            if (WV2Hidden == null) return;

#warning Cts da implementare
            while (CtsScraping == null)
            {
                while (!await WaitForPageReady()) { /* aspetta che la pagina sia pronta prima di iniziare */ }

                // Creiamo una lista TEMPORANEA per questo ciclo di scansione
                List<GameBadgeInfo> currentScanResults = new();

                // 1. Esecuzione script
                string pagesJson = await Invoke(() => WV2Hidden.CoreWebView2.ExecuteScriptAsync(ScriptGetPages()));

                // 2. Parsing (WebView2 restituisce "null" come stringa se il risultato è nullo)
                List<string> pageUrls = (pagesJson == "null")
                    ? []
                    : JsonSerializer.Deserialize<List<string>>(pagesJson) ?? [];

                // Inseriamo anche la pagina attuale (la 1) nella lista se non c'è
                string firstPage = Invoke(() => WV2Hidden.Source.ToString());
                if (!pageUrls.Contains(firstPage)) pageUrls.Insert(0, firstPage);

                // 3. Ciclo di navigazione su ogni pagina

                foreach (string url in pageUrls)
                {
                    if (Invoke(() => WV2Hidden.Source.ToString()) != url)
                    {
                        Invoke(() => WV2Hidden.CoreWebView2.Navigate(url));
                        if (!await WaitForPageReady()) continue;
                    }

                    // Estrai i giochi filtrati da questa pagina
                    string gamesJson = await Invoke(() => WV2Hidden.CoreWebView2.ExecuteScriptAsync(ScriptScrapeBadges()));


                    List<GameBadgeInfo> pageGames = (gamesJson == "null")
                    ? []
                    : JsonSerializer.Deserialize<List<GameBadgeInfo>>(gamesJson) ?? [];


                    if (pageGames != null && pageGames.Count > 0)
                    {
                        currentScanResults.AddRange(pageGames);
                    }

                }

                // SCELTA PRO: Invece di pulire la variabile globale, lanciamo l'evento
                // Passiamo i risultati della scansione appena completata
                OnGamesScraped?.Invoke(currentScanResults);

                await Task.Delay(TimeSpan.FromSeconds(240)); // Attendi 4 minuti prima di rifare lo scraping
            }
        }

        private void SyncListView(List<GameBadgeInfo> newData)
        {
            // Blocca il ridisegno per evitare sfarfallii
            LsvGames.BeginUpdate();

            try
            {
                // --- 1. RIMOZIONE ---
                // Controlliamo quali giochi non sono più presenti nel nuovo scraping
                for (int i = LsvGames.Items.Count - 1; i >= 0; i--)
                {
                    var item = LsvGames.Items[i];
                    // Se l'AppId dell'item non esiste più nei nuovi dati, lo togliamo
                    if (!newData.Any(g => g.AppId == item.Name))
                    {
                        LsvGames.Items.RemoveAt(i);
                    }
                }

                // --- 2. AGGIORNAMENTO E AGGIUNTA ---
                foreach (var game in newData)
                {
                    // Cerchiamo l'item usando l'AppId come chiave (Name)
                    var existingItem = LsvGames.Items[game.AppId];

                    if (existingItem != null)
                    {
                        // AGGIORNA: Se il gioco esiste, aggiorniamo solo i Drop se sono cambiati
                        string dropText = $"{game.Drops} drops remaining";
                        if (existingItem.SubItems[1].Text != dropText)
                        {
                            existingItem.SubItems[1].Text = dropText;
                        }
                    }
                    else
                    {
                        // AGGIUNGI: Il gioco è nuovo
                        ListViewItem newItem = new ListViewItem(game.Title);
                        newItem.Name = game.AppId; // FONDAMENTALE: l'ID per i futuri controlli
                        newItem.SubItems.Add($"{game.Drops} drops remaining");
                        newItem.SubItems.Add(game.AppId); // Colonna nascosta per sicurezza
                        newItem.ImageKey = "loading"; // Placeholder

                        LsvGames.Items.Add(newItem);

                        // Avvia il download dell'immagine solo per questo nuovo arrivato
                        _ = Task.Run(() => DownloadAndAssignImage(game.AppId, newItem));
                    }
                }
            }
            finally
            {
                LsvGames.EndUpdate();
            }
        }


        private async Task DownloadAndAssignImage(string appId, ListViewItem item)
        {
            try
            {
                // 1. Scarichiamo l'immagine usando il tuo metodo esistente
                // Costruiamo l'URI standard di Steam
                string headerUri = $"https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/{appId}/header.jpg";
                Image originalImg = await DownloadGameHeader(headerUri);

                // 2. Ridimensionamento proporzionato (171x80 come calcolato)
                // Creiamo una bitmap della dimensione esatta per la nostra ImageList
                Bitmap resizedImg = new Bitmap(171, 80);
                using (Graphics g = Graphics.FromImage(resizedImg))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalImg, 0, 0, 171, 80);
                }

                // Liberiamo la memoria dell'immagine originale se non è quella di errore (1x1)
                if (originalImg.Width > 1) originalImg.Dispose();

                // 3. Aggiornamento UI (ImageList e Item)
                this.Invoke(() =>
                {
                    // Se per caso l'immagine è già stata aggiunta da un altro thread, la sovrascriviamo
                    if (ImgListGames.Images.ContainsKey(appId))
                        ImgListGames.Images.RemoveByKey(appId);

                    ImgListGames.Images.Add(appId, resizedImg);

                    // Colleghiamo l'item all'immagine appena inserita
                    item.ImageKey = appId;
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore immagine {appId}: {ex.Message}");
            }
        }




        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "AFK: Away From Kards";
                this.Icon = Properties.Resources.Icona;

                await InitializeHiddenBrowser();

                // Crea il token se non esiste
                CtsBackground ??= new System.Threading.CancellationTokenSource();

                // Avvia i task in modo indipendente e non in attesa (detached)
                var fetchTask = Task.Run(() => FetchUsernameFromSteam(CtsBackground.Token));
                var scrapeTask = Task.Run(() => StartFullScraping());

                // Osserva eccezioni per log/visualizzazione senza bloccare il flusso principale
                _ = fetchTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        var ex = t.Exception?.GetBaseException();
                        if (ex != null) this.Invoke(() => MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                    }
                }, TaskScheduler.Default);

                _ = scrapeTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        var ex = t.Exception?.GetBaseException();
                        if (ex != null) this.Invoke(() => MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                    }
                }, TaskScheduler.Default);



                // Ci iscriviamo all'evento
                this.OnGamesScraped += (Results) =>
                {
                    // Poiché l'evento arriva da un Task, usiamo Invoke per tornare sulla UI
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(() => SyncListView(Results));
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsBtn_LoginForm_Click(object sender, EventArgs e)
        {
            try
            {
                using FormLogin FL = new();
                FL.ShowDialog();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
