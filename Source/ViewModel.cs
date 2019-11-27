//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;
using DiabloSimulator.Game;
using DiabloSimulator.Game.Factories;

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
            hero = new Hero("The Vagabond");
            monster = new Monster();
            monsterFactory = new MonsterFactory();
            heroFactory = new HeroFactory();
        }

        public bool InCombat()
        {
            return !Hero.IsDead() && !Monster.IsDead();
        }

        #region heroFunctions

        public Hero Hero { get => hero; }

        public void CreateHero()
        {
            hero = heroFactory.Create(Hero);
        }

        #endregion

        #region monsterFunctions

        public Monster Monster { get => monster; }

        // Returns a message describing
        // the monster's entrance.
        public string CreateMonster()
        {
            monster = monsterFactory.Create(Hero);
            return Monster.Name + " (a level " 
                + Monster.Stats.Level + " " + Monster.Archetype + ") appeared!";
        }

        public void DestroyMonster()
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

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private MonsterFactory monsterFactory;
        private HeroFactory heroFactory;
        private Monster monster;
        private Hero hero;
    }
}
