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
            AddEventHandler("AddWorldEvent", OnAddWorldEvent);

            // Initial game text (new or otherwise)
            RaiseGameEvent("AddWorldEvent", this, "Welcome to the world of Sanctuary!");
        }

        public List<string> WorldEvents
        {
            get 
            {
                List<string> result = new List<string>(worldEventStrings);
                worldEventStrings.Clear();
                return result;
            }
        }

        public void ProcessWorldEvent(WorldEvent worldEvent)
        {
            RaiseGameEvent("AddWorldEvent", this, worldEvent.EventText);

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

                    RaiseGameEvent("AddWorldEvent", this, monsterManager.Monster.Name + ", a level "
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
                        RaiseGameEvent("AddWorldEvent", this, 
                            "You have discovered " + worldEvent.Name + "!");
                    }
                    else
                    {
                        RaiseGameEvent("AddWorldEvent", this, 
                            "You have found the entrance to " + worldEvent.Name + ".");
                    }

                    zoneManager.NextZoneName = worldEvent.Name;

                    RaiseGameEvent("AddWorldEvent", this, 
                        "Click " + GameManager.discoverChoiceText.Choice01Text
                        + " to explore this area. If you wish to return to the previous area, " +
                        "click " + GameManager.discoverChoiceText.Choice02Text + ".");
                    break;
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnAddWorldEvent(object sender, GameEventArgs e)
        {
            worldEventStrings.Add(e.Get<string>());
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private List<string> worldEventStrings = new List<string>();

        // Modules
        private ZoneManager zoneManager;
        private GameManager gameManager;
        private MonsterManager monsterManager;
        private HeroManager heroManager;
    }
}
