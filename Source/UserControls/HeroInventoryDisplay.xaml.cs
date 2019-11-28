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
            lbInventory.MouseDoubleClick += btnItemEquip_Click;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region eventHandlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try 
            {
                // TO DO: REMOVE THIS
                Item testItem = View.CreateItem("Simple Dagger");
                View.Hero.Inventory.AddItem(testItem);

                testItem = View.CreateItem("Short Sword");
                View.Hero.Inventory.AddItem(testItem);

                testItem = View.CreateItem("Leather Hood");
                View.Hero.Inventory.AddItem(testItem);
            }
            catch
            {
                Console.WriteLine("Bleargh?!");
            }
        }

        private void btnItemEquip_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            Inventory inventory = View.Hero.Inventory;
            Equipment equipment = View.Hero.Equipment;
            Item itemToEquip = inventory.Items[selection];

            // Don't equip junk items
            if (itemToEquip.junkStatus == JunkStatus.Junk)
                return;

            // TO DO: Handle rings

            // Remove currently equipped item
            Item itemToRemove = equipment.UnequipItem(itemToEquip.slot, View.Hero.Stats);

            // Equip item in slot
            equipment.EquipItem(itemToEquip, View.Hero.Stats);

            // Remove item from inventory
            inventory.RemoveItem(itemToEquip);

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

            Inventory inventory = View.Hero.Inventory;

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
            View.Hero.Inventory.JunkItem(selection);
            lbInventory.SelectedIndex = selection;
        }

        private void btnItemKeep_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;
            View.Hero.Inventory.KeepItem(selection);
            lbInventory.SelectedIndex = selection;
        }

        #endregion

        #region properties

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }

        #endregion
    }
}
