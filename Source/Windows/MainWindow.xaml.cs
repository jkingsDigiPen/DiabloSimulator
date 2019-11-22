//------------------------------------------------------------------------------
//
// File Name:	MainWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using DiabloSimulator.Game;
using System.Collections.Generic;

namespace DiabloSimulator.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Hero.current.heroClass = HeroClass.Warrior;

            // Event handlers
            btnStart.Click += btnStart_Click;
            rbClassWarrior.Click += rbClassWarrior_Click;
            rbClassRogue.Click += rbClassRogue_Click;
            rbClassSorcerer.Click += rbClassSorcerer_Click;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Check to make sure form is filled
            if(tbxHeroName.Text.Trim() != "")
            {
                Hero.current.name = tbxHeroName.Text;
                PopulateHeroStats();
                Window gw = new GameWindow();
                gw.Show();
            }
            else
            {
                MessageBox.Show("Please enter a name for your character.", "Notification");
            }
        }

        private void rbClassWarrior_Click(object sender, RoutedEventArgs e)
        {
            Hero.current.heroClass = HeroClass.Warrior;
        }

        private void rbClassRogue_Click(object sender, RoutedEventArgs e)
        {
            Hero.current.heroClass = HeroClass.Rogue;
        }

        private void rbClassSorcerer_Click(object sender, RoutedEventArgs e)
        {
            Hero.current.heroClass = HeroClass.Sorcerer;
        }

        private void PopulateHeroStats()
        {
            Dictionary<string, float> values = new Dictionary<string, float>();
            Dictionary<string, float> progression = new Dictionary<string, float>();

            switch (Hero.current.heroClass)
            {
                case HeroClass.Warrior:
                    // Set starting stats
                    values["Strength"] = 10;
                    values["Dexterity"] = 8;
                    values["Intelligence"] = 8;
                    values["Vitality"] = 9;

                    // Set stat progression
                    progression["Strength"] = 3;
                    progression["Dexterity"] = 1;
                    progression["Intelligence"] = 1;
                    progression["Vitality"] = 2;
                    break;

                case HeroClass.Rogue:
                    // Set starting stats
                    values["Strength"] = 8;
                    values["Dexterity"] = 10;
                    values["Intelligence"] = 8;
                    values["Vitality"] = 9;

                    // Set stat progression
                    progression["Strength"] = 1;
                    progression["Dexterity"] = 3;
                    progression["Intelligence"] = 1;
                    progression["Vitality"] = 2;
                    break;

                case HeroClass.Sorcerer:
                    // Set starting stats
                    values["Strength"] = 8;
                    values["Dexterity"] = 8;
                    values["Intelligence"] = 10;
                    values["Vitality"] = 9;

                    // Set stat progression
                    progression["Strength"] = 1;
                    progression["Dexterity"] = 1;
                    progression["Intelligence"] = 3;
                    progression["Vitality"] = 2;
                    break;
            }

            Hero.current.stats = new StatTable(1, values, progression);
        }
    }
}
