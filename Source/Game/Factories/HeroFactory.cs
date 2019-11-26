//------------------------------------------------------------------------------
//
// File Name:	HeroFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;

namespace DiabloSimulator.Game.Factories
{
    public class HeroFactory : IFactory<Hero>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroFactory()
        {
            AddHeroArchetypes();
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

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void AddHeroArchetypes()
        {
            Hero hero = new Hero("The Warrior", "Warrior");

            // WARRIOR

            // Set starting stats
            hero.stats["Strength"] = 10;
            hero.stats["Dexterity"] = 8;
            hero.stats["Intelligence"] = 8;
            hero.stats["Vitality"] = 9;

            // Set stat progression
            hero.stats.SetProgression("Strength", 3);
            hero.stats.SetProgression("Dexterity", 1);
            hero.stats.SetProgression("Intelligence", 1);
            hero.stats.SetProgression("Vitality", 2);

            // Add modifiers
            hero.stats.AddModifier(new StatModifier("MinDamage", "Strength",
                ModifierType.Multiplicative, 0.01f, hero.stats));
            hero.stats.AddModifier(new StatModifier("MaxDamage", "Strength",
                ModifierType.Multiplicative, 0.01f, hero.stats));

            AddArchetype(hero);

            // ROGUE
            hero = new Hero("The Rogue", "Rogue");

            // Set starting stats
            hero.stats["Strength"] = 8;
            hero.stats["Dexterity"] = 10;
            hero.stats["Intelligence"] = 8;
            hero.stats["Vitality"] = 9;

            // Set stat progression
            hero.stats.SetProgression("Strength", 1);
            hero.stats.SetProgression("Dexterity", 3);
            hero.stats.SetProgression("Intelligence", 1);
            hero.stats.SetProgression("Vitality", 2);

            // Add modifiers
            hero.stats.AddModifier(new StatModifier("MinDamage", "Dexterity",
                ModifierType.Multiplicative, 0.01f, hero.stats));
            hero.stats.AddModifier(new StatModifier("MaxDamage", "Dexterity",
                ModifierType.Multiplicative, 0.01f, hero.stats));

            AddArchetype(hero);

            // SORCERER
            hero = new Hero("The Sorcerer", "Sorcerer");

            // Set starting stats
            hero.stats["Strength"] = 8;
            hero.stats["Dexterity"] = 8;
            hero.stats["Intelligence"] = 10;
            hero.stats["Vitality"] = 9;

            // Set stat progression
            hero.stats.SetProgression("Strength", 1);
            hero.stats.SetProgression("Dexterity", 1);
            hero.stats.SetProgression("Intelligence", 3);
            hero.stats.SetProgression("Vitality", 2);

            // Add modifiers
            hero.stats.AddModifier(new StatModifier("MinDamage", "Intelligence",
                ModifierType.Multiplicative, 0.01f, hero.stats));
            hero.stats.AddModifier(new StatModifier("MaxDamage", "Intelligence",
                ModifierType.Multiplicative, 0.01f, hero.stats));

            AddArchetype(hero);
        }

        private void InitCommonStats(Hero hero)
        {
            // Damage
            hero.stats["MinDamage"] = 2;
            hero.stats["MaxDamage"] = 3;
            hero.stats["CriticalHitChance"] = 0.05f;
            hero.stats["CriticalHitDamage"] = 1.5f;

            // Vitality gives health per point
            hero.stats["MaxHealth"] = 0;
            hero.stats.AddModifier(new StatModifier("MaxHealth", "Vitality",
                ModifierType.Additive, 3, hero.stats));

            // Current/Max Health
            hero.stats["CurrentHealth"] = 0;
            hero.stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, hero.stats));

            // Strength gives armor per point
            hero.stats["Armor"] = 0;
            hero.stats.AddModifier(new StatModifier("Armor", "Strength",
                ModifierType.Additive, 1, hero.stats));

            // Dexterity gives dodge chance per point
            hero.stats["DodgeChance"] = 0;
            hero.stats.AddModifier(new StatModifier("DodgeChance", "Dexterity",
                ModifierType.Additive, 0.001f, hero.stats));

            // Intelligence gives resistances per point
            hero.stats["FireResist"] = 0;
            hero.stats["ColdResist"] = 0;
            hero.stats["LightningResist"] = 0;
            hero.stats["PoisionResist"] = 0;
            hero.stats.AddModifier(new StatModifier("FireResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.stats));
            hero.stats.AddModifier(new StatModifier("ColdResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.stats));
            hero.stats.AddModifier(new StatModifier("LightningResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.stats));
            hero.stats.AddModifier(new StatModifier("PoisonResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.stats));

            // Experience
            hero.stats["Experience"] = 0;
        }
    }
}
