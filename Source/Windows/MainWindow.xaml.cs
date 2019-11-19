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
            btnStart.Click += btnStart_Click;
            rbClassWarrior.Click += rbClassWarrior_Click;
            rbClassRogue.Click += rbClassRogue_Click;
            rbClassSorcerer.Click += rbClassSorcerer_Click;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Hero.current.name = tbxHeroName.Text;
            MessageBox.Show("Abandon all hope, all ye who enter here.", "Notification");
            Window gw = new GameWindow();
            gw.Show();
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
