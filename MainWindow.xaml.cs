using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiabloSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnStart.Click += btnStart_Click;
            rbClassWarrior.Click += rbClassWarrior_Click;
            rbClassRogue.Click += rbClassRogue_Click;
            rbClassSorcerer.Click += rbClassSorcerer_Click;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Hero.current.name = tbxHeroName.Text;
            MessageBox.Show("Abandon all hope, all ye who enter here.", "Notification");
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
