//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;

namespace DiabloSimulator.Game
{
    using StatMap = ObservableDictionaryNoThrow<string, float>;
    using StatDependantMap = Dictionary<string, List<string>>;
    using ModifierMap = Dictionary<ModifierType, HashSet<StatModifier>>;

    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class StatTable : INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public StatTable(uint level_ = 1)
        {
            BaseValues = new StatMap();
            LeveledValues = new StatMap();
            ModifiedValues = new StatMap();
            Progressions = new StatMap();
            Modifiers = new Dictionary<string, ModifierMap>();
            Dependants = new StatDependantMap();
            level = level_;
        }

        public StatTable(StatTable other)
        {
            // Base values must be copied
            BaseValues = new StatMap(other.BaseValues);

            // Don't need to copy these directly, as changing level will propagate changes
            LeveledValues = new StatMap();
            ModifiedValues = new StatMap();

            // Progressions, modifiers, dependants must be copied
            Progressions = new StatMap(other.Progressions);
            Modifiers = new Dictionary<string, ModifierMap>(other.Modifiers);
            Dependants = new StatDependantMap(other.Dependants);

            // Redo mod sources
            RemapModifierSources(other, this);

            // Using property fills out leveled and modified values
            Level = other.Level;
        }

        public float this[string key]
        {
            set
            {
                BaseValues[key] = value;
                LeveledValues[key] = 0.0f;
                ModifiedValues[key] = 0.0f;
                UpdateLeveledValue(key);
                UpdateModifiedValue(key);

                OnPropertyChange("BaseValues");
                OnPropertyChange("LeveledValues");
                OnPropertyChange("ModifiedValues");
            }
        }

        public StatMap BaseValues { get; }

        public StatMap LeveledValues { get; }

        public StatMap ModifiedValues { get; }

        public StatMap Progressions { get; }

        public StatDependantMap Dependants { get; }

        public Dictionary<string, ModifierMap> Modifiers { get; }

        public uint Level
        {
            get { return level; }
            set
            {
                if (value != level)
                {
                    level = value;
                    UpdateAllLeveledValues();
                    UpdateAllModifiedValues();
                    OnPropertyChange("Level");
                }
            }
        }

        public void AddModifier(StatModifier mod)
        {
            // Add the mod to the list of modifiers for this stat
            ModifierMap modMap = null;
            if(!Modifiers.TryGetValue(mod.statName, out modMap))
            {
                Modifiers[mod.statName] = new ModifierMap();
            }

            HashSet<StatModifier> modSet;
            if(!Modifiers[mod.statName].TryGetValue(mod.type, out modSet))
            {
                Modifiers[mod.statName][mod.type] = new HashSet<StatModifier>();
            }
            Modifiers[mod.statName][mod.type].Add(mod);

            // Add the mod stat as a dependant of the mod source
            List<string> depList;
            if(!Dependants.TryGetValue(mod.modSourceStat, out depList))
            {
                Dependants[mod.modSourceStat] = new List<string>();
            }
            Dependants[mod.modSourceStat].Add(mod.statName);

            UpdateModifiedValue(mod.statName);
            OnPropertyChange("ModifiedValues");
        }

        public void RemoveModifier(StatModifier mod)
        {
            // Attempt to find modifier
            ModifierMap modMap;
            if (!Modifiers.TryGetValue(mod.statName, out modMap))
                return;

            // Remove mod
            modMap[mod.type].Remove(mod);
            UpdateModifiedValue(mod.statName);

            // Remove mod stat as dependant
            Dependants[mod.modSourceStat].Remove(mod.statName);

            OnPropertyChange("ModifiedValues");
        }

        public void SetProgression(string name, float progression)
        {
            Progressions[name] = progression;
            UpdateLeveledValue(name);
            UpdateModifiedValue(name);
            OnPropertyChange("LeveledValues");
            OnPropertyChange("ModifiedValues");
        }

        public void RemapModifierSources(GameObject modSourceObject)
        {
            foreach (KeyValuePair<string, ModifierMap> modMap in Modifiers)
            {
                foreach (KeyValuePair<ModifierType, HashSet<StatModifier>> modSet in modMap.Value)
                {
                    foreach (StatModifier mod in modSet.Value)
                    {
                        mod.UpdateSourceTable(modSourceObject);
                    }
                }
            }
        }

        public void RemapModifierSources(StatTable oldSourceTable, StatTable newSourceTable)
        {
            foreach (KeyValuePair<string, ModifierMap> modMap in Modifiers)
            {
                foreach (KeyValuePair<ModifierType, HashSet<StatModifier>> modSet in modMap.Value)
                {
                    foreach (StatModifier mod in modSet.Value)
                    {
                        mod.UpdateSourceTable(oldSourceTable, newSourceTable);
                    }
                }
            }
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void UpdateAllLeveledValues()
        {
            foreach (KeyValuePair<string, float> pair in BaseValues)
            {
                UpdateLeveledValue(pair.Key);
            }
            OnPropertyChange("LeveledValues");
        }

        private void UpdateLeveledValue(string name)
        {
            float progression = 0.0f;
            Progressions.TryGetValue(name, out progression);
            LeveledValues[name] = BaseValues[name] + progression * (Level - 1);
        }

        private void UpdateAllModifiedValues()
        {
            foreach(KeyValuePair<string, float> pair in LeveledValues)
            {
                UpdateModifiedValue(pair.Key);
            }
            OnPropertyChange("ModifiedValues");
        }

        private void UpdateModifiedValue(string name)
        {
            ModifiedValues[name] = LeveledValues[name];
            ModifierMap modMap;
            if (Modifiers.TryGetValue(name, out modMap))
            {
                HashSet<StatModifier> addMods;
                if (modMap.TryGetValue(ModifierType.Additive, out addMods))
                {
                    foreach (StatModifier modifier in addMods)
                    {
                        ModifiedValues[name] += modifier.ModValueWithTable();
                    }
                }

                HashSet<StatModifier> multMods;
                if (modMap.TryGetValue(ModifierType.Multiplicative, out multMods))
                {
                    float totalMult = 1.0f;
                    foreach (StatModifier modifier in multMods)
                    {
                        totalMult += modifier.ModValueWithTable();
                    }
                    ModifiedValues[name] *= totalMult;
                }
            }

            // Propagate changes to dependant stats
            List<string> depList;
            if(Dependants.TryGetValue(name, out depList))
            {
                foreach(string dependant in depList)
                {
                    UpdateModifiedValue(dependant);
                }
            }

            OnPropertyChange(name);
        }

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private uint level;
    }
}
