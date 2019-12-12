//------------------------------------------------------------------------------
//
// File Name:	HeroHealthDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.ViewModel;
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
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Register event handlers
            btnUsePotion.Click += View.OnUsePotionClicked;
        }

        private WorldViewModel View
        {
            get => (DataContext as WorldViewModel);
        }
    }
}
