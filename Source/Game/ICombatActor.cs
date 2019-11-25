//------------------------------------------------------------------------------
//
// File Name:	CombatActor.cs
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

    public enum DamageType
    {
        Physical,
        Fire,
        Cold,
        Lightning,
        Poison,
        Holy,
        Arcane
    }

    public class DamageArgs
    {
        public DamageArgs(float amount_, DamageType damageType_ = DamageType.Physical)
        {
            amount = amount_;
            damageType = damageType_;
        }

        public float amount;
        public DamageType damageType;
    }

    public interface ICombatActor
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        // Takes in desired heal amount,
        // returns message about actual amount healed.
        public string Heal(float amount);

        // Takes in desired damage amount,
        // returns message about actual damage taken.
        public string Damage(List<DamageArgs> damageList);

        public List<DamageArgs> GetAttackDamage();

        public void Kill();

        public void Revive();

        public bool IsDead();
    }
}
