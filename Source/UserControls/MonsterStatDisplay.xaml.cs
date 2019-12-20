//------------------------------------------------------------------------------
//
// File Name:	MonsterStatDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class MonsterStatDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public MonsterStatDisplay()
        {
            InitializeComponent();

            // Create view model
            DataContext = new CombatViewModel();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        private CombatViewModel View
        {
            get => DataContext as CombatViewModel;
        }
    }
}
