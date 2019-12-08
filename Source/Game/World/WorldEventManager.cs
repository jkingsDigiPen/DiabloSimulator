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
            zoneManager = EngineCore.GetModule<WorldZoneManager>();
            gameManager = EngineCore.GetModule<GameManager>();
            monsterManager = EngineCore.GetModule<MonsterManager>();
            heroManager = EngineCore.GetModule<HeroManager>();

            // Register for events
            AddEventHandler(GameEvents.AddWorldEventText, OnAddWorldEventText);
            AddEventHandler(GameEvents.WorldMonster, OnWorldMonster);
            AddEventHandler(GameEvents.WorldZoneDiscovery, OnWorldZoneDiscovery);

            // Initial game text (new or otherwise)
            RaiseGameEvent(GameEvents.AddWorldEventText, this, "Welcome to the world of Sanctuary!");
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

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnAddWorldEventText(object sender, GameEventArgs e)
        {
            worldEventsText.Add(e.Get<string>());
        }

        private void OnWorldMonster(object sender, GameEventArgs e)
        {
            WorldEvent worldEvent = e.Get<WorldEvent>();

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

            RaiseGameEvent(GameEvents.AddWorldEventText, this, monsterManager.Monster.Name + ", a level "
                    + monsterManager.Monster.Stats.Level + " "
                    + monsterManager.Monster.Race + ", appeared!");
        }

        private void OnWorldZoneDiscovery(object sender, GameEventArgs e)
        {
            WorldEvent worldEvent = e.Get<WorldEvent>();

            gameManager.CurrentChoiceText = GameManager.discoverChoiceText;

            if (!heroManager.Hero.DiscoveredZones.Contains(worldEvent.Name))
            {
                heroManager.Hero.DiscoveredZones.Add(worldEvent.Name);
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "You have discovered " + worldEvent.Name + "!");
            }
            else
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "You have found the entrance to " + worldEvent.Name + ".");
            }

            zoneManager.NextZoneName = worldEvent.Name;

            RaiseGameEvent(GameEvents.AddWorldEventText, this,
                "Click " + GameManager.discoverChoiceText.Choice01Text
                + " to explore this area. If you wish to return to the previous area, " +
                "click " + GameManager.discoverChoiceText.Choice02Text + ".");
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private List<string> worldEventsText = new List<string>();

        // Modules
        private WorldZoneManager zoneManager;
        private GameManager gameManager;
        private MonsterManager monsterManager;
        private HeroManager heroManager;
    }
}
