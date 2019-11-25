//------------------------------------------------------------------------------
//
// File Name:	IFactory.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DiabloSimulator.Game.Factories
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public abstract class IFactory<T>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        // Create an object based off information from the hero.
        public abstract T Create(Hero hero);

        // Add an archetype/pattern from which to create objects.
        public abstract void AddArchetype(T archetype);

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        // Makes an object by cloning the given archetype.
        protected abstract T CloneArchetype(string name);

        //------------------------------------------------------------------------------
        // Protected Variables:
        //------------------------------------------------------------------------------

        // Collection of patterns from which objects can be made.
        protected Dictionary<string, T> archetypes = new Dictionary<string, T>();
    }
}
