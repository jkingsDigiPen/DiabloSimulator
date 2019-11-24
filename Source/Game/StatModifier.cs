//------------------------------------------------------------------------------
//
// File Name:	StatModifier.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

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
        public StatModifier(string statName_, string modSource_,
            ModifierType type_, float modValue_, StatTable modSourceTable_ = null)
        {
            statName = statName_;
            modSource = modSource_;
            type = type_;

            modValue = modValue_;
            modSourceTable = modSourceTable_;
        }

        // Retrieve modifier value
        public float ModValue
        {
            get { return modSourceTable == null ? modValue : modSourceTable.ModifiedValues[modSource] * modValue; }
        }

        public static bool operator ==(StatModifier lhs, StatModifier rhs)
        {
            return lhs.statName == rhs.statName && lhs.modSource == rhs.modSource && lhs.type == rhs.type;
        }

        public static bool operator !=(StatModifier lhs, StatModifier rhs)
        {
            return lhs.statName != rhs.statName || lhs.modSource != rhs.modSource || lhs.type != rhs.type;
        }

        public bool Equals(StatModifier rhs)
        {
            return statName == rhs.statName && modSource == rhs.modSource && type == rhs.type;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as StatModifier);
        }

        public override int GetHashCode()
        {
            return (statName + modSource + type.ToString()).GetHashCode();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public string statName;
        public string modSource;
        public ModifierType type;

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private float modValue;
        private StatTable modSourceTable;
    }
}
