//------------------------------------------------------------------------------
//
// File Name:	ViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Game;
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

        #region gameState

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

        // TO DO: Remove these functions
        #region testingFunctions
            
        public void AddItemToInventory(string name)
        {
            Hero.Inventory.AddItem(gameManager.CreateItem(name));
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
