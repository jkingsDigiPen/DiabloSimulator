//------------------------------------------------------------------------------
//
// File Name:	HeroFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DiabloSimulator.Game.Factories
{
    public class HeroFactory : IFactory<Hero>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroFactory()
        {
            //AddHeroArchetypes();
            LoadArchetypesFromFile();
        }

        public override Hero Create(Hero hero)
        {
            Hero newHero = CloneArchetype(hero.Archetype);
            newHero.Name = hero.Name;
            return newHero;
        }

        public override void AddArchetype(Hero archetype)
        {
            InitCommonStats(archetype);
            archetypes.Add(archetype.Archetype, archetype);
        }

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        protected override Hero CloneArchetype(string name)
        {
            return new Hero(archetypes[name]);
        }

        protected override void LoadArchetypesFromFile()
        {
            var stream = new System.IO.StreamReader(archetypesFileName);
            string heroStrings = stream.ReadToEnd();
            stream.Close();

            archetypes = JsonConvert.DeserializeObject<Dictionary<string, Hero>>(heroStrings);

            // Remap modifier sources for local modifiers
            foreach (KeyValuePair<string, Hero> hero in archetypes)
            {
                hero.Value.Stats.RemapModifierSources(hero.Value);
            }
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

        private void AddHeroArchetypes()
        {
            // WARRIOR
            Hero hero = new Hero("The Warrior", "Warrior");

            // Set starting stat priorities
            hero.StatPriorities.Add("Strength");
            hero.StatPriorities.Add("Vitality");
            hero.StatPriorities.Add("Dexterity");
            hero.StatPriorities.Add("Intelligence");

            AddArchetype(hero);

            // ROGUE
            hero = new Hero("The Rogue", "Rogue");

            // Set starting stat priorities
            hero.StatPriorities.Add("Dexterity");
            hero.StatPriorities.Add("Vitality");
            hero.StatPriorities.Add("Strength");
            hero.StatPriorities.Add("Intelligence");

            AddArchetype(hero);

            // SORCERER
            hero = new Hero("The Sorcerer", "Sorcerer");

            // Set starting stat priorities
            hero.StatPriorities.Add("Intelligence");
            hero.StatPriorities.Add("Vitality");
            hero.StatPriorities.Add("Dexterity");
            hero.StatPriorities.Add("Strength");

            AddArchetype(hero);

            SaveArchetypesToFile();
        }

        private void InitCommonStats(Hero hero)
        {
            // Initial stats
            string primaryStat = hero.StatPriorities[0];
            string secondaryStat = hero.StatPriorities[1];
            string tertiaryStat1 = hero.StatPriorities[2];
            string tertiaryStat2 = hero.StatPriorities[3];

            hero.Stats[primaryStat] = 10;
            hero.Stats[secondaryStat] = 9;
            hero.Stats[tertiaryStat1] = 8;
            hero.Stats[tertiaryStat2] = 8;

            // Stat progression
            hero.Stats.SetProgression(primaryStat, 3);
            hero.Stats.SetProgression(secondaryStat, 2);
            hero.Stats.SetProgression(tertiaryStat1, 1);
            hero.Stats.SetProgression(tertiaryStat2, 1);

            // Add damage modifiers
            hero.Stats.AddModifier(new StatModifier("MinDamage", primaryStat,
                ModifierType.Multiplicative, 0.01f, hero));
            hero.Stats.AddModifier(new StatModifier("MaxDamage", primaryStat,
                ModifierType.Multiplicative, 0.01f, hero));

            // Damage
            hero.Stats["MinDamage"] = 2;
            hero.Stats["MaxDamage"] = 3;
            hero.Stats["CriticalHitChance"] = 0.05f;
            hero.Stats["CriticalHitDamage"] = 1.5f;

            // Vitality gives health per point
            hero.Stats["MaxHealth"] = 0;
            hero.Stats.AddModifier(new StatModifier("MaxHealth", "Vitality",
                ModifierType.Additive, 3, hero));

            // Current/Max Health
            hero.Stats["CurrentHealth"] = 0;
            hero.Stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, hero));

            // Strength gives armor per point
            hero.Stats["Armor"] = 0;
            hero.Stats.AddModifier(new StatModifier("Armor", "Strength",
                ModifierType.Additive, 1, hero));

            // Dexterity gives dodge chance per point
            hero.Stats["DodgeChance"] = 0;
            hero.Stats.AddModifier(new StatModifier("DodgeChance", "Dexterity",
                ModifierType.Additive, 0.001f, hero));

            // Intelligence gives resistances per point
            hero.Stats["FireResist"] = 0;
            hero.Stats["ColdResist"] = 0;
            hero.Stats["LightningResist"] = 0;
            hero.Stats["PoisionResist"] = 0;
            hero.Stats.AddModifier(new StatModifier("FireResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero));
            hero.Stats.AddModifier(new StatModifier("ColdResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero));
            hero.Stats.AddModifier(new StatModifier("LightningResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero));
            hero.Stats.AddModifier(new StatModifier("PoisonResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero));

            // Experience
            hero.Stats["Experience"] = 0;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private const string archetypesFileName = "Data/HeroClasses.txt";
    }
}
