//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Game;
using DiabloSimulator.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
            gameManager = new GameManager();
            wasInCombat = false;
            ChoiceText = new PlayerChoiceText("Explore", "Rest", "Town Portal");
        }

        #region actors

        public Hero Hero { get => gameManager.Hero; }

        public void CreateHero()
        {
            gameManager.CreateHero();
        }

        public Monster Monster { get => gameManager.Monster; }

        #endregion

        #region miscGameFunctions

        public bool InCombat
        {
            get
            {
                if(wasInCombat != gameManager.InCombat)
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
                    gameManager.SaveState();
                }
            }
        }

        public void OnChoice01Clicked(object sender, RoutedEventArgs e)
        {
            if (InCombat)
            {
                AddWorldEvent(PlayerActionType.Attack);
            }
            else if (!Hero.IsDead())
            {
                AddWorldEvent(PlayerActionType.Explore);
            }
        }

        public void OnChoice02Clicked(object sender, RoutedEventArgs e)
        {
            if (InCombat)
            {
                AddWorldEvent(PlayerActionType.Defend);
            }
            else if (!Hero.IsDead())
            {
                AddWorldEvent(PlayerActionType.Rest);
            }
        }

        public void OnChoice03Clicked(object sender, RoutedEventArgs e)
        {
            if (InCombat)
            {
                AddWorldEvent(PlayerActionType.Flee);
            }
            else
            {
                AddWorldEvent(PlayerActionType.TownPortal);
            }
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
            AddWorldEvent(PlayerActionType.Look);
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
                    gameManager.SaveState();
                }
            }

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        public bool CanLoadGame
        {
            get => gameManager.CanLoadState;
        }

        public List<string> SavedCharacters
        {
            get => gameManager.SavedCharacters;
        }

        public void LoadGame(string saveFileName)
        {
            gameManager.LoadState(saveFileName);
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

        private void AddWorldEvent(PlayerActionType action)
        {
            string eventText = GetActionResult(action);

            // Remove last newline and carriage return
            eventText = eventText.Remove(eventText.Length - 2);

            // Add to list view
            WorldEventLog.Add(eventText);

            // TO DO: Figure out way to correctly propagate monster change
            if (InCombat)
                App.Current.Dispatcher?.Invoke(() =>
                (App.Current.MainWindow as GameWindow)
                .ctrlMonster.OnMonsterChanged(null, null));
        }

        private string GetActionResult(PlayerActionType actionType)
        {
            return GetActionResult(new PlayerAction(actionType));
        }

        private string GetActionResult(PlayerAction action)
        {
            PlayerActionResult result = gameManager.GetActionResult(action);
            ChoiceText = result.choiceText;

            return result.resultText;
        }

        #endregion

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private GameManager gameManager;
        private bool wasInCombat;
        private PlayerChoiceText choiceText;
    }
}
