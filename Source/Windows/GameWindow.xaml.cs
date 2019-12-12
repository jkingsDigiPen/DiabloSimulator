//------------------------------------------------------------------------------
//
// File Name:	GameWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DiabloSimulator.Windows
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class GameWindow : Window
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public GameWindow(WorldViewModel viewModel_)
        {
            viewModel = viewModel_;
            DataContext = viewModel;
            InitializeComponent();

            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = this;

            // Register events
            KeyDown += new KeyEventHandler(viewModel.OnKeyDown);
            Closing += new CancelEventHandler(viewModel.OnQuitGame);
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // We know longer need to size to the contents.
            ClearValue(SizeToContentProperty);

            // We want our control to shrink/expand with the window.
            ctrlStats.ClearValue(WidthProperty);
            ctrlStats.ClearValue(HeightProperty);
            ctrlEquipment.ClearValue(WidthProperty);
            ctrlEquipment.ClearValue(HeightProperty);
            ctrlInventory.ClearValue(WidthProperty);
            ctrlInventory.ClearValue(HeightProperty);
            ctrlEvents.ClearValue(WidthProperty);
            ctrlEvents.ClearValue(HeightProperty);
            ctrlHealth.ClearValue(WidthProperty);
            ctrlHealth.ClearValue(HeightProperty);
            ctrlEvents.ClearValue(WidthProperty);
            ctrlEvents.ClearValue(HeightProperty);
            ctrlWeapon.ClearValue(WidthProperty);
            ctrlWeapon.ClearValue(HeightProperty);
            ctrlMonster.ClearValue(WidthProperty);
            ctrlMonster.ClearValue(HeightProperty);

            // Don't want our window to be able to get any smaller than this.
            SetValue(MinWidthProperty, Width);
            SetValue(MinHeightProperty, Height);
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly WorldViewModel viewModel;
    }
}
