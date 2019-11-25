using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloSimulator.Game
{
    interface CombatActor
    {
        void Heal(float amount);

        void Damage(float amount);

        float GetAttackDamage();
    }
}
