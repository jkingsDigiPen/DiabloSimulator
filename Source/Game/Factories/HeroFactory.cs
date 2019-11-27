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
            hero.Stats["Strength"] = 10;
            hero.Stats["Dexterity"] = 8;
            hero.Stats["Intelligence"] = 8;
            hero.Stats["Vitality"] = 9;

            // Set stat progression
            hero.Stats.SetProgression("Strength", 3);
            hero.Stats.SetProgression("Dexterity", 1);
            hero.Stats.SetProgression("Intelligence", 1);
            hero.Stats.SetProgression("Vitality", 2);

            // Add modifiers
            hero.Stats.AddModifier(new StatModifier("MinDamage", "Strength",
                ModifierType.Multiplicative, 0.01f, hero.Stats));
            hero.Stats.AddModifier(new StatModifier("MaxDamage", "Strength",
                ModifierType.Multiplicative, 0.01f, hero.Stats));

            AddArchetype(hero);

            // ROGUE
            hero = new Hero("The Rogue", "Rogue");

            // Set starting stats
            hero.Stats["Strength"] = 8;
            hero.Stats["Dexterity"] = 10;
            hero.Stats["Intelligence"] = 8;
            hero.Stats["Vitality"] = 9;

            // Set stat progression
            hero.Stats.SetProgression("Strength", 1);
            hero.Stats.SetProgression("Dexterity", 3);
            hero.Stats.SetProgression("Intelligence", 1);
            hero.Stats.SetProgression("Vitality", 2);

            // Add modifiers
            hero.Stats.AddModifier(new StatModifier("MinDamage", "Dexterity",
                ModifierType.Multiplicative, 0.01f, hero.Stats));
            hero.Stats.AddModifier(new StatModifier("MaxDamage", "Dexterity",
                ModifierType.Multiplicative, 0.01f, hero.Stats));

            AddArchetype(hero);

            // SORCERER
            hero = new Hero("The Sorcerer", "Sorcerer");

            // Set starting stats
            hero.Stats["Strength"] = 8;
            hero.Stats["Dexterity"] = 8;
            hero.Stats["Intelligence"] = 10;
            hero.Stats["Vitality"] = 9;

            // Set stat progression
            hero.Stats.SetProgression("Strength", 1);
            hero.Stats.SetProgression("Dexterity", 1);
            hero.Stats.SetProgression("Intelligence", 3);
            hero.Stats.SetProgression("Vitality", 2);

            // Add modifiers
            hero.Stats.AddModifier(new StatModifier("MinDamage", "Intelligence",
                ModifierType.Multiplicative, 0.01f, hero.Stats));
            hero.Stats.AddModifier(new StatModifier("MaxDamage", "Intelligence",
                ModifierType.Multiplicative, 0.01f, hero.Stats));

            AddArchetype(hero);
        }

        private void InitCommonStats(Hero hero)
        {
            // Damage
            hero.Stats["MinDamage"] = 2;
            hero.Stats["MaxDamage"] = 3;
            hero.Stats["CriticalHitChance"] = 0.05f;
            hero.Stats["CriticalHitDamage"] = 1.5f;

            // Vitality gives health per point
            hero.Stats["MaxHealth"] = 0;
            hero.Stats.AddModifier(new StatModifier("MaxHealth", "Vitality",
                ModifierType.Additive, 3, hero.Stats));

            // Current/Max Health
            hero.Stats["CurrentHealth"] = 0;
            hero.Stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, hero.Stats));

            // Strength gives armor per point
            hero.Stats["Armor"] = 0;
            hero.Stats.AddModifier(new StatModifier("Armor", "Strength",
                ModifierType.Additive, 1, hero.Stats));

            // Dexterity gives dodge chance per point
            hero.Stats["DodgeChance"] = 0;
            hero.Stats.AddModifier(new StatModifier("DodgeChance", "Dexterity",
                ModifierType.Additive, 0.001f, hero.Stats));

            // Intelligence gives resistances per point
            hero.Stats["FireResist"] = 0;
            hero.Stats["ColdResist"] = 0;
            hero.Stats["LightningResist"] = 0;
            hero.Stats["PoisionResist"] = 0;
            hero.Stats.AddModifier(new StatModifier("FireResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.Stats));
            hero.Stats.AddModifier(new StatModifier("ColdResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.Stats));
            hero.Stats.AddModifier(new StatModifier("LightningResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.Stats));
            hero.Stats.AddModifier(new StatModifier("PoisonResist", "Intelligence",
                ModifierType.Additive, 0.1f, hero.Stats));

            // Experience
            hero.Stats["Experience"] = 0;
        }
    }
}
