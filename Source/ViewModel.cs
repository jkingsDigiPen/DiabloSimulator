//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using DiabloSimulator.Game;

namespace DiabloSimulator
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class ViewModel : INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public ViewModel()
        {
            hero = new Hero("The Vagabond");
            heroStatList = new ObservableCollection<DataView>();

            heroStatList.Add(new StringRefView("Name", HeroGetName));
            heroStatList.Add(new StringRefView("Class", HeroGetArchetype));
            heroStatList.Add(new FloatRefView("Level", HeroGetLevel));
            heroStatList.Add(new DataView(""));

            heroStatList.Add(new DataView("Core Stats"));
            heroStatList.Add(new StatView("Strength", hero.stats));
            heroStatList.Add(new StatView("Dexterity", hero.stats));
            heroStatList.Add(new StatView("Intelligence", hero.stats));
            heroStatList.Add(new StatView("Vitality", hero.stats));
        }

        public string HeroName
        {
            get { return hero.name; }
            set 
            { 
                if(hero.name != value)
                {
                    hero.name = value;
                    OnPropertyChange("HeroName");
                }
            }
        }

        public string HeroClass
        {
            get { return hero.archetype;  }
            set
            {
                if(hero.archetype != value)
                {
                    hero.archetype = value;
                    OnPropertyChange("HeroClass");
                }
            }
        }

        public float HeroCurrentHealth
        {
            get { return hero.stats["CurrentHealth"]; }
            set
            {
                if (value <= hero.stats["MaxHealth"])
                {
                    hero.stats["CurrentHealth"] = value;
                    OnPropertyChange("HeroCurrentHealth");
                }
            }
        }

        public float HeroMaxHealth
        {
            get { return hero.stats["MaxHealth"]; }
            set
            {
                if (hero.stats.GetBaseValue("MaxHealth") != value)
                {
                    hero.stats["MaxHealth"] = value;
                    OnPropertyChange("HeroMaxHealth");
                }
            }
        }

        public uint HeroLevel
        {
            get { return hero.stats.Level; }
            set
            {
                if (hero.stats.Level != value)
                {
                    hero.stats.Level = value;
                    OnPropertyChange("HeroStatList");
                }
            }
        }

        public StatTable HeroStats
        {
            get { return hero.stats; }
        }

        public Inventory HeroInventory
        {
            get { return hero.inventory; }
        }

        public Equipment HeroEquipment
        {
            get { return hero.equipment; }
        }

        public int HeroPotions
        {
            get { return hero.inventory.potionsHeld; }
            set
            {
                if (value != hero.inventory.potionsHeld && value >= 0)
                {
                    hero.inventory.potionsHeld = value;
                    OnPropertyChange("HeroPotions");
                }
            }
        }

        public ObservableCollection<DataView> HeroStatList
        {
            get { return heroStatList; }
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Structures:
        //------------------------------------------------------------------------------

        public class DataView : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public DataView(string name_)
            {
                name = name_;
            }

            public override string ToString()
            {
                return name;
            }

            public string name;

            private void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private class FloatRefView : DataView
        {
            public delegate float GetFloatValue();

            public FloatRefView(string name_, GetFloatValue getValue_)
                : base(name_)
            {
                getValue = getValue_;
            }

            public override string ToString()
            {
                return name + ": " + getValue().ToString();
            }

            public GetFloatValue getValue;
        }

        private class StringRefView : DataView
        {
            public delegate string GetStringValue();

            public StringRefView(string name_, GetStringValue getValue_)
                : base(name_)
            {
                getValue = getValue_;
            }

            public override string ToString()
            {
                return name + ": " + getValue().ToString();
            }

            public GetStringValue getValue;
        }

        private class StatView : DataView
        {
            public StatView(string name_, StatTable stats_)
                : base(name_)
            {
                stats = stats_;
            }

            public StatTable stats;

            public override string ToString()
            {
                return name + ": " + stats[name];
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // HACK to get stat view working
        private string HeroGetName()
        {
            return HeroName;
        }

        private string HeroGetArchetype()
        {
            return HeroClass;
        }

        private float HeroGetLevel()
        {
            return HeroStats.Level;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private Hero hero;
        private ObservableCollection<DataView> heroStatList;
    }
}
