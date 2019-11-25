//------------------------------------------------------------------------------
//
// File Name:	Hero.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class Hero : GameObject, CombatActor
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

        public void Heal(float amount)
        {
            // Increase health, but keep below max
            stats["CurrentHealth"] = Math.Min(stats.BaseValues["CurrentHealth"] + amount, 0);
        }

        public void Damage(float amount)
        {
            // Decrease health, but keep above 0
            stats["CurrentHealth"] = Math.Max(stats.BaseValues["CurrentHealth"] - amount, 
                -stats.BaseValues["MaxHealth"]);
        }

        public float GetAttackDamage()
        {
            Random random = new Random();

            return random.Next((int)stats.ModifiedValues["MinDamage"], 
                (int)stats.ModifiedValues["MaxDamage"]);
        }

        public void Revive()
        {
            stats["CurrentHealth"] = 0;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Inventory inventory;
        public Equipment equipment;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void InitializeStats()
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
