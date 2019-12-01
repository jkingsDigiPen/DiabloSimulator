//------------------------------------------------------------------------------
//
// File Name:	MonsterStatDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class MonsterStatDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public MonsterStatDisplay()
        {
            InitializeComponent();
        }

        public void OnMonsterChanged(object sender, RoutedEventArgs e)
        {
            tbMonsterName.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            var multiBind = BindingOperations.GetMultiBindingExpression(tbMonsterType, TextBlock.TextProperty);
            multiBind.UpdateTarget();
            tbMonsterHealth.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
        }
    }
}
