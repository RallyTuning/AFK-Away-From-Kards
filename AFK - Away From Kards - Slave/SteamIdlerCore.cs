using Steamworks;

namespace AFK_Away_From_Kards_Slave
{
    internal class SteamIdlerCore : IDisposable
    {
        private readonly IdlerConfig TheConfig;
        private System.Windows.Forms.Timer? LoopTimer;
        private DateTime? EndTime;

        // Eventi per comunicare con l'interfaccia o il context
        internal event EventHandler<string>? OnTimeUpdated;
        internal event EventHandler? OnFinished;

        internal SteamIdlerCore(IdlerConfig config)
        {
            TheConfig = config;
        }

        internal void Start()
        {
            // Inizializza Steam
            SteamClient.Init(TheConfig.AppId);
            if (!SteamClient.IsValid)
                throw new Exception("Steam client not running or not owning a game license");

            // Imposta scadenza
            if (TheConfig.DurationMin > 0)
                EndTime = DateTime.Now.AddMinutes(TheConfig.DurationMin);

            // Timer fissato a 1000ms per permettere un countdown fluido
            LoopTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            LoopTimer.Tick += Timer_Tick;
            LoopTimer.Start();

            // Lancia un primo aggiornamento visivo immediato
            UpdateTimerDisplay();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Mantiene vivo Steam
            SteamClient.RunCallbacks();
            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            if (EndTime.HasValue)
            {
                TimeSpan remaining = EndTime.Value - DateTime.Now;

                if (remaining.TotalSeconds <= 0)
                {
                    LoopTimer?.Stop();
                    OnTimeUpdated?.Invoke(this, "00:00:00");
                    OnFinished?.Invoke(this, EventArgs.Empty); // Segnala che il tempo è scaduto
                }
                else
                {
                    OnTimeUpdated?.Invoke(this, remaining.ToString(@"hh\:mm\:ss"));
                }
            }
            else
            {
                OnTimeUpdated?.Invoke(this, "Endless");
            }
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        // Metodo obbligatorio per chiudere Steam in modo pulito
        internal void Dispose()
        {
            LoopTimer?.Stop();
            LoopTimer?.Dispose();

            if (SteamClient.IsValid)
                SteamClient.Shutdown();
        }
    }
}
