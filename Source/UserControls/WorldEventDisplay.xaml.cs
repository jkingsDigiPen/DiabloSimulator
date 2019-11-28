//------------------------------------------------------------------------------
//
// File Name:	WorldEventDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using DiabloSimulator.Game;

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

            // Event handlers
            btnExploreAttack.Click += btnExploreAttack_Click;
            btnDefend.Click += btnDefend_Click;
        }

        // Allows events to reach other parts of UI
        public static readonly RoutedEvent MonsterChangedEvent =
            EventManager.RegisterRoutedEvent("MonsterChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(WorldEventDisplay));

        // Allows events to reach other parts of UI
        public event RoutedEventHandler MonsterChanged
        {
            add
            {
                AddHandler(MonsterChangedEvent, value);
            }
            remove
            {
                RemoveHandler(MonsterChangedEvent, value);
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Add start text
            AddWorldEvent(PlayerActionType.Look);
        }

        private void btnExploreAttack_Click(object sender, RoutedEventArgs e)
        {
            if(View.InCombat)
            {
                AddWorldEvent(PlayerActionType.Attack);
            }
            else if(!View.Hero.IsDead())
            {
                AddWorldEvent(PlayerActionType.Explore);
            }
        }

        private void btnDefend_Click(object sender, RoutedEventArgs e)
        {
            if (View.InCombat)
            {
                AddWorldEvent(PlayerActionType.Defend);
            }
            else if (!View.Hero.IsDead())
            {
                AddWorldEvent(PlayerActionType.Rest);
            }
        }

        private void AddWorldEvent(PlayerActionType action)
        {
            string eventText = View.GetActionResult(action);

            // Remove last newline and carriage return
            eventText = eventText.Remove(eventText.Length - 2);

            // Add to list view
            lvEvents.Items.Add(eventText);

            // Scroll to bottom
            lvEvents.Items.MoveCurrentToLast();
            lvEvents.ScrollIntoView(lvEvents.Items.CurrentItem);

            // Force monster stat update
            if(View.InCombat)
                RaiseEvent(new RoutedEventArgs(MonsterChangedEvent));
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
