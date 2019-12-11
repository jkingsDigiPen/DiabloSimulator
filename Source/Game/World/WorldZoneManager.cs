//------------------------------------------------------------------------------
//
// File Name:	ZoneManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class WorldZoneManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public override void Inintialize()
        {
            heroManager = EngineCore.GetModule<HeroManager>();

            AddEventHandler(GameEvents.PlayerLook, OnPlayerLook);
            AddEventHandler(GameEvents.PlayerExplore, OnPlayerExplore);
            AddEventHandler(GameEvents.PlayerProceed, OnPlayerProceed);
            AddEventHandler(GameEvents.PlayerBack, OnPlayerBack);
            AddEventHandler(GameEvents.SetWorldZone, OnSetWorldZone);
            AddEventHandler(GameEvents.PlayerTownPortal, OnPlayerTownPortal);
            AddEventHandler(GameEvents.WorldZoneDiscovery, OnWorldZoneDiscovery);
            AddEventHandler(GameEvents.WorldMonster, OnWorldMonster);
        }

        public void SetZone(string name)
        {
            // Only change zones if necessary
            if (CurrentZone != null && CurrentZone.Name == name)
                return;

            CurrentZone = zoneFactory.Create(name);

            RaiseGameEvent(GameEvents.SetAmbientTrack, this, CurrentZone.AmbientTrackName);
            RaiseGameEvent(GameEvents.SetBackgroundTrack, this, CurrentZone.BackgroundTrackName);

            // Make sure player sees new area
            RaiseGameEvent(GameEvents.PlayerLook);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public WorldZone CurrentZone { get; set; }
        public string NextZoneName { get; set; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPlayerLook(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.AddWorldEventText, this, CurrentZone.LookText);
        }

        private void OnPlayerExplore(object sender, GameEventArgs e)
        {
            WorldEvent worldEvent = CurrentZone.EventTable.GenerateObject(heroManager.Hero);
            RaiseGameEvent(worldEvent.EventType, this, worldEvent);
        }

        private void OnPlayerProceed(object sender, GameEventArgs e)
        {
            SetZone(NextZoneName);
        }

        private void OnPlayerBack(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.AddWorldEventText, this, "You step back from the entrance to "
                + NextZoneName + ", remaining in " + CurrentZone.Name + ".");
        }

        private void OnPlayerTownPortal(object sender, GameEventArgs e)
        {
            if (CurrentZone.ZoneType == WorldZoneType.Town)
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this,
                    "There is no need to cast 'Town Portal' at this time. " +
                    "You are already in town.");
            }
            else
            {
                RaiseGameEvent(GameEvents.AddWorldEventText, this, "You reach into your pack and take out " +
                    "a dusty blue tome containing the 'Town Portal' spell. You read the words aloud " +
                    "and suddenly a glowing, translucent portal opens up in the air in front of you. " +
                    "Stepping through it, you find yourself back in town.");
                SetZone("New Tristram");
            }
        }

        private void OnSetWorldZone(object sender, GameEventArgs e)
        {
            SetZone(e.Get<string>());
        }

        private void OnWorldZoneDiscovery(object sender, GameEventArgs e)
        {
            WorldEvent worldEvent = e.Get<WorldEvent>();
            NextZoneName = worldEvent.Name;
        }

        private void OnWorldMonster(object sender, GameEventArgs e)
        {
            WorldEvent worldEvent = e.Get<WorldEvent>();

            // Assume random monster
            string monsterName = null;
            if (worldEvent.Name != "Wandering Monster")
            {
                // Get specific monster name
                monsterName = worldEvent.EventData[0];
            }

            // Create specific monster
            Monster monster;
            if (monsterName != null)
            {
                monster = monsterFactory.Create(monsterName, heroManager.Hero);
            }
            // Create monster using monster table
            else
            {
                monster = CurrentZone.MonsterTable.GenerateObject(
                    heroManager.Hero, monsterFactory);
            }

            RaiseGameEvent(GameEvents.SetMonster, this, monster);
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private WorldZoneFactory zoneFactory = new WorldZoneFactory();
        private MonsterFactory monsterFactory = new MonsterFactory();


        // Modules
        private HeroManager heroManager;
    }
}
