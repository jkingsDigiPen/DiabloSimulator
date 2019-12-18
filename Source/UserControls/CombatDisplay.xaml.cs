﻿//------------------------------------------------------------------------------
//
// File Name:	CombatDisplay.xaml.cs
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

    public partial class CombatDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public CombatDisplay()
        {
            InitializeComponent();

            // Create view model
            DataContext = new CombatViewModel();
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region eventHandlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region properties

        private CombatViewModel View
        {
            get => (DataContext as CombatViewModel);
        }

        #endregion
    }
}