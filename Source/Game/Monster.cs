//------------------------------------------------------------------------------
//
// File Name:	Monster.cs
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

    public enum MonsterRarity
    {
        Common,
        Elite,
        Legendary
    }

    public class Monster : GameObject, ICombatActor
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        #region constructors

        public Monster()
            : base("No Monster Detected")
        {
            rarity = MonsterRarity.Common;
            stats.Level = 0;
        }

        public Monster(string name_, uint level_, string baseMonster_ = "", 
            MonsterRarity rarity_ = MonsterRarity.Common) 
            : base(name_, baseMonster_)
        {
            rarity = rarity_;
            stats.Level = level_;
            random = new Random();
        }

        public Monster(Monster other)
            : base(other.Name, other.Archetype)
        {
            rarity = other.rarity;
            stats = new StatTable(other.stats);
            random = new Random();
        }

        #endregion

        public string Damage(List<DamageArgs> damageList)
        {
            string result = "";

            // TO DO: Calculate actual damage based on damage, resist

            // Decrease health, but keep above 0
            int i = 0;
            foreach(DamageArgs damage in damageList)
            {
                if (i != 0)
                    result += "\n";
                ++i;

                stats["CurrentHealth"] = Math.Max(stats.BaseValues["CurrentHealth"] - damage.amount,
                    -stats.ModifiedValues["MaxHealth"]);

                result += Name + " takes " + damage.amount;
                if (damage.damageType != DamageType.Physical)
                    result += " " + damage.damageType.ToString();
                result += " damage.";
            }

            if (IsDead())
            {
                result += "\n";
                Kill();
                result += Name + " has been been slain!";
            }

            return result;
        }

        public List<DamageArgs> GetAttackDamage()
        {
            var damageList = new List<DamageArgs>();

            // Physical
            int minValue = (int)stats.ModifiedValues["MinDamage"];
            int maxValue = (int)stats.ModifiedValues["MaxDamage"];
            if(minValue != 0 && maxValue != 0)
                damageList.Add(new DamageArgs(random.Next(minValue, maxValue + 1)));

            // Fire
            minValue = (int)stats.ModifiedValues["MinFireDamage"];
            maxValue = (int)stats.ModifiedValues["MaxFireDamage"];
            if(minValue != 0 && maxValue != 0)
                damageList.Add(new DamageArgs(
                    random.Next(minValue, maxValue + 1), DamageType.Fire));

            return damageList;
        }

        public string Heal(float amount)
        {
            // Increase health, but keep below max
            stats["CurrentHealth"] = Math.Min(stats.BaseValues["CurrentHealth"] + amount, 0);

            return amount.ToString();
        }

        public void Kill()
        {
            // Reduce health
            stats["CurrentHealth"] = -stats.ModifiedValues["MaxHealth"];
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

        public const string EmptyMonster = "No Monster Detected";

        public MonsterRarity rarity;

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}
