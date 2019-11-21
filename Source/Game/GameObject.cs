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

        public GameObject(string name_, string archetype_ = "", uint level_ = 1)
        {
            name = name_;
            archetype = archetype_;
            level = level_;
            stats = new StatTable();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public string name;
        public string archetype;
        public uint level;
        public StatTable stats;
    }
}
