using DiabloSimulator.Engine;
using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    class MonsterManager : IModule
    {
        public void Inintialize()
        {
            throw new System.NotImplementedException();
        }

        public void DestroyMonster()
        {
            Monster.Kill();
            Monster = new Monster();
            InCombat = false;
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Monster.Damage(damageList);
        }

        public Monster Monster { get; set; } = new Monster();

        private MonsterFactory monsterFactory = new MonsterFactory();
    }
}
