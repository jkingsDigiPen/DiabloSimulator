//------------------------------------------------------------------------------
//
// File Name:	MainWindow.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;

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

            // Event handlers
            btnNewGame.Click += btnNewGame_Click;
            btnContinue.Click += btnContinue_Click;
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

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            Window characterWindow = new CharacterCreationWindow(viewModel);
            characterWindow.Show();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lvCharacters.SelectedItem;

            if (selectedItem == null && lvCharacters.Items.Count != 0)
            {
                // Use first item if no items are selected
                lvCharacters.SelectedIndex = 0;
                selectedItem = lvCharacters.SelectedItem;
            }

            string selectedCharacter = selectedItem.ToString();
            viewModel.LoadGame(selectedCharacter);

            Window gameWindow = new GameWindow(viewModel);
            gameWindow.Show();
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private readonly ViewModel viewModel;
    }
}
