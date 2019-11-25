//------------------------------------------------------------------------------
//
// File Name:	WorldEventDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            AddWorldEvent("Welcome to Sanctuary!", false);
            AddWorldEvent("You are in the town of Tristram, a place of relative safety.", false);
        }

        private void btnExploreAttack_Click(object sender, RoutedEventArgs e)
        {
            AddWorldEvent("Explore/attack was clicked. Turns taken: " + Turns + ".");

            // TO DO: Remove this
            View.HeroStats.Level = View.HeroStats.Level + 1;
            View.HeroStats["CurrentHealth"] = View.HeroStats.BaseValues["CurrentHealth"] - 10;
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {
            AddWorldEvent("Defend was clicked. Turns taken: " + Turns + ".");

            // TO DO: Remove this
            View.HeroStats["CurrentHealth"] = View.HeroStats.BaseValues["CurrentHealth"] - 5;
        }

        private void AddWorldEvent(string worldEvent, bool advanceTime = true)
        {
            if (advanceTime)
            {
                AdvanceTime();
            }

            lvEvents.Items.Add(worldEvent);
            lvEvents.Items.MoveCurrentToLast();
            lvEvents.ScrollIntoView(lvEvents.Items.CurrentItem);
        }

        private void AdvanceTime()
        {
            ++Turns;

            // Check for player death
            if (View.HeroStats.ModifiedValues["CurrentHealth"] == 0)
            {
                AddWorldEvent(View.HeroName + " has died!");
            }
            // Regen
            else
            {
                View.HealHero(View.HeroStats.ModifiedValues["HealthRegen"]);
            }
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
