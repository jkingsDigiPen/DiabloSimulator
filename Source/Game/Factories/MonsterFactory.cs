//------------------------------------------------------------------------------
//
// File Name:	MonsterFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;

namespace DiabloSimulator.Game.Factories
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

            AddMonsterArchetypes();
        }

        public override Monster Create(Hero hero)
        {
            // TO DO: Get valid types from location
            MonsterType monsterType = (MonsterType)random.Next(
                (int)MonsterType.FallenImp, (int)MonsterType.FallenShaman + 1);

            // Generate level, based on hero level (+/- 2 levels)
            int level = random.Next(Math.Max(1, (int)hero.Stats.Level - 2), 
                 ((int)hero.Stats.Level + 2) + 1);

            Monster monster = null;

            switch(monsterType)
            {
                case MonsterType.FallenImp:
                    monster = CloneArchetype("Fallen Imp");
                    break;
                case MonsterType.FallenShaman:
                    monster = CloneArchetype("Fallen Shaman");
                    break;
            }

            monster.Stats.Level = (uint)level;

            return monster;
        }

        public override void AddArchetype(Monster archetype)
        {
            archetypes.Add(archetype.Name, archetype);
        }

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        protected override Monster CloneArchetype(string name)
        {
            return new Monster(archetypes[name]);
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void AddMonsterArchetypes()
        {
            Monster monster = new Monster("Fallen Imp", 1, "Demon");
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
                ModifierType.Additive, 1, monster.Stats));
            AddArchetype(monster);

            monster = new Monster("Fallen Shaman", 1, "Demon");
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
                ModifierType.Additive, 1, monster.Stats));
            AddArchetype(monster);
        }

        //------------------------------------------------------------------------------
        // Private Structures:
        //------------------------------------------------------------------------------

        private enum MonsterType
        {
            FallenImp,
            FallenShaman,
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Random random;
    }
}
