using DiabloSimulator.Engine;
using DiabloSimulator.Game.World;
using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    class MonsterManager : IModule
    {
        public override void Inintialize()
        {
            gameManager = EngineCore.GetModule<GameManager>();
            heroManager = EngineCore.GetModule<HeroManager>();
            zoneManager = EngineCore.GetModule<ZoneManager>();
        }

        public void CreateMonster(string name = null)
        {
            // Create specific monster
            if(name != null)
            {
                Monster = monsterFactory.Create(name, heroManager.Hero);
            }
            // Create monster using monster table
            else 
            {
                Monster = zoneManager.CurrentZone.MonsterTable.GenerateObject(
                    heroManager.Hero, monsterFactory);
            }
        }

        public void DestroyMonster()
        {
            Monster.Kill();
            Monster = new Monster();
            gameManager.InCombat = false;
        }

        public string DamageMonster(float amount)
        {
            var damageList = new List<DamageArgs>();
            damageList.Add(new DamageArgs(amount));
            return Monster.Damage(damageList);
        }

        public Monster Monster { get; set; } = new Monster();

        private MonsterFactory monsterFactory = new MonsterFactory();

        // Modules
        GameManager gameManager;
        HeroManager heroManager;
        ZoneManager zoneManager;
    }
}
