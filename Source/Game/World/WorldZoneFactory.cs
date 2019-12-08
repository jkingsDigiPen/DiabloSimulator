//------------------------------------------------------------------------------
//
// File Name:	WorldZoneFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using Newtonsoft.Json;
using System.Collections.Generic;
using static DiabloSimulator.Game.RandomGeneration.WorldEventTable;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    class WorldZoneFactory : IFactory<WorldZone>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldZoneFactory()
        {
            AddZoneArchetypes();
            //LoadArchetypesFromFile();
        }

        public override void AddArchetype(WorldZone archetype)
        {
            archetypes[archetype.Name] = archetype;
        }

        public override WorldZone Create(string name)
        {
            return new WorldZone(archetypes[name]);
        }

        public override WorldZone Create(string name, Hero hero)
        {
            return Create(name);
        }

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        protected override void LoadArchetypesFromFile()
        {
            var stream = new System.IO.StreamReader(archetypesFileName);
            string zoneStrings = stream.ReadToEnd();
            stream.Close();

            archetypes = JsonConvert.DeserializeObject<Dictionary<string, WorldZone>>(zoneStrings);
        }

        protected override void SaveArchetypesToFile()
        {
            var stream = new System.IO.StreamWriter("../../../" + archetypesFileName);

            string zoneStrings = JsonConvert.SerializeObject(archetypes, Formatting.Indented);
            stream.Write(zoneStrings);

            stream.Close();
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void AddZoneArchetypes()
        {
            // TRISTRAM
            WorldZone zone = new WorldZone("New Tristram", WorldZoneType.Town, 
                "You are in the town of New Tristram, a place of relative safety.");
            zone.ConnectedZones.Add("Old Tristram Road");

            // Discover next zone
            WorldEvent worldEvent = new WorldEvent("Old Tristram Road", GameEvents.WorldZoneDiscovery,
                "Exiting the town's eastern gate, you find a weathered road leading north.");
            zone.EventTable.Events[EventRarity.Common].Add(worldEvent);

            AddArchetype(zone);

            // OLD TRISTRAM ROAD
            zone = new WorldZone("Old Tristram Road", WorldZoneType.Outdoors,
                "You are in Old Tristram Road, just outside of the town of Tristram. An eerie fog blankets" +
                " the area, making it difficult to see more than about 30 feet in front of you.");
            zone.ConnectedZones.Add("Tristram");
            zone.MonsterTable.Monsters[MonsterRarity.Common].Add("Fallen Imp");
            zone.MonsterTable.Monsters[MonsterRarity.Uncommon].Add("Fallen Shaman");

            worldEvent = new WorldEvent("Wandering Monster", GameEvents.WorldMonster,
                "As you wander, you stumble into the territory of a monster.");
            zone.EventTable.Events[EventRarity.Common].Add(worldEvent);

            AddArchetype(zone);

            SaveArchetypesToFile();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private const string archetypesFileName = "Data/WorldZones.txt";
    }
}
