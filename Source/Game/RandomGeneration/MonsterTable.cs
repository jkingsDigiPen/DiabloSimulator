//------------------------------------------------------------------------------
//
// File Name:	MonsterTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DiabloSimulator.Game.RandomGeneration
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class MonsterTable : IRandomTable<Monster>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        [JsonConstructor]
        public MonsterTable()
        {
            random = new Random();

            Weights = new Dictionary<MonsterRarity, double>();
            Weights[MonsterRarity.Common] = 0.67;
            Weights[MonsterRarity.Uncommon] = 0.20;
            Weights[MonsterRarity.Elite] = 0.11;
            Weights[MonsterRarity.Legendary] = 0.02;

            Monsters = new Dictionary<MonsterRarity, List<string>>();
            for (MonsterRarity rarity = MonsterRarity.Common; rarity <= MonsterRarity.Legendary; ++rarity)
            {
                Monsters[rarity] = new List<string>();
            }
        }

        public MonsterTable(MonsterTable other)
        {
            random = new Random();
            Weights = new Dictionary<MonsterRarity, double>(other.Weights);
            Monsters = new Dictionary<MonsterRarity, List<string>>(other.Monsters);
        }

        public override Monster GenerateObject(Hero hero, IFactory<Monster> factory)
        {
            // Keep trying to generate monsters
            string name = null;
            while (name == null)
            {
                name = GenerateName(GenerateRarity());
            }

            return factory.Create(name, hero);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        // Monster rarity weights - should be organized from highest weight to lowest
        public Dictionary<MonsterRarity, double> Weights { get; set; }

        public Dictionary<MonsterRarity, List<string>> Monsters { get; set; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private MonsterRarity GenerateRarity()
        {
            double percentile = random.NextDouble();

            foreach(var weight in Weights)
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

            return MonsterRarity.Legendary;
        }

        private string GenerateName(MonsterRarity rarity)
        {
            int index = random.Next(0, Monsters[rarity].Count);
            string monsterName = null;
            if(Monsters[rarity].Count != 0)
            {
                monsterName = Monsters[rarity][index];
            }

            return monsterName;
        }
    }
}
