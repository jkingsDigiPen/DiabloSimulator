//------------------------------------------------------------------------------
//
// File Name:	CombatViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using DiabloSimulator.Windows;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace DiabloSimulator.ViewModel
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class CombatViewModel : INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public CombatViewModel()
        {
            monsterManager = EngineCore.GetModule<MonsterManager>();
            gameManager = EngineCore.GetModule<GameManager>();
            heroManager = EngineCore.GetModule<HeroManager>();

            gameManager.PropertyChanged += OnGameManagerPropertyChanged;
            monsterManager.PropertyChanged += OnMonsterManagerPropertyChanged;
        }

        public void SetMonsterSelected(int selectedIndex)
        {
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.MonsterSelected, selectedIndex));
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public ObservableCollection<Monster> MonsterList { get => monsterManager.MonsterList; }

        public Monster SelectedMonster { get => monsterManager.Monster; }

        public bool InCombat { get => gameManager.InCombat; }

        public int ExperienceNeeded { get => (int)heroManager.Hero.ExperienceNeeded; }

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnGameManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "InCombat")
                OnPropertyChange("InCombat");
        }

        private void OnMonsterManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Monster")
                OnPropertyChange("SelectedMonster");
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Modules
        private MonsterManager monsterManager;
        private GameManager gameManager;
        private HeroManager heroManager;
    }
}
