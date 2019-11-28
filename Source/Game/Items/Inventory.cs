//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class Inventory : INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Inventory()
        {
            GoldAmount = 0;
            Items = new ObservableCollection<Item>();
        }

        public Inventory(Inventory other)
        {
            GoldAmount = other.GoldAmount;
            Items = new ObservableCollection<Item>(other.Items);
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }

        public void DiscardItem(Item item)
        {
            if (item.junkStatus != JunkStatus.Favorite)
                RemoveItem(item);
        }

        public void JunkItem(int selection)
        {
            Item itemToJunk = Items[selection];

            if (itemToJunk.junkStatus != JunkStatus.Junk)
            {
                itemToJunk.junkStatus = JunkStatus.Junk;
            }
            else
            {
                itemToJunk.junkStatus = JunkStatus.None;
            }

            // HACK to force UI update
            Items.Remove(itemToJunk);
            Items.Insert(selection, itemToJunk);
        }

        public void KeepItem(int selection)
        {
            Item itemToFavorite = Items[selection];

            if (itemToFavorite.junkStatus != JunkStatus.Favorite)
            {
                itemToFavorite.junkStatus = JunkStatus.Favorite;
            }
            else
            {
                itemToFavorite.junkStatus = JunkStatus.None;
            }

            // HACK to force UI update
            Items.Remove(itemToFavorite);
            Items.Insert(selection, itemToFavorite);
        }

        public uint PotionsHeld 
        { 
            get => potionsHeld; 
            set
            {
                if (potionsHeld != value)
                {
                    potionsHeld = value;
                    OnPropertyChange("PotionsHeld");
                }
            }
        }

        public uint GoldAmount 
        {
            get => goldAmount;
            set
            {
                if (goldAmount != value)
                {
                    goldAmount = value;
                    OnPropertyChange("GoldAmount");
                }
            }
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Item> Items { get; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private uint goldAmount;
        private uint potionsHeld;
    }
}
