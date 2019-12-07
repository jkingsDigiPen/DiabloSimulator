using System;
using System.Collections;
using System.Collections.Generic;

namespace DiabloSimulator.Engine
{
    public class GameEventArgs : EventArgs
    {
        public GameEventArgs(string name, ArrayList args = null)
        {
            Name = name;
            if (args != null)
                Args = args;
        }

        public string Name { get; set; }
        public ArrayList Args { get; set; } = new ArrayList();
    }

    public abstract class IEventHandler
    {
        public delegate void GameEventHandler(object sender, GameEventArgs e);

        public void RaiseGameEvent(GameEventArgs e)
        {
            EngineCore.RaiseGameEvent(this, e);
        }

        public void AddEventHandler(string eventType, GameEventHandler handler)
        {
            EventHandlers[eventType] = handler;
        }

        public void OnGameEvent(object sender, GameEventArgs e)
        {
            foreach (var handler in EventHandlers)
            {
                if (handler.Key == e.Name)
                {
                    handler.Value(sender, e);
                }
            }
        }

        private Dictionary<string, GameEventHandler> EventHandlers { get; } = new Dictionary<string, GameEventHandler>();
    }
}
