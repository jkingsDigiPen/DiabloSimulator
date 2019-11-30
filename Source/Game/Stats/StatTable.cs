//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

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

        #region constructors

        [JsonConstructor]
        public StatTable(int level_ = 1)
        {
            id = nextId++;

            BaseValues = new StatMap();
            LeveledValues = new StatMap();
            ModifiedValues = new StatMap();
            Progressions = new StatMap();
            Modifiers = new Dictionary<string, ModifierMap>();
            Dependants = new StatDependantMap();
            Level = level_;
        }

        public StatTable(StatTable other)
        {
            id = nextId++;

            // Base values must be copied
            BaseValues = new StatMap(other.BaseValues);

            // Don't need to copy these directly, as changing level will propagate changes
            LeveledValues = new StatMap();
            ModifiedValues = new StatMap();

            // Progressions, modifiers, dependants must be copied
            Progressions = new StatMap(other.Progressions);
            Dependants = new StatDependantMap(other.Dependants);

            // Modifiers need extra care, as they need to refer to other objects
            Modifiers = new Dictionary<string, ModifierMap>();
            foreach(KeyValuePair<string, ModifierMap> modMap in other.Modifiers)
            {
                foreach(KeyValuePair<ModifierType, HashSet<StatModifier>> modSet in modMap.Value)
                {
                    foreach(StatModifier mod in modSet.Value)
                    {
                        AddModifier(new StatModifier(mod, this));
                    }
                }
            }

            // Using property fills out leveled and modified values
            Level = other.Level;
        }

        #endregion

        #region properties

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

        public int Level
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

        #endregion

        #region modifiers

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
            if (mod.modSourceStat != null)
            {
                List<string> depList;
                if (!Dependants.TryGetValue(mod.modSourceStat, out depList))
                {
                    Dependants[mod.modSourceStat] = new List<string>();
                }
                Dependants[mod.modSourceStat].Add(mod.statName);
            }

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
            if(mod.modSourceStat != null)
                Dependants[mod.modSourceStat].Remove(mod.statName);

            OnPropertyChange("ModifiedValues");
        }

        #endregion

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

            UpdateAllModifiedValues();
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

        private int level;

        private uint id;
        private static uint nextId = 0;
    }
}
