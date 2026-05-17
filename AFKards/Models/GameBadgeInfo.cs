using System;
using System.Collections.Generic;
using System.Text;

namespace AFKards.Models
{
    internal class GameBadgeInfo
    {
        public string AppId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Drops { get; set; }

        public string ToolTipText => $"{Drops} cards remaining";


        // Trucco per la UI: Restituisce una lista di oggetti vuoti lunga quanto il numero di 'Drops'.
        // L'interfaccia (ItemsControl) la userà esclusivamente per disegnare N rettangolini.
        public List<int> CardRectangles
        {
            get
            {
                var list = new List<int>();
                for (int i = 0; i < Drops; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }
    }
}
