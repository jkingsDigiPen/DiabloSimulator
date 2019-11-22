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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            App.Current.MainWindow.Close();
            App.Current.MainWindow = this;

            // Add stats to list box
            PopulateStats();

            // Add health information
            PopulateHealth();

            // Add equipment to list box
            PopulateEquipment();

            // Add inventory to list box
            PopulateInventory();

            // Add events to list box
            PopulateEvents();
        }

        private void PopulateStats()
        {
            ref var stats = ref Hero.current.stats;

            lbStats.Items.Add("Name: " + Hero.current.name);
            lbStats.Items.Add("Class: " + Hero.current.archetype);
            lbStats.Items.Add("Level: " + stats.Level);
            lbStats.Items.Add("");

            lbStats.Items.Add("Core Stats");
            lbStats.Items.Add("Strength: " + stats.GetModifiedValue("Strength"));
            lbStats.Items.Add("Dexterity: " + stats.GetModifiedValue("Dexterity"));
            lbStats.Items.Add("Intelligence: " + stats.GetModifiedValue("Intelligence"));
            lbStats.Items.Add("Vitality: " + stats.GetModifiedValue("Vitality"));
        }

        private void PopulateHealth()
        {
            ref var stats = ref Hero.current.stats;

            // Ensure current is not above max
            if (stats["CurrentHealth"] > stats["MaxHealth"])
                stats["CurrentHealth"] = stats["MaxHealth"];

            // Fill labels
            lblHealth.Content = stats["CurrentHealth"].ToString() 
                + " / " + stats["MaxHealth"].ToString();
        }

        private void PopulateEquipment()
        {
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

        private void PopulateInventory()
        {
            lbInventory.Items.Add("Buckler " + "(Off-Hand)");
            lbInventory.Items.Add("Cap " + "(Head)");
            lbInventory.Items.Add("Halcyon's Ascent " + "(Amulet)");

            lbInventory.Items.Add("Hammering Pants " + "(Pants)");
            lbInventory.Items.Add("Heavy Belt " + "(Belt)");
            lbInventory.Items.Add("Hellfire Ring " + "(Ring)");

            lbInventory.Items.Add("Leather Bots " + "(Boots)");
            lbInventory.Items.Add("Quilted Armor " + "(Chest)");
            lbInventory.Items.Add("Stone of Jordan " + "(Ring)");

            lbInventory.Items.Add("Thin Gloves " + "(Gloves)");
            lbInventory.Items.Add("Wirt's Leg " + "(Weapon)");  
        }

        private void PopulateEvents()
        {
            lbEvents.Items.Add("Welcome to Sanctuary!");
            lbEvents.Items.Add("You are in the town of Tristram, a place of relative safety.");
        }
    }
}
