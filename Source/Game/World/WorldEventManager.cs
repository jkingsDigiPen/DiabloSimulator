using DiabloSimulator.Engine;
using System;
using System.IO;

namespace DiabloSimulator.Game.World
{
    public class WorldEventManager : IModule
    {
        public WorldEventManager()
        {
            // Initial game text (new or otherwise)
            NextEvent = "Welcome to the world of Sanctuary!";
        }

        public void Inintialize()
        {
            throw new NotImplementedException();
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
                    turns = 0;
                    InCombat = true;

                    if (worldEvent.Name == "Wandering Monster")
                    {
                        // Get random monster name
                        monster = zone.MonsterTable.GenerateObject(hero, monsterFactory);
                    }
                    else
                    {
                        string monsterName = worldEvent.EventData[0];
                        monster = monsterFactory.Create(monsterName, hero);
                    }

                    nextEvent.WriteLine(Monster.Name + ", a level "
                            + Monster.Stats.Level + " " + Monster.Race + ", appeared!");
                    break;
                default:
                    break;

                case WorldEventType.ZoneDiscoveryEvent:
                    currentChoiceText = discoverChoiceText;

                    if (!hero.DiscoveredZones.Contains(worldEvent.Name))
                    {
                        hero.DiscoveredZones.Add(worldEvent.Name);
                        nextEvent.WriteLine("You have discovered " + worldEvent.Name + "!");
                    }
                    else
                    {
                        nextEvent.WriteLine("You have found the entrance to " + worldEvent.Name + ".");
                    }

                    nextZoneName = worldEvent.Name;

                    nextEvent.WriteLine("Click " + discoverChoiceText.Choice01Text
                        + " to explore this area. If you wish to return to the previous area, " +
                        "click " + discoverChoiceText.Choice02Text + ".");
                    break;
            }
        }

        private StringWriter nextEvent = new StringWriter();
    }
}
