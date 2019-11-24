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
            // Handle two-handed weapons
            if (item.slot == SlotType.BothHands)
            {
                equippedItems.Insert((int)SlotType.MainHand, item);
                equippedItems.Insert((int)SlotType.OffHand, item);
            }
            // Handle other slots
            else
            {
                equippedItems.Insert((int)item.slot, item);
            }
        }

        public List<Item> Items
        {
            get => equippedItems;
        }

        public Item UnequipItem(SlotType slot)
        {
            int index = (int)slot;

            if (index >= equippedItems.Count)
                return null;

            Item removedItem = equippedItems[index];
            equippedItems.RemoveAt(index);

            // Handle two-handed weapons
            if (removedItem.slot == SlotType.BothHands)
            {
                if (slot == SlotType.MainHand)
                {
                    equippedItems.RemoveAt((int)SlotType.OffHand);
                }
                else if(slot == SlotType.OffHand)
                {
                    equippedItems.RemoveAt((int)SlotType.MainHand);
                }
            }

            return removedItem;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        readonly private List<Item> equippedItems;
    }
}
