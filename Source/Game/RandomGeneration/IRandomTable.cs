//------------------------------------------------------------------------------
//
// File Name:	IRandomTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using System;

namespace DiabloSimulator.Game.RandomGeneration
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public abstract class IRandomTable<T>
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        // Generate an object based on the hero's state.
        public abstract T GenerateObject(Hero hero, IFactory<T> factory = null);

        //------------------------------------------------------------------------------
        // Protected Variables:
        //------------------------------------------------------------------------------

        protected Random random = new Random();
    }
}
