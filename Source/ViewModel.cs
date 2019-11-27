//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Collections.Generic;
using DiabloSimulator.Game;
using DiabloSimulator.Game.Factories;
using System;

namespace DiabloSimulator
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class ViewModel
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public ViewModel()
        {
            Hero = new Hero("The Vagabond");
            Monster = new Monster();
            monsterFactory = new MonsterFactory();
            heroFactory = new HeroFactory();
        }

        public bool InCombat()
        {
            return !Hero.IsDead() && !Monster.IsDead();
        }

        #region heroFunctions

        public void CreateHero()
        {
            hero = heroFactory.Create(Hero);
        }

        public string DamageHero(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Hero.Damage(damageList);
        }

        #endregion

        #region monsterFunctions

        // Returns a message describing
        // the monster's entrance.
        public string GenerateMonster()
        {
            monster = monsterFactory.Create(Hero);
            return Monster.Name + " (a level " 
                + Monster.stats.Level + " " + Monster.Archetype + ") appeared!";
        }

        public void KillMonster()
        {
            Monster.Kill();
            monster = new Monster();
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Monster.Damage(damageList);
        }

        #endregion

        public Hero Hero { get; }
        public Monster Monster { get; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private MonsterFactory monsterFactory;
        private HeroFactory heroFactory;
        private Monster monster;
        private Hero hero;
    }
}
