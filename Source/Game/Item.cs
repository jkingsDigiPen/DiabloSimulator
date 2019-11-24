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
        MainHand,
        OffHand,
        Head,
        Torso,
        Legs,
        Hands,
        Waist,
        Feet,
        Ring1,
        Ring2,
        Amulet,
        BothHands,
    }

    public enum JunkStatus
    {
        None,
        Junk,
        Favorite,
    }

    public class Item : GameObject
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Item()
            : base("NULL")
        {
        }

        public Item(string name, SlotType slot_, ItemRarity rarity_ = ItemRarity.Common, 
            string baseItem = "") : base(name, baseItem == "" ? name : baseItem)
        {
            slot = slot_;
            rarity = rarity_;
            junkStatus = JunkStatus.None;
        }

        public static bool operator==(Item lhs, Item rhs)
        {
            // Null items
            if (lhs is null && !(rhs is null))
                return false;

            if (!(lhs is null) && rhs is null)
                return false;

            if (lhs is null && rhs is null)
                return true;

            // Non-null items
            if (lhs.slot != rhs.slot)
                return false;

            if (lhs.archetype != rhs.archetype)
                return false;

            if (lhs.rarity != rhs.rarity)
                return false;

            if (lhs.Name != rhs.Name)
                return false;

            return true;
        }

        public static bool operator !=(Item lhs, Item rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            Item rhs = obj as Item;
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return (slot.ToString() + archetype + Name + rarity.ToString()).GetHashCode();
        }

        public override string ToString()
        {
            // Default/empty item - return empty string
            if (Name == "NULL")
                return "";

            string itemView = Name + " (" + rarity + " " + archetype + ")";
            if (junkStatus != JunkStatus.None)
            {
                itemView += ", marked as " + junkStatus;
            }

            return itemView;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public SlotType slot;
        public ItemRarity rarity;
        public JunkStatus junkStatus;
    }
}
