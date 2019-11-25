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
            // Vitality gives health per point
            stats["MaxHealth"] = 0;
            stats.AddModifier(new StatModifier("MaxHealth", "Vitality",
                ModifierType.Additive, 3, stats));

            // Current/Max Health
            stats["CurrentHealth"] = 0;
            stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, stats));

            // Damage
            stats["MinDamage"] = 2;
            stats["MaxDamage"] = 3;
            stats["CriticalHitChance"] = 0.05f;
            stats["CriticalHitDamage"] = 1.5f;
        }
    }
}
