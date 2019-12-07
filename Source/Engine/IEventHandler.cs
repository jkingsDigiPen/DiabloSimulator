//------------------------------------------------------------------------------
//
// File Name:	IEventHandler.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace DiabloSimulator.Engine
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class GameEventArgs : EventArgs
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public GameEventArgs(string name)
        {
            Name = name;
        }

        public GameEventArgs(string name, float value)
        {
            Name = name;
            Args.Add(value);
        }

        public static GameEventArgs Create<T>(string name, T value)
        {
            GameEventArgs eventArgs = new GameEventArgs(name);
            eventArgs.Add(value);
            return eventArgs;
        }

        public T Get<T>(int index = 0)
        {
            return (T)Args[index];
        }

        public void Add<T>(T value)
        {
            Args.Add(value);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public string Name { get; private set; }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private ArrayList Args { get; set; } = new ArrayList();
    }

    public delegate void GameEventHandler(object sender, GameEventArgs e);

    public abstract class IEventHandler
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public void RaiseGameEvent(GameEventArgs e, object sender = null)
        {
            if (sender == null)
                sender = this;

            EngineCore.RaiseGameEvent(sender, e);
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

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Dictionary<string, GameEventHandler> EventHandlers { get; } = new Dictionary<string, GameEventHandler>();
    }
}
