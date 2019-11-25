﻿//------------------------------------------------------------------------------
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

    public class Monster : GameObject, CombatActor
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Monster(string name_, uint level_, string baseMonster_ = "", 
            MonsterRarity rarity_ = MonsterRarity.Common) 
            : base(name_, baseMonster_)
        {
            rarity = rarity_;
            stats.Level = level_;
        }

        public string Damage(List<DamageArgs> damageList)
        {
            string result = "";

            // TO DO: Calculate actual damage based on damage, resist

            // Decrease health, but keep above 0
            foreach(DamageArgs damage in damageList)
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

        public MonsterRarity rarity;

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}