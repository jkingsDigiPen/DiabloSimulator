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

        public StatModifier(string statName_, string modSource_, float modValue_, ModifierType type_)
        {
            statName = statName_;
            modSource = modSource_;
            modValue = modValue_;
            type = type_;
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
        public float modValue;
        public ModifierType type;
    }
}
