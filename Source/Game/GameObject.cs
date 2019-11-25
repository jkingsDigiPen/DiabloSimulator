//------------------------------------------------------------------------------
//
// File Name:	GameObject.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public abstract class GameObject
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public GameObject(string name_, string archetype_ = "")
        {
            Name = name_;
            Archetype = archetype_;
            stats = new StatTable();
        }

        public string Name { get; set; }
        public string Archetype { get; set; }
        public string Description { get; set; }

        public StatTable stats;
    }
}
