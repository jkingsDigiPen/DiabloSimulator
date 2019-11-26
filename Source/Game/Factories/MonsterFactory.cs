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
            int level = random.Next(Math.Max(1, (int)hero.stats.Level - 2), 
                 ((int)hero.stats.Level + 2) + 1);

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

            monster.stats.Level = (uint)level;

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
            monster.stats["MaxHealth"] = 10;
            monster.stats.SetProgression("MaxHealth", 12);
            monster.stats["MinDamage"] = 1;
            monster.stats["MaxDamage"] = 4;
            monster.stats.SetProgression("MinDamage", 1);
            monster.stats.SetProgression("MaxDamage", 1);
            monster.stats["Experience"] = 5;
            monster.stats.SetProgression("Experience", 5);
            monster.stats["CurrentHealth"] = 0;
            monster.stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, monster.stats));
            AddArchetype(monster);

            Monster monster2 = new Monster("Fallen Shaman", 1, "Demon");
            monster2.stats["MaxHealth"] = 30;
            monster2.stats.SetProgression("MaxHealth", 20);
            monster2.stats["MinFireDamage"] = 2;
            monster2.stats["MaxFireDamage"] = 8;
            monster2.stats.SetProgression("MinFireDamage", 2);
            monster2.stats.SetProgression("MaxFireDamage", 2);
            monster2.stats["Experience"] = 20;
            monster2.stats.SetProgression("Experience", 10);
            monster2.stats["CurrentHealth"] = 0;
            monster2.stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, monster2.stats));
            AddArchetype(monster2);
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
