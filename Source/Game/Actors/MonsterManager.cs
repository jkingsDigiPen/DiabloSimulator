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
using System.ComponentModel;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    class MonsterManager : IModule, INotifyPropertyChanged
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
            AddEventHandler(GameEvents.MonsterSelected, OnMonsterSelected);
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

        public event PropertyChangedEventHandler PropertyChanged;

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

            // Force UI update for monster
            OnPropertyChange("Monster");
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
            OnPropertyChange("Monster");
        }

        private void OnMonsterSelected(object sender, GameEventArgs e)
        {
            selectedMonsterIndex = e.Get<int>();

            // Force UI update for monster
            OnPropertyChange("Monster");
        }

        #endregion

        private void DestroyAllMonsters()
        {
            int count = MonsterList.Count;

            foreach(Monster monster in MonsterList)
            {
                monster.Kill();
                RaiseGameEvent(GameEvents.MonsterDead, monster, count - 1);
                --count;
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
                    RaiseGameEvent(GameEvents.MonsterDead, m, MonsterList.Count - 1);

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

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Internal data
        private Random random = new Random();
        private int selectedMonsterIndex = 0;
    }
}
