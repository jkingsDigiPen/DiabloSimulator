//------------------------------------------------------------------------------
//
// File Name:	WorldViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using DiabloSimulator.Game.World;
using DiabloSimulator.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DiabloSimulator.ViewModel
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class WorldViewModel : INotifyPropertyChanged
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldViewModel()
        {
            gameManager = EngineCore.GetModule<GameManager>();
            worldEventManager = EngineCore.GetModule<WorldEventManager>();
            heroManager = EngineCore.GetModule<HeroManager>();
            monsterManager = EngineCore.GetModule<MonsterManager>();

            wasInCombat = false;
            ChoiceText = new PlayerChoiceText("Explore", "Rest", "Town Portal");
        }

        #region actors

        public Hero Hero { get => heroManager.Hero; }

        public void CreateHero()
        {
            EngineCore.RaiseGameEvent(this, GameEvents.HeroCreate);
        }

        public Monster Monster { get => monsterManager.Monster; }

        #endregion

        #region miscGameFunctions

        public bool InCombat
        {
            get
            {
                if (wasInCombat != gameManager.InCombat)
                {
                    wasInCombat = gameManager.InCombat;
                    OnPropertyChange("InCombat");
                }

                return gameManager.InCombat;
            }
        }

        #endregion

        #region playerActions

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Ignore commands during combat
            if (InCombat)
                return;

            // Ctrl S to save
            if (e.Key == Key.S && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                MessageBoxResult result = MessageBox.Show("Saving your game. This will overwrite your current save for this character.",
                    "Diablo Simulator", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    EngineCore.RaiseGameEvent(sender, GameEvents.GameSave);
                }
            }
        }

        public void OnChoice01Clicked(object sender, RoutedEventArgs e)
        {
            if (InCombat)
            {
                AddWorldEvent(GameEvents.PlayerAttack);
            }
            else if (!Hero.IsDead)
            {
                if (ChoiceText == GameManager.exploreChoiceText)
                {
                    AddWorldEvent(GameEvents.PlayerExplore);
                }
                else if (ChoiceText == GameManager.discoverChoiceText)
                {
                    AddWorldEvent(GameEvents.PlayerProceed);
                }
            }
        }

        public void OnChoice02Clicked(object sender, RoutedEventArgs e)
        {
            if (InCombat)
            {
                AddWorldEvent(GameEvents.PlayerDefend);
            }
            else if (!Hero.IsDead)
            {
                if (ChoiceText == GameManager.exploreChoiceText)
                {
                    AddWorldEvent(GameEvents.PlayerRest);
                }
                else if (ChoiceText == GameManager.discoverChoiceText)
                {
                    AddWorldEvent(GameEvents.PlayerBack);
                }
            }
        }

        public void OnChoice03Clicked(object sender, RoutedEventArgs e)
        {
            if (InCombat)
            {
                AddWorldEvent(GameEvents.PlayerFlee);
            }
            else
            {
                AddWorldEvent(GameEvents.PlayerTownPortal);
            }
        }

        public void OnUsePotionClicked(object sender, RoutedEventArgs e)
        {
            EngineCore.RaiseGameEvent(sender, GameEvents.PlayerUsePotion);
        }

        public PlayerChoiceText ChoiceText
        {
            get => choiceText;
            set
            {
                choiceText = value;
                OnPropertyChange("ChoiceText");
            }
        }

        #endregion

        #region saveLoad

        public void OnGameLoaded(object sender, RoutedEventArgs e)
        {
            AddWorldEvent(GameEvents.GameStart);
        }

        public void OnQuitGame(object sender, CancelEventArgs e)
        {
            MessageBoxResult result;

            if (InCombat)
            {
                result = MessageBox.Show("WARNING: Game cannot be saved during combat. " +
                    "Unsaved data will be lost upon exit.",
                    "Diablo Simulator", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                result = MessageBox.Show("Would you like to save your game before quitting?",
                    "Diablo Simulator", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    EngineCore.RaiseGameEvent(sender, GameEvents.GameSave);
                }
            }

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        public bool CanLoadGame
        {
            get => heroManager.CanLoadState;
        }

        public List<string> SavedCharacters
        {
            get => heroManager.SavedCharacters;
        }

        public void LoadGame(string saveFileName)
        {
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.GameLoad, saveFileName));
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public ObservableCollection<string> WorldEventLog { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region privateFunctions

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddWorldEvent(GameEvents action)
        {
            // No hero? No point in being here.
            if (heroManager == null)
                return;

            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(action, heroManager.Hero));
            ChoiceText = gameManager.CurrentChoiceText;

            // Add to list view
            foreach (string eventString in worldEventManager.WorldEventsText)
            {
                WorldEventLog.Add(eventString);
            }

            // TO DO: Figure out way to correctly propagate monster change
            if (InCombat)
                Application.Current.Dispatcher?.Invoke(() =>
                (Application.Current.MainWindow as GameWindow)
                .ctrlMonster.OnMonsterChanged(this, null));
        }

        #endregion

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private bool wasInCombat;
        private PlayerChoiceText choiceText;

        // Modules
        private HeroManager heroManager;
        private MonsterManager monsterManager;
        private GameManager gameManager;
        private WorldEventManager worldEventManager;
    }
}
