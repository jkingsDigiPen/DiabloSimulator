//------------------------------------------------------------------------------
//
// File Name:	GameWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System;
using DiabloSimulator.Game;

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

            // Register event handlers
            btnUsePotion.Click += btnUsePotion_Click;

            // TO DO: REMOVE THIS
            viewModel.HeroPotions = 3;

            // TO DO: REMOVE THIS
            Item testItem = new Item("Simple Dagger", SlotType.Weapon1H, ItemRarity.Common, "Dagger");
            testItem.stats["MinDamage"] = 2;
            testItem.stats["MaxDamage"] = 6;
            testItem.stats["RequiredLevel"] = 1;
            viewModel.HeroInventory.AddItem(testItem);
        }

        // Whether we are in a combat event
        public bool InCombat
        {
            get; set;
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

            // Don't want our window to be able to get any smaller than this.
            SetValue(MinWidthProperty, this.Width);
            SetValue(MinHeightProperty, this.Height);
        }

        private void btnUsePotion_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.HeroPotions == 0)
                return;

            --viewModel.HeroPotions;

            // Increase health, but keep below max
            viewModel.HeroStats["CurrentHealth"] = Math.Min(viewModel.HeroStats.BaseValues["CurrentHealth"] + 50,
                0);
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
