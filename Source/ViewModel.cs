﻿//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using DiabloSimulator.Game;

namespace DiabloSimulator
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class ViewModel : INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public ViewModel()
        {
            hero = new Hero("The Vagabond");
        }

        #region heroFunctions

        public string HealHero(float amount)
        {
            return hero.Heal(amount);
        }

        public void KillHero()
        {
            hero.Kill();
        }

        public void ReviveHero()
        {
            hero.Revive();
        }

        public string DamageHero(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return DamageHero(damageList);
        }

        public string DamageHero(List<DamageArgs> damageList)
        {
            return hero.Damage(damageList);
        }
        
        public List<DamageArgs> GetHeroAttackDamage()
        {
            return hero.GetAttackDamage();
        }

        #endregion

        #region heroProperties

        public string HeroName
        {
            get { return hero.Name; }
            set 
            { 
                if(hero.Name != value)
                {
                    hero.Name = value;
                    OnPropertyChange("HeroName");
                }
            }
        }

        public string HeroClass
        {
            get { return hero.Archetype;  }
            set
            {
                if(hero.Archetype != value)
                {
                    hero.Archetype = value;
                    OnPropertyChange("HeroClass");
                }
            }
        }

        public string HeroDescription
        {
            get { return hero.Description; }
            set
            {
                if (hero.Description != value)
                {
                    hero.Description = value;
                    OnPropertyChange("HeroDescription");
                }
            }
        }

        public StatTable HeroStats
        {
            get { return hero.stats; }
        }

        public Inventory HeroInventory
        {
            get { return hero.inventory; }
        }

        public Equipment HeroEquipment
        {
            get { return hero.equipment; }
        }

        public uint HeroPotions
        {
            get { return hero.inventory.potionsHeld; }
            set
            {
                if (value != hero.inventory.potionsHeld)
                {
                    hero.inventory.potionsHeld = value;
                    OnPropertyChange("HeroPotions");
                }
            }
        }

        #endregion

        #region monsterFunctions

        // Returns a message describing
        // the monster's entrance.
        public string GenerateMonster()
        {
            monster = new Monster("Fallen Imp", 1, "Fallen Imp");
            return "A monster appeared!";
        }

        public Inventory GetMonsterLoot()
        {
            return new Inventory();
        }

        public void KillMonster()
        {
            monster.Kill();
            monster = null;
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return DamageMonster(damageList);
        }

        public string DamageMonster(List<DamageArgs> damageList)
        {
            return monster.Damage(damageList);
        }

        public List<DamageArgs> GetMonsterAttackDamage()
        {
            return monster.GetAttackDamage();
        }

        #endregion

        #region monsterProperties

        public string MonsterName
        {
            get { return monster.Name; }
        }

        public string MonsterType
        {
            get { return monster.Archetype; }
        }

        public string MonsterDescription
        {
            get { return monster.Description; }
        }

        public StatTable MonsterStats
        {
            get { return monster.stats; }
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Hero hero;
        private Monster monster;
    }
}
