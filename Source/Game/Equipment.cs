//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class Equipment
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Equipment()
        {
            equippedItems = new List<Item>();
        }

        public void EquipItem(Item item)
        {
            equippedItems[(int)item.slot] = item;
        }

        public List<Item> Items
        {
            get => equippedItems;
        }

        public Item UnequipItem(SlotType slot)
        {
            Item removedItem = equippedItems[(int)slot];
            equippedItems.RemoveAt((int)slot);
            return removedItem;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        readonly private List<Item> equippedItems;
    }
}
