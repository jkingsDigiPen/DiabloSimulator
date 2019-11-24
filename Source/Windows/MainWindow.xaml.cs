//------------------------------------------------------------------------------
//
// File Name:	MainWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using DiabloSimulator.Game;

namespace DiabloSimulator.Windows
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class MainWindow : Window
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public MainWindow()
        {
            InitializeComponent();

            // Create view model
            viewModel = new ViewModel();
            DataContext = viewModel;

            // Stock hero description
            viewModel.HeroDescription = "A wandering adventurer, seeking fame and fortune.";

            // Event handlers
            btnStart.Click += btnStart_Click;
            rbClassWarrior.Click += rbClass_Click;
            rbClassRogue.Click += rbClass_Click;
            rbClassSorcerer.Click += rbClass_Click;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Check to make sure form is filled
            if(viewModel.HeroName.Trim() != "")
            {
                PopulateHeroStats();
                Window gw = new GameWindow(viewModel);
                gw.Show();
            }
            else
            {
                MessageBox.Show("Please enter a name for your character.", "Notification");
            }
        }

        private void rbClass_Click(object sender, RoutedEventArgs e)
        {
            viewModel.HeroClass = (sender as RadioButton).Content.ToString();
        }

        private void PopulateHeroStats()
        {
            StatTable stats = viewModel.HeroStats;

            switch (viewModel.HeroClass)
            {
                case "Warrior":
                    // Set starting stats
                    stats["Strength"] = 10;
                    stats["Dexterity"] = 8;
                    stats["Intelligence"] = 8;
                    stats["Vitality"] = 9;

                    // Set stat progression
                    stats.SetProgression("Strength", 3);
                    stats.SetProgression("Dexterity", 1);
                    stats.SetProgression("Intelligence", 1);
                    stats.SetProgression("Vitality", 2);
                    break;

                case "Rogue":
                    // Set starting stats
                    stats["Strength"] = 8;
                    stats["Dexterity"] = 10;
                    stats["Intelligence"] = 8;
                    stats["Vitality"] = 9;

                    // Set stat progression
                    stats.SetProgression("Strength", 1);
                    stats.SetProgression("Dexterity", 3);
                    stats.SetProgression("Intelligence", 1);
                    stats.SetProgression("Vitality", 2);
                    break;

                case "Sorcerer":
                    // Set starting stats
                    stats["Strength"] = 8;
                    stats["Dexterity"] = 8;
                    stats["Intelligence"] = 10;
                    stats["Vitality"] = 9;

                    // Set stat progression
                    stats.SetProgression("Strength", 1);
                    stats.SetProgression("Dexterity", 1);
                    stats.SetProgression("Intelligence", 3);
                    stats.SetProgression("Vitality", 2);
                    break;
            }

            // Vitality gives 10 health per point
            stats["MaxHealth"] = 0;
            stats.AddModifier(new StatModifier("MaxHealth", "Vitality", 
                ModifierType.Additive, 10, stats));

            stats["CurrentHealth"] = 0;
            stats.AddModifier(new StatModifier("CurrentHealth", "MaxHealth",
                ModifierType.Additive, 1, stats));

            // Current health is initialized to max health
            //stats["CurrentHealth"] = stats["MaxHealth"];
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
