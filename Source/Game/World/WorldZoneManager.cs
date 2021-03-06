﻿//------------------------------------------------------------------------------
//
// File Name:	ZoneManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using System;

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
            WorldEvent worldEvent = CurrentZone.EventTable.GenerateObject(e.Get<Hero>());
            RaiseGameEvent(worldEvent.EventType, this, worldEvent);
        }

        private void OnPlayerProceed(object sender, GameEventArgs e)
        {
            RaiseGameEvent(GameEvents.SetWorldZone, this, NextZoneName);
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
            Monster monster;

            // Create specific monster(s)
            if (worldEvent.Name != "Wandering Monster")
            {
                foreach(string name in worldEvent.EventData)
                {
                    monster = monsterFactory.Create(name, worldEvent.Hero);
                    RaiseGameEvent(GameEvents.SetMonster, this, monster);
                }
            }
            // Create monster using monster table
            else
            {
                // Select random number of monsters
                int groupSize = random.Next(minMonsterGroupSize, maxMonsterGroupSize + 1);
                for(int i = 0; i < groupSize; ++i)
                {
                    monster = CurrentZone.MonsterTable.GenerateObject(
                    worldEvent.Hero, monsterFactory);
                    RaiseGameEvent(GameEvents.SetMonster, this, monster);
                }
            }
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private WorldZoneFactory zoneFactory = new WorldZoneFactory();
        private MonsterFactory monsterFactory = new MonsterFactory();

        // Internal data
        private Random random = new Random();

        const int minMonsterGroupSize = 1;
        const int maxMonsterGroupSize = 3;
    }
}
