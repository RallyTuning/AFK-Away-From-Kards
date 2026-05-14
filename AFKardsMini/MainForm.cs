using static AFKardsMini.Functions;

namespace AFKardsMini
{
    public partial class MainForm : Form
    {

        private readonly IdlerConfig TheConfig;
        private SteamIdlerCore? IdlerCore;


        public MainForm(IdlerConfig config)
        {
            InitializeComponent();

            TxtHelp.Dock = DockStyle.Fill;
            TxtHelp.Text = GetHelpText();
            TheConfig = config;
        }


        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = $"AFKardsMini";
                this.Icon = Properties.Resources.Icona;

                PicApp.Image = await GenerateTextImage("// LOADING //\n\nDownload infos...");

                if (TheConfig.ShowHelp || TheConfig.AppId == 0)
                {
                    TxtHelp.Visible = true;
                    PicApp.Visible = false;
                    return;
                }
                else
                {
                    TxtHelp.Visible = false;
                    PicApp.Visible = true;
                }
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
                await StartIdling();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Call Dispose on the core engine to ensure proper cleanup of resources
                IdlerCore?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private async Task StartIdling()
        {
            try
            {
                if (TheConfig.AppId == 0) { return; }

                // Download game details
                SteamJson IdleGameDetails = await GetGameDetails(TheConfig.AppId);

                if (IdleGameDetails is null)
                {
                    MessageBox.Show($"Can't fetch details for AppID {TheConfig.AppId}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Update UI with game details
                this.Text += $" - {IdleGameDetails.Data.Name}";
                this.Icon = await DownloadGameIcon(IdleGameDetails.Data.SteamAppid.ToString());

                PicApp.Image = await DownloadGameHeader(IdleGameDetails.Data.HeaderImage);


                // Inizialize the core engine with the configuration
                IdlerCore = new SteamIdlerCore(TheConfig);

                // Subscribe to events for UI updates
                IdlerCore.OnTimeUpdated += (s, timeStr) => TsLbl_Countdown.Text = timeStr;
                IdlerCore.OnFinished += (s, e) => this.Close();

                // Start the idling process
                IdlerCore.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
