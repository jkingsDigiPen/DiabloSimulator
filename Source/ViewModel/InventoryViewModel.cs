//------------------------------------------------------------------------------
//
// File Name:	InventoryViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using System.Collections.ObjectModel;

namespace DiabloSimulator.ViewModel
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class InventoryViewModel
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public InventoryViewModel()
        {
            heroManager = EngineCore.GetModule<HeroManager>();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public ObservableCollection<Item> Inventory { get => heroManager.Hero.Inventory.Items; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        public void ItemEquip(int selection)
        {
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.ItemEquip, selection));
        }

        public void ItemDiscardSell(int selection)
        {
            // Remove item from inventory
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.ItemDiscard, selection));

            // TO DO: Implement selling items when in town

        }

        public void ItemJunk(int selection)
        {
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.ItemJunk, selection));
        }

        public void ItemKeep(int selection)
        {
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.ItemKeep, selection));
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Modules
        private HeroManager heroManager;
    }
}
