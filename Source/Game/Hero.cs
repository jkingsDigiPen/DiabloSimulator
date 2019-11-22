//------------------------------------------------------------------------------
//
// File Name:	Hero.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public enum HeroClass
    {
        Warrior,
        Rogue,
        Sorcerer,
    }

    public class Hero : GameObject
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Hero(string name_ = "", HeroClass heroClass_ = HeroClass.Warrior)
            : base(name_, heroClass_.ToString())
        {
            heroClass = heroClass_;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public HeroClass heroClass;

        public static Hero current = new Hero();
    }
}
