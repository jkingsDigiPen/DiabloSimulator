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

        public GameEventArgs(Game.GameEvents name)
        {
            Name = name;
        }

        public GameEventArgs(Game.GameEvents name, float value)
        {
            Name = name;
            Args.Add(value);
        }

        public static GameEventArgs Create<T>(Game.GameEvents name, T value)
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

        public Game.GameEvents Name { get; private set; }

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

        public void RaiseGameEvent(Game.GameEvents eventName)
        {
            RaiseGameEvent(eventName, this);
        }

        public void RaiseGameEvent(Game.GameEvents eventName, object sender)
        {
            RaiseGameEvent(new GameEventArgs(eventName), sender);
        }

        public void RaiseGameEvent<T>(Game.GameEvents eventName, object sender, T arg0)
        {
            GameEventArgs e = GameEventArgs.Create(eventName, arg0);
            RaiseGameEvent(e, sender);
        }

        public void AddEventHandler(Game.GameEvents eventType, GameEventHandler handler)
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

        private Dictionary<Game.GameEvents, GameEventHandler> EventHandlers { get; } 
            = new Dictionary<Game.GameEvents, GameEventHandler>();
    }
}
