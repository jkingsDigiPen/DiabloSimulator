//------------------------------------------------------------------------------
//
// File Name:	WorldEvent.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class WorldEvent
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldEvent(string name_, GameEvents type_, string eventText_)
        {
            Name = name_;
            EventType = type_;
            EventText = eventText_;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public string Name { get; set; } = null;

        public string PreviousEventName { get; set; } = null;

        public string NextEventName { get; set; } = null;

        public string EventText { get; set; } = null;

        public GameEvents EventType { get; set; } = GameEvents.WorldText;

        public List<string> EventData { get; set; } = new List<string>();

        // Only happens once per exploration
        public bool UniquePerSession { get; set; } = false;

        // Only happens once per hero
        public bool UniquePerHero { get; set; } = false;

        // Hero reference - for convenience
        public Hero Hero { get; set; } = null;
    }
}
