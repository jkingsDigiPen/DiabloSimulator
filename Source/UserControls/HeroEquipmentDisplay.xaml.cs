//------------------------------------------------------------------------------
//
// File Name:	HeroEquipmentDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DiabloSimulator.Game;
using DiabloSimulator.ViewModel;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures
    //------------------------------------------------------------------------------

    public partial class HeroEquipmentDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroEquipmentDisplay()
        {
            InitializeComponent();

            // Create view model
            DataContext = new EquipmentViewModel();

            liMainHand.MouseDoubleClick += View.OnMainHandDoubleClick;
            liOffHand.MouseDoubleClick += View.OnOffHandDoubleClick;
            liHead.MouseDoubleClick += View.OnHeadDoubleClick;
            liTorso.MouseDoubleClick += View.OnTorsoDoubleClick;
            liLegs.MouseDoubleClick += View.OnLegsDoubleClick;
            liWaist.MouseDoubleClick += View.OnWaistDoubleClick;
            liGloves.MouseDoubleClick += View.OnGlovesDoubleClick;
            liFeet.MouseDoubleClick += View.OnFeetDoubleClick;
            liAmulet.MouseDoubleClick += View.OnAmuletDoubleClick;
            liRing1.MouseDoubleClick += View.OnRing1DoubleClick;
            liRing2.MouseDoubleClick += View.OnRing2DoubleClick;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region properties

        private EquipmentViewModel View
        {
            get => (DataContext as EquipmentViewModel);
        }

        #endregion
    }
}
