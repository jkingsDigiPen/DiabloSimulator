//------------------------------------------------------------------------------
//
// File Name:	CharacterCreationWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;

namespace DiabloSimulator.Windows
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class CharacterCreationWindow : Window
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public CharacterCreationWindow(ViewModel view)
        {
            InitializeComponent();

            // Create view model
            viewModel = view;
            DataContext = viewModel;

            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = this;

            // Stock hero description
            viewModel.Hero.Description = "A wandering adventurer, seeking fame and fortune.";

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
            if (viewModel.Hero.Name.Trim() != "")
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
            viewModel.Hero.Archetype = (sender as RadioButton).Content.ToString();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
