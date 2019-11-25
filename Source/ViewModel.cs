//------------------------------------------------------------------------------
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

        public void HealHero(float amount)
        {
            hero.Heal(amount);
        }

        public void KillHero()
        {
            hero.Kill();
        }

        public void ReviveHero()
        {
            hero.Revive();
        }

        public void DamageHero(float amount)
        {
            hero.Damage(amount);
        }
        
        public float GetHeroAttackDamage()
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

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Structures:
        //------------------------------------------------------------------------------

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
    }
}
