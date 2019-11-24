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
            archetype = archetype_;
            stats = new StatTable();
        }

        public string Name { get; set; }

        public string archetype;
        public StatTable stats;
        public string description;
    }
}
