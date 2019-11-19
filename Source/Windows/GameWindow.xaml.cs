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
        }
    }
}
