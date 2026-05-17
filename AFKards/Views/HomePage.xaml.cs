using AFKards.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AFKards.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    internal sealed partial class HomePage : Page
    {
        // L'ObservableCollection è fondamentale in WinUI: se aggiungi o rimuovi
        // elementi da questa lista, la GridView si aggiorna da sola in tempo reale!
        internal ObservableCollection<GameBadgeInfo> MockGamesList { get; set; } = new();

        internal HomePage()
        {
            InitializeComponent();

            // Creiamo i dati fittizi
            GenerateDummyData();

            // Colleghiamo la lista al controllo grafico GridView
            GvGames.ItemsSource = MockGamesList;
        }


        private void BtnRefresh_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // 1. Svuotiamo la lista esistente
            MockGamesList.Clear();

            // 2. Generiamo nuovi dati casuali
            GenerateDummyData();

            // L'interfaccia grafica si aggiornerà da sola in tempo reale!
        }




        private void GenerateDummyData()
        {
            Random rnd = new Random();

            // Nomi inventati per test
            string[] fakeTitles = { "Assassin's Creed", "Cyberpunk 2077", "Hollow Knight",
                                "The Witcher 3", "Dead Space", "Terraria", "Stardew Valley" };

            // Creiamo tra 5 e 25 schede casuali
            int amountOfGames = rnd.Next(5, 26);

            for (int i = 0; i < amountOfGames; i++)
            {
                var fakeGame = new GameBadgeInfo
                {
                    AppId = rnd.Next(100, 9999).ToString(),
                    Title = fakeTitles[rnd.Next(fakeTitles.Length)],
                    Drops = rnd.Next(1, 21) // Tra 1 e 20 carte rimanenti
                };

                MockGamesList.Add(fakeGame);
            }
        }
    }
}
