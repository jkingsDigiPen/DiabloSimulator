//------------------------------------------------------------------------------
//
// File Name:	HeroInventoryDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

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
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TO DO: REMOVE THIS
            Item testItem = new Item("Simple Dagger", SlotType.Weapon1H, ItemRarity.Common, "Dagger");
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
