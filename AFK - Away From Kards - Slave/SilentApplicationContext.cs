namespace AFK_Away_From_Kards_Slave
{
    public class SilentApplicationContext : ApplicationContext
    {
        private readonly SteamIdlerCore? SilentIdlerCore;

        public SilentApplicationContext(IdlerConfig config)
        {
            try
            {
                SilentIdlerCore = new SteamIdlerCore(config);

                // Close the application when idling is finished
                SilentIdlerCore.OnFinished += (s, e) =>
                {
                    SilentIdlerCore.Dispose();
                    Application.Exit();
                };

                SilentIdlerCore.Start();
            }
            catch
            {
                Application.Exit();
            }
        }
    }
}
