using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AFKards;

public class Program
{

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    // Costanti per lo stile del messaggio
    const uint MB_OK = 0x00000000;
    const uint MB_ICONERROR = 0x00000010;


    [STAThread]
    static void Main(string[] Args)
    {
        // 1. CONTROLLO DIPENDENZE (Robust Check)
        if (!VerifyDependencies()) return;

        // 2. MUTEX PER CARTELLA (Single Instance per Directory)
        using (var mutex = CreateFolderSpecificMutex())
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                // Un'istanza di AFKards è già attiva in questa cartella
                return;
            }

            // 3. AVVIO NATIVO WINUI 3
            Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
                new App();
            });
        }
    }

    private static bool VerifyDependencies()
    {
        try
        {
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

            // Lista dei file critici
            var RequiredFiles = new List<string>
        {
            "AFKardsMini.exe",
            "Deck.dll",
            "Facepunch.Steamworks.Win64.dll",
            "steam_api.dll",
            "steam_api64.dll"
        };

            foreach (var FileR in RequiredFiles)
            {
                FileInfo FI = new(Path.Combine(BaseDir, FileR));

                if (!FI.Exists)
                {
                    _ = MessageBox(IntPtr.Zero, $"Error: The file '{FileR}' is missing.\nThe application will be closed.", "AFKards", MB_OK | MB_ICONERROR);
                    return false;
                }

                // Il file è vuoto? (Corruzione)
                if (FI.Length == 0) return false;
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static Mutex CreateFolderSpecificMutex()
    {
        // Otteniamo il percorso della cartella e ne creiamo un Hash unico
        string path = AppDomain.CurrentDomain.BaseDirectory.ToLower().TrimEnd('\\');
        byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(path));
        string folderId = Convert.ToHexString(hashBytes);

        // Il nome del Mutex sarà unico per questa specifica cartella
        return new Mutex(true, $"Global\\AFKards_{folderId}");
    }
}
