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
        Uncommon,
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
            Stats.Level = 0;
        }

        public Monster(string name_, int level_, MonsterRarity rarity_ = MonsterRarity.Common, 
            string race_ = "", string family_ = "", string baseMonster_ = "") 
            : base(name_, baseMonster_)
        {
            rarity = rarity_;
            Race = race_;
            family = family_;
            Stats.Level = level_;
            random = new Random();
        }

        public Monster(Monster other)
            : base(other)
        {
            rarity = other.Rarity;
            Race = other.Race;
            family = other.Family;
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
                    result += "\r\n";
                ++i;

                Stats["CurrentHealth"] = Math.Max(Stats.BaseValues["CurrentHealth"] - damage.amount,
                    -Stats.ModifiedValues["MaxHealth"]);

                result += Name + " takes " + damage.amount;
                if (damage.damageType != DamageType.Physical)
                    result += " " + damage.damageType.ToString();
                result += " damage.";
            }

            if (IsDead())
            {
                result += "\r\n";
                Kill();
                result += Name + " has been been slain!";
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

            // Physical
            int minValue = (int)Stats.ModifiedValues["MinDamage"];
            int maxValue = (int)Stats.ModifiedValues["MaxDamage"];
            if(minValue != 0 && maxValue != 0)
                damageList.Add(new DamageArgs(random.Next(minValue, maxValue + 1)));

            // Fire
            minValue = (int)Stats.ModifiedValues["MinFireDamage"];
            maxValue = (int)Stats.ModifiedValues["MaxFireDamage"];
            if(minValue != 0 && maxValue != 0)
                damageList.Add(new DamageArgs(
                    random.Next(minValue, maxValue + 1), DamageType.Fire));

            return damageList;
        }

        public string Heal(float amount)
        {
            // Increase health, but keep below max
            Stats["CurrentHealth"] = Math.Min(Stats.BaseValues["CurrentHealth"] + amount, 0);

            return amount.ToString();
        }

        public void Kill()
        {
            // Reduce health
            Stats["CurrentHealth"] = -Stats.ModifiedValues["MaxHealth"];
        }

        public void Revive()
        {
            Stats["CurrentHealth"] = 0;
        }

        public bool IsDead()
        {
            return Name == Monster.EmptyMonster || Stats.ModifiedValues["CurrentHealth"] == 0;
        }

        public float HealthPercent
        {
            get
            {
                float currentMonsterHealth = Stats.ModifiedValues["CurrentHealth"];
                float maxMonsterHealth = Stats.ModifiedValues["MaxHealth"];

                if (maxMonsterHealth == 0.0f)
                    return 0.0f;

                return currentMonsterHealth / maxMonsterHealth;
            }
        }
        public string Race
        {
            get => race;
            set
            {
                if (race != value)
                {
                    race = value;
                    base.OnPropertyChange("Race");
                }
            }
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public MonsterRarity Rarity { get => rarity; set => rarity = value; }

        public string Family { get => family; set => family = value; }

        public const string EmptyMonster = "No Monster Detected";

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
        private MonsterRarity rarity;
        private string race;
        private string family;
    }
}
