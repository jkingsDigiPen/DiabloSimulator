//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows.Controls;

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
            items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void DiscardItem(ref Item item)
        {
            if(item.junkStatus != JunkStatus.Favorite)
                items.Remove(item);
        }

        public void Display(ListBox itemListView)
        {
            itemListView.Items.Clear();
            foreach(Item item in items)
            {
                string itemView = item.name + " (" + item.rarity + " " + item.archetype + ")";
                if(item.junkStatus != JunkStatus.None)
                {
                    itemView += ", marked as " + item.junkStatus;
                }

                itemListView.Items.Add(itemView);
            }
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public uint goldAmount;

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private List<Item> items;
    }
}
