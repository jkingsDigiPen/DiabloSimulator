//------------------------------------------------------------------------------
//
// File Name:	WorldEventDisplay.xaml.cs
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

    public partial class WorldEventDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldEventDisplay()
        {
            InitializeComponent();

            // Event handlers
            btnExploreAttack.Click += btnExploreAttack_Click;
            btnDefend.Click += btnDefend_Click;

            // Misc setup
            Turns = 0;
            PopulateEvents();
        }

        // The number of turns taken so far in the game
        public uint Turns
        {
            get; set;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

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

            View.HeroStats.Level = View.HeroStats.Level + 1;
            View.HeroStats["CurrentHealth"] = View.HeroStats.BaseValues["CurrentHealth"] - 10;
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {
            ++Turns;
            lbEvents.Items.Add("Defend was clicked. Turns taken: " + Turns + ".");
            svEvents.ScrollToBottom();

            View.HeroStats["CurrentHealth"] = View.HeroStats.BaseValues["CurrentHealth"] - 5;
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
