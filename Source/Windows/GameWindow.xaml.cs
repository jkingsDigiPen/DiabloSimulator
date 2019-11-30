//------------------------------------------------------------------------------
//
// File Name:	GameWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

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

        public GameWindow(ViewModel viewModel_)
        {
            viewModel = viewModel_;
            DataContext = viewModel;
            InitializeComponent();

            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = this;

            // Register events
            ctrlEvents.MonsterChanged += ctrlEvents_MonsterChanged;
            KeyDown += new KeyEventHandler(Window_KeyDown);
            Closing += new CancelEventHandler(Window_Closing);
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
            SetValue(MinWidthProperty, this.Width);
            SetValue(MinHeightProperty, this.Height);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result;

            if(viewModel.InCombat)
            {
                result = MessageBox.Show("WARNING: Game cannot be saved during combat. " +
                    "Unsaved data will be lost upon exit.",
                    "Diablo Simulator", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                result = MessageBox.Show("Would you like to save your game before quitting?",
                    "Diablo Simulator", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    viewModel.SaveGame();
                }
            }

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Ignore commands during combat
            if (viewModel.InCombat)
                return;

            // Ctrl S to save
            if (e.Key == Key.S && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                MessageBoxResult result = MessageBox.Show("Saving your game. This will overwrite your current save for this character.", 
                    "Diablo Simulator", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if(result == MessageBoxResult.OK)
                {
                    viewModel.SaveGame();
                }
            }
        }

        private void ctrlEvents_MonsterChanged(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Derp");
            ctrlMonster.RaiseEvent(e);
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
