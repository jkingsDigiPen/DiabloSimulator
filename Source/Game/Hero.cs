//------------------------------------------------------------------------------
//
// File Name:	Hero.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows.Controls;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class Hero : GameObject
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Hero(string name_ = "", string heroClass = "Warrior")
            : base(name_, heroClass)
        {
            inventory = new Inventory();
            equipment = new Equipment();

            InitializeStats();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Inventory inventory;
        public Equipment equipment;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        void InitializeStats()
        {
            // Vitality gives 10 health per point
            stats["MaxHealth"] = 0;
            stats.AddModifier(new StatModifier("MaxHealth", "Vitality",
                ModifierType.Additive, 10, stats));

            stats["CurrentHealth"] = 0;
            stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, stats));
        }
    }
}
