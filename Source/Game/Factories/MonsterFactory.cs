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

    public class MonsterFactory
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public MonsterFactory()
        {
            random = new Random();
        }

        public Monster CreateMonster(Hero hero)
        {
            // TO DO: Get valid types from location
            MonsterType monsterType = (MonsterType)random.Next(
                (int)MonsterType.FallenImp, (int)MonsterType.FallenShaman + 1);

            // Generate level, based on hero level (+/- 2 levels)
            int level = random.Next(Math.Max(1, (int)hero.stats.Level - 2), 
                ((int)hero.stats.Level + 2) + 1);

            return GenerateStats(monsterType, level);
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private Monster GenerateStats(MonsterType monsterType, int level_)
        {
            Monster monster = null;
            uint level = (uint)level_;

            switch (monsterType)
            {
                case MonsterType.FallenImp:
                    monster = new Monster("Fallen Imp", level, "Demon");
                    monster.stats["MaxHealth"] = 10;
                    monster.stats.SetProgression("MaxHealth", 12);
                    monster.stats["MinDamage"] = 1;
                    monster.stats["MaxDamage"] = 4;
                    monster.stats.SetProgression("MinDamage", 1);
                    monster.stats.SetProgression("MaxDamage", 1);
                    monster.stats["Experience"] = 5;
                    monster.stats.SetProgression("Experience", 5);
                    break;

                case MonsterType.FallenShaman:
                    monster = new Monster("Fallen Shaman", level, "Demon");
                    monster.stats["MaxHealth"] = 30;
                    monster.stats.SetProgression("MaxHealth", 20);
                    monster.stats["MinFireDamage"] = 2;
                    monster.stats["MaxFireDamage"] = 8;
                    monster.stats.SetProgression("MinFireDamage", 2);
                    monster.stats.SetProgression("MaxFireDamage", 2);
                    monster.stats["Experience"] = 20;
                    monster.stats.SetProgression("Experience", 10);
                    break;
            }

            // Current/Max Health
            monster.stats["CurrentHealth"] = 0;
            monster.stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, monster.stats));

            return monster;
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
