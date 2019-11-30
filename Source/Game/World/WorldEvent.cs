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

    public enum WorldEventType
    {
        MonsterEvent,
        ItemEvent,
        GoldEvent,
        PotionEvent,
        ZoneDiscoveryEvent,
        TextEvent,
        ChoiceEvent,
    }

    public class WorldEvent
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldEvent(string name_, WorldEventType type_, string eventText_)
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

        public WorldEventType EventType { get; set; } = WorldEventType.TextEvent;

        public List<string> EventData { get; set; } = new List<string>();

        // Only happens once per exploration
        public bool UniquePerSession { get; set; } = false;

        // Only happens once per hero
        public bool UniquePerHero { get; set; } = false;
    }
}
