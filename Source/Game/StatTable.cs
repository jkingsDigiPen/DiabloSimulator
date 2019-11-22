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

        public StatTable(uint level_ = 1, Dictionary<string, float> values_ = null, Dictionary<string, float> progression_ = null)
        {
            level = level_;
            values = values_;
            progression = progression_;
            modifiers = new Dictionary<string, Dictionary<ModifierType, HashSet<StatModifier>>>();
        }

        public float GetBaseValue(string name)
        {
            return values[name] + progression[name] * (level - 1);
        }

        public float GetModifiedValue(string name)
        {
            float baseValue = GetBaseValue(name);
            ModifierMap modMap;
            if (modifiers.TryGetValue(name, out modMap))
            {
                HashSet<StatModifier> addMods = modMap[ModifierType.Additive];
                foreach (StatModifier modifier in addMods)
                {
                    baseValue += modifier.modValue;
                }

                HashSet<StatModifier> multMods = modMap[ModifierType.Multiplicative];
                float totalMult = 1.0f;
                foreach (StatModifier modifier in multMods)
                {
                    totalMult += modifier.modValue;
                }
                baseValue *= totalMult;
            }

            return baseValue;
        }

        public void IncreaseBaseValue(string name, float value)
        {
            values[name] += value;
        }

        public void SetBaseValue(string name, float value)
        {
            values[name] = value;
        }

        public void AddModifier(StatModifier mod)
        {
            modifiers[mod.statName][mod.type].Add(mod);
        }

        public void RemoveModifier(StatModifier mod)
        {
            modifiers[mod.statName][mod.type].Remove(mod);
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

        StatMap values;
        StatMap progression;
        private Dictionary<string, ModifierMap> modifiers;
    }
}
