//------------------------------------------------------------------------------
//
// File Name:	WorldEventManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using System.Collections.Generic;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class WorldEventManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public override void Inintialize()
        {
            zoneManager = EngineCore.GetModule<ZoneManager>();
            gameManager = EngineCore.GetModule<GameManager>();
            monsterManager = EngineCore.GetModule<MonsterManager>();
            heroManager = EngineCore.GetModule<HeroManager>();

            // Register for events
            AddEventHandler(GameEvents.WorldEventText, OnWorldEventText);

            // Initial game text (new or otherwise)
            RaiseGameEvent(GameEvents.WorldEventText, this, "Welcome to the world of Sanctuary!");
        }

        public List<string> WorldEventsText
        {
            get 
            {
                List<string> result = new List<string>(worldEventsText);
                worldEventsText.Clear();
                return result;
            }
        }

        public void ProcessWorldEvent(WorldEvent worldEvent)
        {
            RaiseGameEvent(GameEvents.WorldEventText, this, worldEvent.EventText);

            switch (worldEvent.EventType)
            {
                // Monster generation
                case WorldEventType.MonsterEvent:
                    gameManager.Turns = 0;
                    gameManager.InCombat = true;

                    // Assume random monster
                    string monsterName = null;
                    if (worldEvent.Name != "Wandering Monster")
                    {
                        // Get specific monster name
                        monsterName = worldEvent.EventData[0];
                    }
                    monsterManager.CreateMonster(monsterName);

                    RaiseGameEvent(GameEvents.WorldEventText, this, monsterManager.Monster.Name + ", a level "
                            + monsterManager.Monster.Stats.Level + " " 
                            + monsterManager.Monster.Race + ", appeared!");
                    break;
                default:
                    break;

                case WorldEventType.ZoneDiscoveryEvent:
                    gameManager.CurrentChoiceText = GameManager.discoverChoiceText;

                    if (!heroManager.Hero.DiscoveredZones.Contains(worldEvent.Name))
                    {
                        heroManager.Hero.DiscoveredZones.Add(worldEvent.Name);
                        RaiseGameEvent(GameEvents.WorldEventText, this, 
                            "You have discovered " + worldEvent.Name + "!");
                    }
                    else
                    {
                        RaiseGameEvent(GameEvents.WorldEventText, this, 
                            "You have found the entrance to " + worldEvent.Name + ".");
                    }

                    zoneManager.NextZoneName = worldEvent.Name;

                    RaiseGameEvent(GameEvents.WorldEventText, this, 
                        "Click " + GameManager.discoverChoiceText.Choice01Text
                        + " to explore this area. If you wish to return to the previous area, " +
                        "click " + GameManager.discoverChoiceText.Choice02Text + ".");
                    break;
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnWorldEventText(object sender, GameEventArgs e)
        {
            worldEventsText.Add(e.Get<string>());
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private List<string> worldEventsText = new List<string>();

        // Modules
        private ZoneManager zoneManager;
        private GameManager gameManager;
        private MonsterManager monsterManager;
        private HeroManager heroManager;
    }
}
