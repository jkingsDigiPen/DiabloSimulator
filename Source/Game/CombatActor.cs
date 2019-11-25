//------------------------------------------------------------------------------
//
// File Name:	CombatActor.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

namespace DiabloSimulator.Game
{
    interface CombatActor
    {
        public void Heal(float amount);

        public void Damage(float amount);

        public float GetAttackDamage();

        public void Kill();

        public void Revive();
    }
}
