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
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class StatTable
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public StatTable()
        {
            values = new Dictionary<string, float>();
            modifiers = new Dictionary<string, Dictionary<ModifierType, HashSet<StatModifier>>>();
        }

        public float GetModifiedValue(string name)
        {
            float baseValue = values[name];
            HashSet<StatModifier> addMods = modifiers[name][ModifierType.Additive];
            foreach(StatModifier modifier in addMods)
            {
                baseValue += modifier.modValue;
            }

            HashSet<StatModifier> multMods = modifiers[name][ModifierType.Multiplicative];
            float totalMult = 1.0f;
            foreach (StatModifier modifier in multMods)
            {
                totalMult += modifier.modValue;
            }
            baseValue *= totalMult;

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

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Dictionary<string, float> values;
        private Dictionary<string, Dictionary<ModifierType, HashSet<StatModifier>>> modifiers;
    }
}
