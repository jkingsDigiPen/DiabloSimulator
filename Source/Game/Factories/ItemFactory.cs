//------------------------------------------------------------------------------
//
// File Name:	ItemFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DiabloSimulator.Game.Factories
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    class ItemFactory : IFactory<Item>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public ItemFactory()
        {
            random = new Random();

            //AddItemArchetypes();
            LoadArchetypesFromFile();
        }

        public override void AddArchetype(Item archetype)
        {
            archetypes.Add(archetype.Name, archetype);
        }

        public override Item Create(Hero hero)
        {
            var validItems = new List<string>();

            foreach (KeyValuePair<string, Item> item in archetypes)
            {
                // Make sure required level is appropriate
                if (item.Value.Stats.Level <= hero.Stats.Level)
                {
                    validItems.Add(item.Value.Name);
                }
            }

            int itemIndex = random.Next(0, validItems.Count);

            Item clonedItem = Create(hero, validItems[itemIndex]);

            return clonedItem;
        }

        // Create a specific item
        public Item Create(Hero hero, string name)
        {
            Item clonedItem = CloneArchetype(name);

            // Level up the item
            if(clonedItem.Stats.Level < hero.Stats.Level)
                clonedItem.Stats.Level = hero.Stats.Level;

            // TO DO: Decide on rarity, magical properties

            return clonedItem;
        }

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        protected override Item CloneArchetype(string name)
        {
            return new Item(archetypes[name]);
        }

        protected override void LoadArchetypesFromFile()
        {
            var stream = new System.IO.StreamReader(archetypesFileName);
            string itemStrings = stream.ReadToEnd();
            stream.Close();

            archetypes = JsonConvert.DeserializeObject<Dictionary<string, Item>>(itemStrings);
        }

        protected override void SaveArchetypesToFile()
        {
            var stream = new System.IO.StreamWriter("../../../" + archetypesFileName);

            string itemStrings = JsonConvert.SerializeObject(archetypes, Formatting.Indented);
            stream.Write(itemStrings);

            stream.Close();
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void AddItemArchetypes()
        {
            Item testItem = new Item("Simple Dagger", SlotType.MainHand, ItemRarity.Common, "Dagger");
            testItem.Stats["MinDamage"] = 2;
            testItem.Stats["MaxDamage"] = 6;
            testItem.Stats.Level = 1;
            AddArchetype(testItem);

            testItem = new Item("Short Sword", SlotType.MainHand, ItemRarity.Common, "1-Handed Sword");
            testItem.Stats["MinDamage"] = 1;
            testItem.Stats["MaxDamage"] = 7;
            testItem.Stats.Level = 1;
            AddArchetype(testItem);

            testItem = new Item("Leather Hood", SlotType.Head, ItemRarity.Magic, "Helm");
            testItem.Stats["Armor"] = 21;
            testItem.Stats["Vitality"] = 4;
            testItem.Stats["HealthRegen"] = 2;
            testItem.Stats.Level = 4;
            AddArchetype(testItem);

            SaveArchetypesToFile();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private const string archetypesFileName = "Data/Items.txt";

        private Random random;
    }
}
