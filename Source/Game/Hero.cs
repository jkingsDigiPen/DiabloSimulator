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
            Inventory = new Inventory();
            Equipment = new Equipment();
            random = new Random();
        }

        public Hero(Hero other)
            : base(other)
        {
            Inventory = new Inventory(other.Inventory);
            Equipment = new Equipment(other.Equipment);
            random = new Random();
        }

        public string Heal(float amount)
        {
            // Increase health, but keep below max
            Stats["CurrentHealth"] = Math.Min(Stats.BaseValues["CurrentHealth"] + amount, 0);

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

                Stats["CurrentHealth"] = Math.Max(Stats.BaseValues["CurrentHealth"] - damage.amount,
                -Stats.ModifiedValues["MaxHealth"]);

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

        public string Damage(float damage)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(damage));
            return Damage(damageList);
        }

        public List<DamageArgs> GetAttackDamage()
        {
            var damageList = new List<DamageArgs>();

            int minValue = (int)Stats.ModifiedValues["MinDamage"];
            int maxValue = (int)Stats.ModifiedValues["MaxDamage"];

            damageList.Add(new DamageArgs(random.Next(minValue, maxValue + 1)));
            return damageList;
        }

        public void Kill()
        {
            // Reduce health
            Stats["CurrentHealth"] = -Stats.ModifiedValues["MaxHealth"];

            // Lose gold
            Inventory.GoldAmount = 0;

            // TO DO: Remove temp buffs

        }

        public void Revive()
        {
            Stats["CurrentHealth"] = 0;
        }

        public bool IsDead()
        {
            return Stats.ModifiedValues["CurrentHealth"] == 0;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Inventory Inventory { get; }
        public Equipment Equipment { get; }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}
