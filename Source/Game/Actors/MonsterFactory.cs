//------------------------------------------------------------------------------
//
// File Name:	MonsterFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DiabloSimulator.Engine;
using Newtonsoft.Json;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class MonsterFactory : IFactory<Monster>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public MonsterFactory()
        {
            random = new Random();

            //AddMonsterArchetypes();
            LoadArchetypesFromFile();
        }

        public override Monster Create(string name)
        {
            return new Monster(archetypes[name]);
        }

        public override Monster Create(string name, Hero hero)
        {
            var monster = Create(name);

            // Increase monster level as necessary
            if (monster.Stats.Level < hero.Stats.Level - 3)
            {
                monster.Stats.Level = random.Next(hero.Stats.Level - 3, hero.Stats.Level + 1);
            }

            return monster;
        }

        public override void AddArchetype(Monster archetype)
        {
            archetypes.Add(archetype.Name, archetype);
        }

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        protected override void LoadArchetypesFromFile()
        {
            var stream = new System.IO.StreamReader(archetypesFileName);
            string monsterStrings = stream.ReadToEnd();
            stream.Close();

            archetypes = JsonConvert.DeserializeObject<Dictionary<string, Monster>>(monsterStrings);

            // Remap modifier sources for local modifiers
            /*foreach (KeyValuePair<string, Monster> monster in archetypes)
            {
                monster.Value.Stats.RemapModifierSources(monster.Value);
            }*/
        }

        protected override void SaveArchetypesToFile()
        {
            var stream = new System.IO.StreamWriter("../../../" + archetypesFileName);

            string monsterStrings = JsonConvert.SerializeObject(archetypes, Formatting.Indented);
            stream.Write(monsterStrings);

            stream.Close();
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void AddMonsterArchetypes()
        {
            Monster monster = new Monster("Fallen Imp", 1, 
                MonsterRarity.Common, "Demon", "Fallen");
            monster.Stats["MaxHealth"] = 10;
            monster.Stats.SetProgression("MaxHealth", 12);
            monster.Stats["MinDamage"] = 1;
            monster.Stats["MaxDamage"] = 4;
            monster.Stats.SetProgression("MinDamage", 1);
            monster.Stats.SetProgression("MaxDamage", 1);
            monster.Stats["Experience"] = 5;
            monster.Stats.SetProgression("Experience", 5);
            monster.Stats["CurrentHealth"] = 0;
            monster.Stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, monster));
            AddArchetype(monster);

            monster = new Monster("Fallen Shaman", 1,
                MonsterRarity.Common, "Demon", "Fallen");
            monster.Stats["MaxHealth"] = 30;
            monster.Stats.SetProgression("MaxHealth", 20);
            monster.Stats["MinFireDamage"] = 2;
            monster.Stats["MaxFireDamage"] = 8;
            monster.Stats.SetProgression("MinFireDamage", 2);
            monster.Stats.SetProgression("MaxFireDamage", 2);
            monster.Stats["Experience"] = 20;
            monster.Stats.SetProgression("Experience", 10);
            monster.Stats["CurrentHealth"] = 0;
            monster.Stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, monster));
            AddArchetype(monster);

           SaveArchetypesToFile();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private const string archetypesFileName = "Data/Monsters.txt";
        private Random random;
    }
}
