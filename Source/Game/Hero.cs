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

    public class Hero : GameObject, ICombatActor
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
        }

        public Hero(Hero other)
            : base(other.Name, other.Archetype)
        {
            inventory = new Inventory(other.inventory);
            equipment = new Equipment(other.equipment);
            random = new Random();
            stats = new StatTable(other.stats);
        }

        public string Heal(float amount)
        {
            // Increase health, but keep below max
            stats["CurrentHealth"] = Math.Min(stats.BaseValues["CurrentHealth"] + amount, 0);

            return "You heal " + amount + " damage";
        }

        public string Damage(List<DamageArgs> damageList)
        {
            string result = "";

            // TO DO: Calculate actual damage based on damage, resist

            // Decrease health, but keep above 0
            int i = 0;
            foreach (DamageArgs damage in damageList)
            {
                if (i != 0)
                    result += "\n";
                ++i;

                stats["CurrentHealth"] = Math.Max(stats.BaseValues["CurrentHealth"] - damage.amount,
                -stats.ModifiedValues["MaxHealth"]);

                result += "You take " + damage.amount;
                if (damage.damageType != DamageType.Physical)
                    result += " " + damage.damageType.ToString();
                result += " damage.";
            }

            if (IsDead())
            {
                result += "\n";
                Kill();
                result += "You have been vanquished by the forces of evil!\n You have lost all of your gold.";
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
           
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}
