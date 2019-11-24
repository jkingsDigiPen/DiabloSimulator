//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class Inventory
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Inventory()
        {
            goldAmount = 0;
            Items = new ObservableCollection<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void DiscardItem(ref Item item)
        {
            if (item.junkStatus != JunkStatus.Favorite)
                Items.Remove(item);
        }

        public ObservableCollection<Item> Items { get; }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public uint goldAmount;
        public uint potionsHeld;
    }
}
