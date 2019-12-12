//------------------------------------------------------------------------------
//
// File Name:	InventoryViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

        public InventoryViewModel(ListView lvInventory_)
        {
            heroManager = EngineCore.GetModule<HeroManager>();
            lvInventory = lvInventory_;
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Hero Hero { get => heroManager.Hero; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        public void btnItemEquip_Click(object sender, RoutedEventArgs e)
        {
            int selection = lvInventory.SelectedIndex;
            if (selection == -1)
                return;
            
            Hero.EquipItem(selection);
        }

        public void btnItemDiscardSell_Click(object sender, RoutedEventArgs e)
        {
            int selection = lvInventory.SelectedIndex;
            if (selection == -1)
                return;

            Inventory inventory = Hero.Inventory;

            // Remove item from inventory
            Item itemToRemove = inventory.Items[selection];
            inventory.DiscardItem(itemToRemove);

            // TO DO: Implement selling items when in town
        }

        public void btnItemJunk_Click(object sender, RoutedEventArgs e)
        {
            int selection = lvInventory.SelectedIndex;
            if (selection == -1)
                return;
            Hero.Inventory.JunkItem(selection);
            lvInventory.SelectedIndex = selection;
        }

        public void btnItemKeep_Click(object sender, RoutedEventArgs e)
        {
            int selection = lvInventory.SelectedIndex;
            if (selection == -1)
                return;
            Hero.Inventory.KeepItem(selection);
            lvInventory.SelectedIndex = selection;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Modules
        private HeroManager heroManager;
        private ListView lvInventory;
    }
}
