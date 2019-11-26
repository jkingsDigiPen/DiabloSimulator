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

        private void Window_Loaded(object sender, RoutedEventArgs args)
        {
            // We know longer need to size to the contents.
            ClearValue(SizeToContentProperty);

            // Don't want our window to be able to get any smaller than this.
            SetValue(MinWidthProperty, this.Width);
            SetValue(MinHeightProperty, this.Height);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // Check to make sure form is filled
            if(viewModel.HeroName.Trim() != "")
            {
                // We have everything we need to create the hero
                viewModel.CreateHero();

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

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
