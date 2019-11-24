//------------------------------------------------------------------------------
//
// File Name:	HeroInventoryDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using DiabloSimulator.Game;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class HeroInventoryDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroInventoryDisplay()
        {
            InitializeComponent();

            btnItemEquip.Click += btnItemEquip_Click;
            btnItemDiscardSell.Click += btnItemDiscardSell_Click;
            btnItemJunk.Click += btnItemJunk_Click;
            btnItemKeep.Click += btnItemKeep_Click;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void btnItemEquip_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            Inventory inventory = View.HeroInventory;
            Equipment equipment = View.HeroEquipment;

            Item itemToEquip = inventory.Items[selection];

            // Remove currently equipped item
            Item itemToRemove = equipment.UnequipItem(itemToEquip.slot);

            // Equip item in slot
            View.HeroEquipment.EquipItem(itemToEquip);

            // Remove item from inventory
            inventory.DiscardItem(itemToEquip);

            // Add unequipped item to inventory
            if(itemToRemove != null)
            {
                inventory.AddItem(itemToRemove);
            }
        }

        private void btnItemDiscardSell_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            Inventory inventory = View.HeroInventory;

            // Remove item from inventory
            Item itemToRemove = inventory.Items[selection];
            inventory.DiscardItem(itemToRemove);

            // TO DO: Implement selling items when in town
        }

        private void btnItemJunk_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;
            View.HeroInventory.JunkItem(selection);
            lbInventory.SelectedIndex = selection;
        }

        private void btnItemKeep_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;
            View.HeroInventory.KeepItem(selection);
            lbInventory.SelectedIndex = selection;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TO DO: REMOVE THIS
            Item testItem = new Item("Simple Dagger", SlotType.MainHand, ItemRarity.Common, "Dagger");
            testItem.stats["MinDamage"] = 2;
            testItem.stats["MaxDamage"] = 6;
            testItem.stats["RequiredLevel"] = 1;
            View.HeroInventory.AddItem(testItem);
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
