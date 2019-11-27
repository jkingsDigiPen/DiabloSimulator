//------------------------------------------------------------------------------
//
// File Name:	GameObject.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.ComponentModel;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public abstract class GameObject : INotifyPropertyChanged
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

        public string Name
        {
            get => Name;
            set
            {
                if (Name != value)
                {
                    Name = value;
                    OnPropertyChange("Name");
                }
            }
        }

        public string Archetype 
        {
            get => Archetype; 
            set
            {
                if (Archetype != value)
                {
                    Archetype = value;
                    OnPropertyChange("Archetype");
                }
            }
        }

        public string Description 
        { 
            get => Description; 
            set
            {
                if (Description != value)
                {
                    Description = value;
                    OnPropertyChange("Description");
                }
            }
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public StatTable stats;

        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}
