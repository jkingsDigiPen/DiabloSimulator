//------------------------------------------------------------------------------
//
// File Name:	HeroHealthDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

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
        }

        private void btnUsePotion_Click(object sender, RoutedEventArgs e)
        {
            View.Hero.UsePotion();
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
