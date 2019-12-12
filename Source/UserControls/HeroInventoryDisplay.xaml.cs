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
using DiabloSimulator.ViewModel;

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

            // Create view model
            DataContext = new InventoryViewModel();

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
        }

        private void btnItemEquip_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            View.ItemEquip(selection);
        }

        private void btnItemDiscardSell_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            View.ItemDiscardSell(selection);

            // TO DO: Implement selling items when in town

        }

        private void btnItemJunk_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;
            
            View.ItemJunk(selection);
            lbInventory.SelectedIndex = selection;
        }

        private void btnItemKeep_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            View.ItemKeep(selection);
            lbInventory.SelectedIndex = selection;
        }

        #endregion

        #region properties

        private InventoryViewModel View
        {
            get => (DataContext as InventoryViewModel);
        }

        #endregion
    }
}
