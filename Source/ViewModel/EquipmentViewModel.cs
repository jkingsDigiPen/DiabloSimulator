//------------------------------------------------------------------------------
//
// File Name:	EquipmentViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DiabloSimulator.ViewModel
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class EquipmentViewModel
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public EquipmentViewModel()
        {
            heroManager = EngineCore.GetModule<HeroManager>();
        }

        public void OnMainHandDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.MainHand);
        }

        public void OnOffHandDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.OffHand);
        }

        public void OnTorsoDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Torso);
        }

        public void OnWaistDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Waist);
        }

        public void OnLegsDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Legs);
        }

        public void OnGlovesDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Hands);
        }

        public void OnFeetDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Feet);
        }

        public void OnHeadDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Head);
        }

        public void OnAmuletDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Amulet);
        }

        public void OnRing1DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Ring1);
        }

        public void OnRing2DoubleClick(object sender, MouseButtonEventArgs e)
        {
            UnequipItem(SlotType.Ring2);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public ObservableDictionaryNoThrow<SlotType, Item> Equipment { get => heroManager.Hero.Equipment.Items; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void UnequipItem(SlotType slot)
        {
            EngineCore.RaiseGameEvent(this, GameEventArgs.Create(GameEvents.ItemUnequip, slot));
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Modules
        private HeroManager heroManager;
    }
}
