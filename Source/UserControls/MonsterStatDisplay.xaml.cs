using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DiabloSimulator.UserControls
{
    /// <summary>
    /// Interaction logic for MonsterStatDisplay.xaml
    /// </summary>
    public partial class MonsterStatDisplay : UserControl
    {
        public MonsterStatDisplay()
        {
            InitializeComponent();

            MonsterChanged += OnMonsterChanged;
        }

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

        private void OnMonsterChanged(object sender, RoutedEventArgs e)
        {
            tbMonsterName.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            var multiBind = BindingOperations.GetMultiBindingExpression(tbMonsterType, TextBlock.TextProperty);
            multiBind.UpdateTarget();
            tbMonsterHealth.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
    }
}
