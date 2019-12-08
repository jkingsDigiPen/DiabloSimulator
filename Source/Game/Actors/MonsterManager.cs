//------------------------------------------------------------------------------
//
// File Name:	MonsterManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    class MonsterManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public override void Inintialize()
        {
            gameManager = EngineCore.GetModule<GameManager>();
            heroManager = EngineCore.GetModule<HeroManager>();
            zoneManager = EngineCore.GetModule<WorldZoneManager>();

            // Register for events
            AddEventHandler(GameEvents.HeroAttack, OnHeroAttack);
            AddEventHandler(GameEvents.PlayerFlee, OnPlayerFlee);
        }

        public void CreateMonster(string name = null)
        {
            // Create specific monster
            if(name != null)
            {
                Monster = monsterFactory.Create(name, heroManager.Hero);
            }
            // Create monster using monster table
            else 
            {
                Monster = zoneManager.CurrentZone.MonsterTable.GenerateObject(
                    heroManager.Hero, monsterFactory);
            }
        }

        public void DestroyMonster()
        {
            Monster.Kill();
            Monster = new Monster();
            gameManager.InCombat = false;
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Monster.Damage(damageList);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Monster Monster { get; set; } = new Monster();

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnHeroAttack(object sender, GameEventArgs e)
        {
            string damageDealtString = Monster.Damage(e.Get<List<DamageArgs>>());
            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "You attack the " + Monster.Race + ". " + damageDealtString);

            if (!Monster.IsDead)
            {
                var damageArgs = Monster.GetAttackDamage();
                RaiseGameEvent(GameEvents.MonsterAttack, Monster, damageArgs);
            }
            else
            {
                RaiseGameEvent(GameEvents.MonsterDead, Monster);
            }
        }

        private void OnPlayerFlee(object sender, GameEventArgs e)
        {

        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private MonsterFactory monsterFactory = new MonsterFactory();

        // Modules
        GameManager gameManager;
        HeroManager heroManager;
        WorldZoneManager zoneManager;
    }
}
