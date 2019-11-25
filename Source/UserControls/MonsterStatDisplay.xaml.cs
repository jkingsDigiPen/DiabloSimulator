using System.Windows;
using System.Windows.Controls;

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
        }

        // Allows events to reach other parts of UI
        public event RoutedPropertyChangedEventHandler<string> MonsterChanged
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
    }
}
