using System;
using System.Windows;
using System.Windows.Controls;

namespace DiabloSimulator.UserControls
{
    /// <summary>
    /// Interaction logic for MonsterStatDisplay.xaml
    /// </summary>
    public partial class MonsterStatDisplay : UserControl
    {
        // Allows events to reach other parts of UI
        public event RoutedEventHandler MonsterChanged
        {
            add
            {
                AddHandler(WorldEventDisplay.MonsterChangedEvent, value);
            }
            remove
            {
                RemoveHandler(WorldEventDisplay.MonsterChangedEvent, value);
            }
        }

        public MonsterStatDisplay()
        {
            InitializeComponent();

            MonsterChanged += OnMonsterChanged;
        }

        private void OnMonsterChanged(object sender, RoutedEventArgs e)
        {
            tbMonsterName.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            //tbMonsterType.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            tbMonsterHealth.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
    }
}
