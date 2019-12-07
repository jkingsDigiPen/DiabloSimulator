using DiabloSimulator.Game;
using DiabloSimulator.Game.World;
using System;
using System.Collections.Generic;

namespace DiabloSimulator.Engine
{
    public static class EngineCore
    {
        static EngineCore()
        {
            AddModule(new ConsoleManager());
            AddModule(new AudioManager());
            AddModule(new GameManager());
            AddModule(new ZoneManager());
            AddModule(new WorldEventManager());
            AddModule(new HeroManager());
            AddModule(new MonsterManager());

            foreach(IModule module in modules)
            {
                module.Inintialize();
            }
        }

        public static T GetModule<T>() where T : IModule
        {
            foreach(IModule module in modules)
            {
                if(module.GetType() == typeof(T))
                {
                    return module as T;
                }
            }

            return null;
        }

        public static void AddModule(IModule module)
        {
            modules.Add(module);
        }

        public static void RaiseGameEvent(object sender, GameEventArgs e)
        {
            foreach(IModule module in modules)
            {
                module.OnGameEvent(sender, e);
            }
        }

        private static List<IModule> modules = new List<IModule>();
    }
}
