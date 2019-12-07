//------------------------------------------------------------------------------
//
// File Name:	WorldEventManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using System.IO;

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

            // Initial game text (new or otherwise)
            NextEvent = "Welcome to the world of Sanctuary!";

            // Register for events
            AddEventHandler("SetNextWorldEvent", OnSetNextWorldEvent);
        }

        public string NextEvent
        {
            get 
            {
                string result = nextEvent.ToString();
                nextEvent.GetStringBuilder().Clear();
                return result;
            }
            set
            {
                nextEvent.WriteLine(value);
            }
        }

        public void ProcessWorldEvent(WorldEvent worldEvent)
        {
            nextEvent.WriteLine(worldEvent.EventText);

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

                    nextEvent.WriteLine(monsterManager.Monster.Name + ", a level "
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
                        nextEvent.WriteLine("You have discovered " + worldEvent.Name + "!");
                    }
                    else
                    {
                        nextEvent.WriteLine("You have found the entrance to " + worldEvent.Name + ".");
                    }

                    zoneManager.NextZoneName = worldEvent.Name;

                    nextEvent.WriteLine("Click " + GameManager.discoverChoiceText.Choice01Text
                        + " to explore this area. If you wish to return to the previous area, " +
                        "click " + GameManager.discoverChoiceText.Choice02Text + ".");
                    break;
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnSetNextWorldEvent(object sender, GameEventArgs e)
        {
            NextEvent = e.Get<string>();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private StringWriter nextEvent = new StringWriter();

        // Modules
        private ZoneManager zoneManager;
        private GameManager gameManager;
        private MonsterManager monsterManager;
        private HeroManager heroManager;
    }
}
