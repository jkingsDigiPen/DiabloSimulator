//------------------------------------------------------------------------------
//
// File Name:	MonsterManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            // Register for events
            AddEventHandler(GameEvents.HeroAttack, OnHeroAttack);
            AddEventHandler(GameEvents.PlayerFlee, OnPlayerFlee);
            AddEventHandler(GameEvents.HeroDead, OnHeroDead);
            AddEventHandler(GameEvents.PlayerDefend, OnPlayerDefend);
            AddEventHandler(GameEvents.SetMonster, OnSetMonster);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Monster Monster 
        {
            get
            {
                if (MonsterList.Count != 0)
                {
                    return MonsterList[selectedMonsterIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        public ObservableCollection<Monster> MonsterList { get; private set; } = new ObservableCollection<Monster>();

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region eventHandlers
        private void OnHeroAttack(object sender, GameEventArgs e)
        {
            Monster monster = MonsterList[selectedMonsterIndex];

            string damageDealtString = monster.Damage(e.Get<List<DamageArgs>>());
            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "You attack the " + monster.Race + ". " + damageDealtString);

            ExecuteMonsterActions();
        }

        private void OnPlayerFlee(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "You attempt to flee from battle...");

            bool fleeSuccess = random.NextDouble() <= 0.6f;
            if (fleeSuccess)
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "You have successfully escaped from battle.");

                DestroyAllMonsters();
                RaiseGameEvent(GameEvents.PlayerLook);
            }
            else
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this, "You can't seem to find an opening" +
                    " to escape! You are locked in combat!");

                ExecuteMonsterActions();

                RaiseGameEvent(GameEvents.AdvanceTime);
            }
        }

        private void OnPlayerDefend(object sender, GameEventArgs e)
        {
            // TO DO: Add additive bonus dodge chance to hero, mult bonus to block chance

            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "You steel yourself, waiting for your enemy to attack.");

            ExecuteMonsterActions();

            // TO DO: Remove bonus dodge chance, block chance
            RaiseGameEvent(GameEvents.AdvanceTime);
        }

        private void OnHeroDead(object sender, GameEventArgs e)
        {
            DestroyAllMonsters();
        }

        private void OnSetMonster(object sender, GameEventArgs e)
        {
            Monster monster = e.Get<Monster>();
            MonsterList.Add(monster);
            RaiseGameEvent(GameEvents.AddWorldEventText, this, monster.Name + ", a level "
                    + monster.Stats.Level + " "
                    + monster.Race + ", appeared!");
        }

        #endregion

        private void DestroyAllMonsters()
        {
            foreach(Monster monster in MonsterList)
            {
                monster.Kill();
                RaiseGameEvent(GameEvents.MonsterDead, monster);
            }
            MonsterList.Clear();
            selectedMonsterIndex = 0;
        }

        private void ExecuteMonsterActions()
        {
            for(int i = 0; i < MonsterList.Count; )
            {
                Monster m = MonsterList[i];

                if (!m.IsDead)
                {
                    RaiseGameEvent(GameEvents.MonsterAttack, m, m.GetAttackDamage());
                    ++i;
                }
                else
                {
                    RaiseGameEvent(GameEvents.MonsterDead, m);

                    // Set selected monster index to alive monster
                    if (selectedMonsterIndex == MonsterList.IndexOf(m) && selectedMonsterIndex != 0)
                    {
                        --selectedMonsterIndex;
                    }

                    // Remove monster from list
                    MonsterList.Remove(m);
                }
            }
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Internal data
        private Random random = new Random();
        private int selectedMonsterIndex = 0;
    }
}
