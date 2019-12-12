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

            liMainHand.MouseDoubleClick += liMainHand_DoubleClick;
            liOffHand.MouseDoubleClick += liOffHand_DoubleClick;
            liHead.MouseDoubleClick += liHead_DoubleClick;
            liTorso.MouseDoubleClick += liTorso_DoubleClick;
            liLegs.MouseDoubleClick += liLegs_DoubleClick;
            liWaist.MouseDoubleClick += liWaist_DoubleClick;
            liGloves.MouseDoubleClick += liGloves_DoubleClick;
            liFeet.MouseDoubleClick += liFeet_DoubleClick;
            liAmulet.MouseDoubleClick += liAmulet_DoubleClick;
            liRing1.MouseDoubleClick += liRing1_DoubleClick;
            liRing2.MouseDoubleClick += liRing2_DoubleClick;
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region eventHandlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void liMainHand_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item unequipped = View.Hero.UnequipItem(SlotType.MainHand);

            if (unequipped is null)
                return;

            if (unequipped.slot == SlotType.BothHands)
                View.Hero.UnequipItem(SlotType.OffHand);
        }

        private void liOffHand_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item unequipped = View.Hero.UnequipItem(SlotType.OffHand);

            if (unequipped is null)
                return;

            if (unequipped.slot == SlotType.BothHands)
                View.Hero.UnequipItem(SlotType.MainHand);
        }

        private void liTorso_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Torso);
        }

        private void liWaist_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Waist);
        }

        private void liLegs_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Legs);
        }

        private void liGloves_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Hands);
        }

        private void liFeet_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Feet);
        }

        private void liHead_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Head);
        }

        private void liAmulet_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Amulet);
        }

        private void liRing1_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Ring1);
        }

        private void liRing2_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            View.Hero.UnequipItem(SlotType.Ring2);
        }

        #endregion

        #region properties

        private EquipmentViewModel View
        {
            get => (DataContext as EquipmentViewModel);
        }

        #endregion
    }
}
