//------------------------------------------------------------------------------
//
// File Name:	MainWindow.xaml.cs
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
    }
}
