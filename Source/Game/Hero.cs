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

    public class Hero : GameObject
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Hero(string name_ = "", string heroClass = "Warrior")
            : base(name_, heroClass)
        {
            inventory = new Inventory();
            equipment = new Equipment();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public static Hero current = new Hero();

        public Inventory inventory;
        public Equipment equipment;
    }
}
