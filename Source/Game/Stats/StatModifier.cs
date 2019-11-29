//------------------------------------------------------------------------------
//
// File Name:	StatModifier.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public enum ModifierType
    {
        Additive,
        Multiplicative,
    }

    public class StatModifier
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        // Constructor for modifier
        [JsonConstructor]
        public StatModifier(string statName_, string modSourceStat_,
            ModifierType type_, float modValue_, GameObject modSourceObject_ = null)
        {
            statName = statName_;
            modSourceStat = modSourceStat_;
            type = type_;
            ModValue = modValue_;

            if (modSourceObject_ != null)
            {
                ModSourceObject = modSourceObject_.Archetype;
                modSourceTable = modSourceObject_.Stats;
            }
        }

        public StatModifier(StatModifier other, StatTable newTable)
        {
            statName = other.statName;
            modSourceStat = other.modSourceStat;
            type = other.type;
            ModValue = other.ModValue;
            ModSourceObject = other.ModSourceObject;
            if(ModSourceObject != null)
                modSourceTable = newTable;
        }

        public float ModValueWithTable()
        {
            return (modSourceTable == null || modSourceStat == null) ? ModValue
                    : modSourceTable.ModifiedValues[modSourceStat] * ModValue;
        }

        public static bool operator ==(StatModifier lhs, StatModifier rhs)
        {
            return lhs.statName == rhs.statName && lhs.modSourceStat == rhs.modSourceStat 
                && lhs.ModSourceObject == rhs.ModSourceObject && lhs.type == rhs.type;
        }

        public static bool operator !=(StatModifier lhs, StatModifier rhs)
        {
            return lhs.statName != rhs.statName || lhs.modSourceStat != rhs.modSourceStat 
                || lhs.ModSourceObject == rhs.ModSourceObject || lhs.type != rhs.type;
        }

        public bool Equals(StatModifier rhs)
        {
            return this == rhs;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StatModifier);
        }

        public override int GetHashCode()
        {
            return (statName + ModSourceObject + type.ToString()).GetHashCode();
        }

        /*public void UpdateSourceTable(StatTable oldTable, StatTable newTable)
        {
            if(modSourceTable == oldTable)
            {
                modSourceTable = newTable;
            }
        }*/

        public void UpdateSourceTable(GameObject modSourceObject_)
        {
            if (ModSourceObject == modSourceObject_.Archetype)
            {
                modSourceTable = modSourceObject_.Stats;
            }
        }

        public string ModSourceObject { get; set; }

        public float ModValue { get; set; }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public string statName;
        public string modSourceStat;
        public ModifierType type;

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private StatTable modSourceTable;
    }
}
