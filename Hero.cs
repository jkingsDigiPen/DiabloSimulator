using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloSimulator
{
    public enum HeroClass
    {
        Warrior,
        Rogue,
        Sorcerer,
    }

    public class Hero
    {
        public Hero(string name_ = "", HeroClass heroClass_ = HeroClass.Warrior)
        {
            name = name_;
            heroClass = heroClass_;
        }

        public string name;
        public HeroClass heroClass;

        public static Hero current;
    }
}
