using AFKards.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AFKards
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // --- RIDIMENSIONAMENTO E ICONA FINESTRA ---
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);

            // CORREZIONE QUI: Usiamo AppWindow direttamente (grazie a using Microsoft.UI.Windowing;)
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            // Impostiamo la grandezza a 1450x900
            appWindow.Resize(new Windows.Graphics.SizeInt32 { Width = 1450, Height = 900 });
            // ----------------------------------

            // Il file deve avere nelle proprietà "Azione di compilazione" = "Contenuto".
            appWindow.SetIcon("Assets\\AFKards icon master.ico");

            // Impostiamo la pagina di partenza all'avvio
            MainNav.SelectedItem = NavFarm;
            ContentFrame.Navigate(typeof(HomePage));
        }

        private void MainNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // Se l'utente clicca sulla rotellina degli ingranaggi nativa in basso
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
                return;
            }

            // Altrimenti leggiamo il Tag della voce cliccata
            if (args.InvokedItemContainer?.Tag is string tag)
            {
                switch (tag)
                {
                    case "Farm":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "About":
                        ContentFrame.Navigate(typeof(AboutPage));
                        break;
                }
            }
        }
    }
}
