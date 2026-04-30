namespace AFK_Away_From_Kards_Slave
{
    internal static class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            ApplicationConfiguration.Initialize();


            args = "--id 464920 --duration 1".Split(' '); // Clear args to prevent accidental use in MainForm





            var config = ParseArguments(args);

            if (config.IsSilent && config.AppId != 0)
            {
                // Modalità invisibile senza Form
                var silentContext = new SilentApplicationContext(config);
                Application.Run(silentContext);
            }
            else
            {
                // Avvio standard con interfaccia
                Application.Run(new MainForm(config));
            }
        }

        private static IdlerConfig ParseArguments(string[] args)
        {
            var config = new IdlerConfig();
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-i":
                    case "--id":
                    case "/id":
                        if (i + 1 < args.Length && uint.TryParse(args[++i], out uint id))
                            config.AppId = id;
                        break;

                    case "-d":
                    case "--duration":
                    case "/duration":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int dur))
                            config.DurationMin = dur;
                        break;

                    case "-t":
                    case "--title":
                    case "/title":
                        if (i + 1 < args.Length)
                            config.Title = args[++i];
                        break;

                    case "-s":
                    case "--silent":
                    case "/silent":
                        config.IsSilent = true;
                        break;

                    case "-h":
                    case "--help":
                    case "/help":
                        config.ShowHelp = true;
                        break;
                }
            }
            return config;
        }
    }

    public class IdlerConfig
    {
        public uint AppId { get; set; } = 0;
        public int DurationMin { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public bool IsSilent { get; set; } = false;
        public bool ShowHelp { get; set; } = false;
    }
}