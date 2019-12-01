//------------------------------------------------------------------------------
//
// File Name:	WorldEventDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class WorldEventDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public WorldEventDisplay()
        {
            InitializeComponent();

            Loaded += OnControlLoaded;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            // Choices
            btnExploreAttack.Click += View.OnChoice01Clicked;
            btnDefend.Click += View.OnChoice02Clicked;
            btnFleeTown.Click += View.OnChoice03Clicked;

            // Force scrolling to end
            View.WorldEventLog.CollectionChanged += OnCollectionChanged;

            // Tell view we are ready to start
            View.OnGameLoaded(null, null);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (VisualTreeHelper.GetChildrenCount(lvEvents) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(lvEvents, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
