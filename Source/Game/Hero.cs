//------------------------------------------------------------------------------
//
// File Name:	Hero.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

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
            random = new Random();

            InitializeStats();
        }

        public string Heal(float amount)
        {
            // Increase health, but keep below max
            stats["CurrentHealth"] = Math.Min(stats.BaseValues["CurrentHealth"] + amount, 0);

            return amount.ToString();
        }

        public string Damage(List<DamageArgs> damageList)
        {
            string result = "";

            // TO DO: Calculate actual damage based on damage, resist

            // Decrease health, but keep above 0
            foreach (DamageArgs damage in damageList)
            {
                stats["CurrentHealth"] = Math.Max(stats.BaseValues["CurrentHealth"] - damage.amount,
                    -stats.ModifiedValues["MaxHealth"]);

                result += Name + " took " + damage.amount;
                if (damage.damageType != DamageType.Physical)
                    result += " " + damage.damageType.ToString();
                result += " damage./n";
            }

            return result;
        }

        public List<DamageArgs> GetAttackDamage()
        {
            var damageList = new List<DamageArgs>();

            int minValue = (int)stats.ModifiedValues["MinDamage"];
            int maxValue = (int)stats.ModifiedValues["MaxDamage"];

            damageList.Add(new DamageArgs(random.Next(minValue, maxValue + 1)));
            return damageList;
        }

        public void Kill()
        {
            // Reduce health
            stats["CurrentHealth"] = -stats.ModifiedValues["MaxHealth"];

            // Lose gold
            inventory.goldAmount = 0;

            // TO DO: Remove temp buffs

        }

        public void Revive()
        {
            stats["CurrentHealth"] = 0;
        }

        public bool IsDead()
        {
            return stats.ModifiedValues["CurrentHealth"] == 0;
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
            // Damage
            stats["MinDamage"] = 2;
            stats["MaxDamage"] = 3;
            stats["CriticalHitChance"] = 0.05f;
            stats["CriticalHitDamage"] = 1.5f;

            // Class-specific stats
            switch (Archetype)
            {
                case "Warrior":
                    // Set starting stats
                    stats["Strength"] = 10;
                    stats["Dexterity"] = 8;
                    stats["Intelligence"] = 8;
                    stats["Vitality"] = 9;

                    // Set stat progression
                    stats.SetProgression("Strength", 3);
                    stats.SetProgression("Dexterity", 1);
                    stats.SetProgression("Intelligence", 1);
                    stats.SetProgression("Vitality", 2);

                    // Add modifiers
                    stats.AddModifier(new StatModifier("MinDamage", "Strength",
                        ModifierType.Multiplicative, 0.01f, stats));
                    stats.AddModifier(new StatModifier("MaxDamage", "Strength",
                        ModifierType.Multiplicative, 0.01f, stats));
                    break;

                case "Rogue":
                    // Set starting stats
                    stats["Strength"] = 8;
                    stats["Dexterity"] = 10;
                    stats["Intelligence"] = 8;
                    stats["Vitality"] = 9;

                    // Set stat progression
                    stats.SetProgression("Strength", 1);
                    stats.SetProgression("Dexterity", 3);
                    stats.SetProgression("Intelligence", 1);
                    stats.SetProgression("Vitality", 2);

                    // Add modifiers
                    stats.AddModifier(new StatModifier("MinDamage", "Dexterity",
                        ModifierType.Multiplicative, 0.01f, stats));
                    stats.AddModifier(new StatModifier("MaxDamage", "Dexterity",
                        ModifierType.Multiplicative, 0.01f, stats));
                    break;

                case "Sorcerer":
                    // Set starting stats
                    stats["Strength"] = 8;
                    stats["Dexterity"] = 8;
                    stats["Intelligence"] = 10;
                    stats["Vitality"] = 9;

                    // Set stat progression
                    stats.SetProgression("Strength", 1);
                    stats.SetProgression("Dexterity", 1);
                    stats.SetProgression("Intelligence", 3);
                    stats.SetProgression("Vitality", 2);

                    // Add modifiers
                    stats.AddModifier(new StatModifier("MinDamage", "Intelligence",
                        ModifierType.Multiplicative, 0.01f, stats));
                    stats.AddModifier(new StatModifier("MaxDamage", "Intelligence",
                        ModifierType.Multiplicative, 0.01f, stats));
                    break;
            }

            // Vitality gives health per point
            stats["MaxHealth"] = 0;
            stats.AddModifier(new StatModifier("MaxHealth", "Vitality",
                ModifierType.Additive, 3, stats));

            // Current/Max Health
            stats["CurrentHealth"] = 0;
            stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, stats));

            // Strength gives armor per point
            stats["Armor"] = 0;
            stats.AddModifier(new StatModifier("Armor", "Strength", 
                ModifierType.Additive, 1, stats));

            // Dexterity gives dodge chance per point
            stats["DodgeChance"] = 0;
            stats.AddModifier(new StatModifier("DodgeChance", "Dexterity",
                ModifierType.Additive, 0.001f, stats));

            // Intelligence gives resistances per point
            stats["FireResist"] = 0;
            stats["ColdResist"] = 0;
            stats["LightningResist"] = 0;
            stats["PoisionResist"] = 0;
            stats.AddModifier(new StatModifier("FireResist", "Intelligence",
                ModifierType.Additive, 0.1f, stats));
            stats.AddModifier(new StatModifier("ColdResist", "Intelligence",
                ModifierType.Additive, 0.1f, stats));
            stats.AddModifier(new StatModifier("LightningResist", "Intelligence",
                ModifierType.Additive, 0.1f, stats));
            stats.AddModifier(new StatModifier("PoisonResist", "Intelligence",
                ModifierType.Additive, 0.1f, stats));

            // Experience
            stats["Experience"] = 0;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}
