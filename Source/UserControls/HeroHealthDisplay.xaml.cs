//------------------------------------------------------------------------------
//
// File Name:	HeroHealthDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class HeroHealthDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroHealthDisplay()
        {
            InitializeComponent();

            // Register event handlers
            btnUsePotion.Click += btnUsePotion_Click;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // TO DO: REMOVE THIS
                View.Hero.Inventory.PotionsHeld = 3;
            }
            catch
            {
                Console.WriteLine("Bleargh?!");
            }
        }

        private void btnUsePotion_Click(object sender, RoutedEventArgs e)
        {
            if (View.Hero.Inventory.PotionsHeld == 0)
                return;

            --View.Hero.Inventory.PotionsHeld;

            View.Hero.Heal(25);
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
