//------------------------------------------------------------------------------
//
// File Name:	WorldEventTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DiabloSimulator.Game.RandomGeneration
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class WorldEventTable : IRandomTable<WorldEvent>
    {
        public enum EventRarity
        {
            Common,
            Uncommon,
            Rare,
            Legendary,
        }

        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        [JsonConstructor]
        public WorldEventTable()
        {
            random = new Random();

            Weights = new Dictionary<EventRarity, double>();
            Weights[EventRarity.Common] = 0.67;
            Weights[EventRarity.Uncommon] = 0.20;
            Weights[EventRarity.Rare] = 0.11;
            Weights[EventRarity.Legendary] = 0.02;

            Events = new Dictionary<EventRarity, List<WorldEvent>>();
            for(EventRarity rarity = EventRarity.Common; rarity <= EventRarity.Legendary; ++rarity)
            {
                Events[rarity] = new List<WorldEvent>();
            }
        }

        public WorldEventTable(WorldEventTable other)
        {
            random = new Random();
            Weights = new Dictionary<EventRarity, double>(other.Weights);
            Events = new Dictionary<EventRarity, List<WorldEvent>>(other.Events);
        }

        public override WorldEvent GenerateObject(Hero hero, IFactory<WorldEvent> factory = null)
        {
            EventRarity rarity = EventRarity.Common;
            WorldEvent worldEvent = null;
            
            while(worldEvent == null)
            {
                rarity = GenerateRarity();
                worldEvent = GenerateEvent(rarity);
            }

            if(worldEvent.UniquePerSession || worldEvent.UniquePerHero)
            {
                // Remove it from our list
                Events[rarity].Remove(worldEvent);

                // Have we already done this event on this hero?
                if(worldEvent.UniquePerHero)
                {
                    // Find a different event
                    if (hero.UniqueEventsSeen.Contains(worldEvent.Name))
                        return GenerateObject(hero);
                    // Remember this event
                    else
                        hero.UniqueEventsSeen.Add(worldEvent.Name);
                }
            }

            worldEvent.Hero = hero;
            return worldEvent;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        // Monster rarity weights - should be organized from highest weight to lowest
        public Dictionary<EventRarity, double> Weights { get; set; }

        public Dictionary<EventRarity, List<WorldEvent>> Events { get; set; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private EventRarity GenerateRarity()
        {
            double percentile = random.NextDouble();

            foreach (var weight in Weights)
            {
                if (percentile < weight.Value)
                {
                    return weight.Key;
                }
                else
                {
                    percentile -= weight.Value;
                }
            }

            return EventRarity.Legendary;
        }

        private WorldEvent GenerateEvent(EventRarity rarity)
        {
            int index = random.Next(0, Events[rarity].Count);

            WorldEvent worldEvent = null;
            if (Events[rarity].Count != 0)
            {
                worldEvent = Events[rarity][index];
            }

            return worldEvent;
        }
    }
}
