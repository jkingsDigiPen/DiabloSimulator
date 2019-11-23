//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    using StatMap = Dictionary<string, float>;
    using ModifierMap = Dictionary<ModifierType, HashSet<StatModifier>>;

    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class StatTable
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public StatTable(uint level_ = 1)
        {
            level = level_;
            values = new StatMap();
            progressions = new StatMap();
            modifiers = new Dictionary<string, Dictionary<ModifierType, HashSet<StatModifier>>>();
        }

        public float this[string key]
        {
            get { return GetModifiedValue(key); }
            set { SetBaseValue(key, value); }
        }

        public float GetBaseValue(string name)
        {
            float progression = 0.0f;
            progressions.TryGetValue(name, out progression);

            return values[name] + progression * (level - 1);
        }

        public float GetModifiedValue(string name)
        {
            float baseValue = GetBaseValue(name);
            ModifierMap modMap;
            if (modifiers.TryGetValue(name, out modMap))
            {
                HashSet<StatModifier> addMods;
                if (modMap.TryGetValue(ModifierType.Additive, out addMods))
                {
                    foreach (StatModifier modifier in addMods)
                    {
                        baseValue += modifier.ModValue;
                    }
                }

                HashSet<StatModifier> multMods;
                if (modMap.TryGetValue(ModifierType.Multiplicative, out multMods))
                {
                    float totalMult = 1.0f;
                    foreach (StatModifier modifier in multMods)
                    {
                        totalMult += modifier.ModValue;
                    }
                    baseValue *= totalMult;
                }
            }

            return baseValue;
        }

        public void IncreaseBaseValue(string name, float value)
        {
            values[name] += value;
            OnPropertyChanged("Property");
        }

        public void SetBaseValue(string name, float value)
        {
            values[name] = value;
            OnPropertyChanged("Property");
        }

        public void AddModifier(StatModifier mod)
        {
            ModifierMap modMap = null;
            if(!modifiers.TryGetValue(mod.statName, out modMap))
            {
                modifiers[mod.statName] = new ModifierMap();
                modifiers[mod.statName][mod.type] = new HashSet<StatModifier>();
            }

            modifiers[mod.statName][mod.type].Add(mod);
        }

        public void RemoveModifier(StatModifier mod)
        {
            modifiers[mod.statName][mod.type].Remove(mod);
        }

        public void SetProgression(string name, float progression)
        {
            progressions[name] = progression;
        }

        public uint Level 
        { 
            get { return level; } 
            set { level = value; }
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private uint level;

        private StatMap values;
        private StatMap progressions;
        private Dictionary<string, ModifierMap> modifiers;
    }
}
