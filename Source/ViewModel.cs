//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Game;
using System.Collections.Generic;
using System.ComponentModel;

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
        }

        #region actors

        public Hero Hero { get => gameManager.Hero; }

        public void CreateHero()
        {
            gameManager.CreateHero();
        }

        public Monster Monster { get => gameManager.Monster; }

        #endregion

        #region gameFunctions

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

        public string GetActionResult(PlayerActionType actionType)
        {
            return gameManager.GetActionResult(new PlayerAction(actionType));
        }

        public string GetActionResult(PlayerAction action)
        {
            return gameManager.GetActionResult(action);
        }

        #endregion

        #region saveLoad

        public bool CanLoadGame
        {
            get => gameManager.CanLoadState;
        }

        public List<string> SavedCharacters
        {
            get => gameManager.SavedCharacters;
        }

        public void SaveGame()
        {
            gameManager.SaveState();
        } 

        public void LoadGame(string saveFileName)
        {
            gameManager.LoadState(saveFileName);
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private GameManager gameManager;
        private bool wasInCombat;
    }
}
