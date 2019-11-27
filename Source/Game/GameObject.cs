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

        public GameObject(GameObject other)
        {
            Name = other.Name;
            Archetype = other.Archetype;
            Description = other.Description;
            stats = new StatTable(other.stats);
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChange("Name");
                }
            }
        }

        public string Archetype 
        {
            get => archetype; 
            set
            {
                if (archetype != value)
                {
                    archetype = value;
                    OnPropertyChange("Archetype");
                }
            }
        }

        public string Description 
        { 
            get => description; 
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChange("Description");
                }
            }
        }

        public StatTable Stats { get => stats; }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

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

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private StatTable stats;
        private string name;
        private string archetype;
        private string description;
    }
}
