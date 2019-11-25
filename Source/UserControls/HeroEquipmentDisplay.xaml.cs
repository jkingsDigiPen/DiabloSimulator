//------------------------------------------------------------------------------
//
// File Name:	HeroEquipmentDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DiabloSimulator.Game;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures
    //------------------------------------------------------------------------------

    using ModifierMap = Dictionary<ModifierType, HashSet<StatModifier>>;

    public partial class HeroEquipmentDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroEquipmentDisplay()
        {
            InitializeComponent();

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

        // Allows events to reach other parts of UI
        public event RoutedPropertyChangedEventHandler<string> EquipmentChanged
        {
            add
            {
                AddHandler(HeroInventoryDisplay.EquipmentChangedEvent, value);
            }
            remove
            {
                RemoveHandler(HeroInventoryDisplay.EquipmentChangedEvent, value);
            }
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
            Item unequipped = UnequipItem(SlotType.MainHand);

            if (unequipped is null)
                return;

            if (unequipped.slot == SlotType.BothHands)
                UnequipItem(SlotType.OffHand);
        }

        private void liOffHand_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Item unequipped = UnequipItem(SlotType.OffHand);

            if (unequipped is null)
                return;

            if (unequipped.slot == SlotType.BothHands)
                UnequipItem(SlotType.MainHand);
        }

        private void liTorso_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Torso);
        }

        private void liWaist_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Waist);
        }

        private void liLegs_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Legs);
        }

        private void liGloves_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Hands);
        }

        private void liFeet_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Feet);
        }

        private void liHead_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Head);
        }

        private void liAmulet_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Amulet);
        }

        private void liRing1_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Ring1);
        }

        private void liRing2_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Ring2);
        }

        #endregion

        #region helperFunctions

        private Item UnequipItem(SlotType slot)
        {
            // Unequip item
            Item unequipped = View.HeroEquipment.UnequipItem(slot);

            // Add to inventory
            if (unequipped is null)
                return null;

            View.HeroInventory.AddItem(unequipped);

            RemoveItemModsFromHeroStats(unequipped);

            return unequipped;
        }

        private void RemoveItemModsFromHeroStats(Item unequipped)
        {
            // Remove stats from item from hero stat mods
            foreach (KeyValuePair<string, float> mod in unequipped.stats.LeveledValues)
            {
                View.HeroStats.RemoveModifier(new StatModifier(mod.Key,
                    unequipped.Name, ModifierType.Additive, mod.Value));
            }

            foreach (KeyValuePair<string, ModifierMap> modMap in unequipped.stats.Modifiers)
            {
                foreach (KeyValuePair<ModifierType, HashSet<StatModifier>> modSet in modMap.Value)
                {
                    foreach (StatModifier mod in modSet.Value)
                    {
                        View.HeroStats.RemoveModifier(mod);
                    }
                }
            }
        }

        #endregion

        #region properties

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }

        #endregion
    }
}
