//------------------------------------------------------------------------------
//
// File Name:	MonsterManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using System;
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
            heroManager = EngineCore.GetModule<HeroManager>();
            zoneManager = EngineCore.GetModule<WorldZoneManager>();

            // Register for events
            AddEventHandler(GameEvents.HeroAttack, OnHeroAttack);
            AddEventHandler(GameEvents.PlayerFlee, OnPlayerFlee);
            AddEventHandler(GameEvents.HeroDead, OnHeroDead);
            AddEventHandler(GameEvents.PlayerDefend, OnPlayerDefend);
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
                RaiseGameEvent(GameEvents.MonsterAttack, Monster, Monster.GetAttackDamage());
            }
            else
            {
                RaiseGameEvent(GameEvents.MonsterDead, Monster);
            }
        }

        private void OnPlayerFlee(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.AddWorldEventText, this,
               "You attempt to flee from the " + Monster.Race + "...");

            bool fleeSuccess = random.NextDouble() <= 0.6f;
            if (fleeSuccess)
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "You have successfully escaped from " + Monster.Name + ".");
                DestroyAllMonsters();
                RaiseGameEvent(GameEvents.PlayerLook);
            }
            else
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this, "You can't seem to find an opening" +
                    " to escape! You are locked in combat with " + Monster.Name + ".");

                if (!Monster.IsDead)
                {
                    RaiseGameEvent(GameEvents.MonsterAttack, Monster, Monster.GetAttackDamage());
                }

                RaiseGameEvent(GameEvents.AdvanceTime);
            }
        }

        private void OnPlayerDefend(object sender, GameEventArgs e)
        {
            // TO DO: Add additive bonus dodge chance to hero, mult bonus to block chance

            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "You steel yourself, waiting for your enemy to attack.");

            if (!Monster.IsDead)
            {
                RaiseGameEvent(GameEvents.MonsterAttack, Monster, Monster.GetAttackDamage());
            }

            // TO DO: Remove bonus dodge chance, block chance

            RaiseGameEvent(GameEvents.AdvanceTime);
        }

        private void OnHeroDead(object sender, GameEventArgs e)
        {
            DestroyAllMonsters();
        }

        private void DestroyAllMonsters()
        {
            Monster.Kill();
            Monster = new Monster();
            RaiseGameEvent(GameEvents.MonsterDead, Monster);
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private MonsterFactory monsterFactory = new MonsterFactory();

        // Modules
        HeroManager heroManager;
        WorldZoneManager zoneManager;

        // Internal data
        private Random random = new Random();
    }
}
