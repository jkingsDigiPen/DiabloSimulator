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

        // Create an object from an archetype.
        public abstract T Create(string name);

        // Create an object from an archetype using the hero's stats.
        public abstract T Create(string name, Hero hero);

        // Add an archetype/pattern from which to create objects.
        public abstract void AddArchetype(T archetype);

        //------------------------------------------------------------------------------
        // Protected Functions:
        //------------------------------------------------------------------------------

        // Loads all archetype from a file
        protected abstract void LoadArchetypesFromFile();

        // Save all archetypes to a file
        protected abstract void SaveArchetypesToFile();

        //------------------------------------------------------------------------------
        // Protected Variables:
        //------------------------------------------------------------------------------

        // Collection of patterns from which objects can be made.
        protected Dictionary<string, T> archetypes = new Dictionary<string, T>();
    }
}
