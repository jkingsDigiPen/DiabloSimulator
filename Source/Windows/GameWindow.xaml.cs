//------------------------------------------------------------------------------
//
// File Name:	GameWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
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
            btnExploreAttack.Click += RefreshUI;
            btnDefend.Click += RefreshUI;
            btnUsePotion.Click += btnUsePotion_Click;
            btnItemEquip.Click += RefreshUI;
            btnItemJunk.Click += RefreshUI;
            btnItemKeep.Click += RefreshUI;
            btnItemDiscardSell.Click += RefreshUI;

            btnExploreAttack.Click += btnExploreAttack_Click;

            // Force UI refresh
            RefreshUI(null, null);

            // Add events to list box
            PopulateEvents();

            // Initialize other vars
            Turns = 0;

            // TO DO: REMOVE THIS
            viewModel.HeroPotions = 3;
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

        private void PopulateEquipment()
        {
            lbEquip0.Items.Clear();
            lbEquip1.Items.Clear();
            lbEquip2.Items.Clear();

            lbEquip0.Items.Add("");
            lbEquip1.Items.Add("Head: " + "Cap");
            lbEquip2.Items.Add("Amulet: " + "Halcyon's Ascent");

            lbEquip0.Items.Add("Gloves: " + "Thin Gloves");
            lbEquip1.Items.Add("Chest: " + "Quilted Armor");
            lbEquip2.Items.Add("Belt: " + "Heavy Belt");

            lbEquip0.Items.Add("Ring 1: " + "Stone of Jordan");
            lbEquip1.Items.Add("Pants: " + "Hammering Pants");
            lbEquip2.Items.Add("Ring 2: " + "Hellfire Ring");

            lbEquip0.Items.Add("Weapon: " + "Wirt's Leg");
            lbEquip1.Items.Add("Boots: " + "Leather Boots");
            lbEquip2.Items.Add("Off-Hand: " + "Buckler");
        }

        private void PopulateEvents()
        {
            lbEvents.Items.Add("Welcome to Sanctuary!");
            lbEvents.Items.Add("You are in the town of Tristram, a place of relative safety.");
        }

        // Refreshes dynamic elements of the UI
        private void RefreshUI(object sender, RoutedEventArgs e)
        {
            // Add equipment to list box
            PopulateEquipment();

            // Add inventory to list box
            viewModel.HeroInventory.Display(lbInventory);
        }

        private void btnExploreAttack_Click(object sender, RoutedEventArgs e)
        {
            ++Turns;
            lbEvents.Items.Add("Explore/attack was clicked. Turns taken: " + Turns + ".");
            svEvents.ScrollToBottom();

            ++viewModel.HeroLevel;
        }

        private void btnUsePotion_Click(object sender, RoutedEventArgs e)
        {
            --viewModel.HeroPotions;
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
