//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    using EquipmentMap = ObservableDictionaryNoThrow<SlotType, Item>;

    public class Equipment
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Equipment()
        {
            equippedItems = new EquipmentMap();
        }

        public void EquipItem(Item item)
        {
            // Handle two-handed weapons
            if (item.slot == SlotType.BothHands)
            {
                equippedItems[SlotType.MainHand] = item;
                equippedItems[SlotType.OffHand] = item;
            }
            // Handle other slots
            else
            {
                equippedItems[item.slot] = item;
            }
        }

        public Item UnequipItem(SlotType slot)
        {
            Item removedItem = null;
            if (!equippedItems.TryGetValue(slot, out removedItem) 
                || removedItem is null || removedItem.Name == "NULL")
                return null;

            // DON'T actually remove - breaks bindings
            equippedItems[slot] = new Item();

            // Handle two-handed weapons
            if (removedItem.slot == SlotType.BothHands)
            {
                if (slot == SlotType.MainHand)
                {
                    equippedItems[SlotType.OffHand] = new Item();
                }
                else if(slot == SlotType.OffHand)
                {
                    equippedItems[SlotType.MainHand] = new Item();
                }
            }

            return removedItem;
        }

        public EquipmentMap Items
        {
            get => equippedItems;
        }


        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly EquipmentMap equippedItems;
    }
}
