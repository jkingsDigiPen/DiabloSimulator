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
            btnExploreAttack.Click += btnExploreAttack_Click;
            btnDefend.Click += btnDefend_Click;
            btnUsePotion.Click += btnUsePotion_Click;

            // Add events to list box
            PopulateEvents();

            // Initialize other vars
            Turns = 0;

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

        // The number of turns taken so far in the game
        public uint Turns
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

            // Don't want our window to be able to get any smaller than this.
            SetValue(MinWidthProperty, this.Width);
            SetValue(MinHeightProperty, this.Height);
        }

        private void PopulateEvents()
        {
            lbEvents.Items.Add("Welcome to Sanctuary!");
            lbEvents.Items.Add("You are in the town of Tristram, a place of relative safety.");
        }

        private void btnExploreAttack_Click(object sender, RoutedEventArgs e)
        {
            ++Turns;
            lbEvents.Items.Add("Explore/attack was clicked. Turns taken: " + Turns + ".");
            svEvents.ScrollToBottom();

            viewModel.HeroStats.Level = viewModel.HeroStats.Level + 1;
            viewModel.HeroStats["CurrentHealth"] = viewModel.HeroStats.BaseValues["CurrentHealth"] - 10;
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {
            ++Turns;
            lbEvents.Items.Add("Defend was clicked. Turns taken: " + Turns + ".");
            svEvents.ScrollToBottom();

            viewModel.HeroStats["CurrentHealth"] = viewModel.HeroStats.BaseValues["CurrentHealth"] - 5;
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
