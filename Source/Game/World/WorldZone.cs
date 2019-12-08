//------------------------------------------------------------------------------
//
// File Name:	WorldZone.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Game.RandomGeneration;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public enum WorldZoneType
    {
        Town,
        SafeZone,
        Outdoors,
        Dungeon,
    }

    public class WorldZone
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        [JsonConstructor]
        public WorldZone(string name_, WorldZoneType type_, string lookText_)
        {
            Name = name_;
            ZoneType = type_;
            LookText = lookText_;
            ConnectedZones = new List<string>();
            MonsterTable = new MonsterTable();
            EventTable = new WorldEventTable();
        }

        public WorldZone(WorldZone other)
        {
            Name = other.Name;
            ZoneType = other.ZoneType;
            LookText = other.LookText;
            ConnectedZones = new List<string>(other.ConnectedZones);
            MonsterTable = new MonsterTable(other.MonsterTable);
            EventTable = new WorldEventTable(other.EventTable);
            BackgroundTrackName = other.BackgroundTrackName;
            AmbientTrackName = other.AmbientTrackName;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public string Name { get; set; }

        // Text displayed when performing the 'Look' action in this zone
        public string LookText { get; set; }

        public List<string> ConnectedZones { get; set; }

        public MonsterTable MonsterTable { get; set; }

        public WorldZoneType ZoneType { get; set; }

        public WorldEventTable EventTable { get; set; }

        // Name of the event used for music in this level
        public string BackgroundTrackName { get; set; }

        // Name of the event used for ambience in this level
        public string AmbientTrackName { get; set; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------
    }
}
