//------------------------------------------------------------------------------
//
// File Name:	Item.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public enum ItemRarity
    {
       Common,
       Magic,
       Rare,
       Legendary
    }

    public enum SlotType
    {
        Weapon1H,
        Weapon2H,
        OffHand,
        Head,
        Torso,
        Legs,
        Hands,
        Waist,
        Feet,
        Ring,
        Amulet,
    }

    public enum JunkStatus
    {
        None,
        Junk,
        Keep,
    }

    public class Item : GameObject
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Item(string name, SlotType slot_, ItemRarity rarity_ = ItemRarity.Common, 
            string baseItem = "") : base(name, baseItem == "" ? name : baseItem)
        {
            slot = slot_;
            rarity = rarity_;
            junkStatus = JunkStatus.None;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public SlotType slot;
        public ItemRarity rarity;
        public JunkStatus junkStatus;
    }
}
