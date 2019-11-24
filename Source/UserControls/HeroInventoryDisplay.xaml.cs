//------------------------------------------------------------------------------
//
// File Name:	HeroInventoryDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DiabloSimulator.Game;

namespace DiabloSimulator.UserControls
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public partial class HeroInventoryDisplay : UserControl
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public HeroInventoryDisplay()
        {
            InitializeComponent();

            btnItemEquip.Click += btnItemEquip_Click;
            btnItemDiscardSell.Click += btnItemDiscardSell_Click;
            btnItemJunk.Click += btnItemJunk_Click;
            btnItemKeep.Click += btnItemKeep_Click;
            lbInventory.MouseDoubleClick += btnItemEquip_Click;
        }

        // Allows events to reach other parts of UI
        public static readonly RoutedEvent EquipmentChangedEvent =
            EventManager.RegisterRoutedEvent("EquipmentChanged", RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<string>), typeof(Control));

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TO DO: REMOVE THIS
            Item testItem = new Item("Simple Dagger", SlotType.MainHand, ItemRarity.Common, "Dagger");
            testItem.stats["MinDamage"] = 2;
            testItem.stats["MaxDamage"] = 6;
            testItem.stats["RequiredLevel"] = 1;
            View.HeroInventory.AddItem(testItem);

            testItem = new Item("Short Sword", SlotType.MainHand, ItemRarity.Common, "1-Handed Sword");
            testItem.stats["MinDamage"] = 1;
            testItem.stats["MaxDamage"] = 7;
            testItem.stats["RequiredLevel"] = 1;
            View.HeroInventory.AddItem(testItem);

            testItem = new Item("Leather Hood", SlotType.Head, ItemRarity.Magic, "Helm");
            testItem.stats["Armor"] = 21;
            testItem.stats["Vitality"] = 4;
            testItem.stats["RequiredLevel"] = 4;
            View.HeroInventory.AddItem(testItem);
        }

        private void btnItemEquip_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            Inventory inventory = View.HeroInventory;
            Equipment equipment = View.HeroEquipment;
            Item itemToEquip = inventory.Items[selection];

            // Don't equip junk items
            if (itemToEquip.junkStatus == JunkStatus.Junk)
                return;

            // TO DO: Handle rings

            // Remove currently equipped item
            Item itemToRemove = equipment.UnequipItem(itemToEquip.slot);

            // Equip item in slot
            View.HeroEquipment.EquipItem(itemToEquip);

            // Remove item from inventory
            inventory.RemoveItem(itemToEquip);

            // Add unequipped item to inventory
            if(itemToRemove != null)
            {
                inventory.AddItem(itemToRemove);
            }

            RaiseEvent(new RoutedEventArgs(EquipmentChangedEvent));
        }

        private void btnItemDiscardSell_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;

            Inventory inventory = View.HeroInventory;

            // Remove item from inventory
            Item itemToRemove = inventory.Items[selection];
            inventory.DiscardItem(itemToRemove);

            // TO DO: Implement selling items when in town
        }

        private void btnItemJunk_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;
            View.HeroInventory.JunkItem(selection);
            lbInventory.SelectedIndex = selection;
        }

        private void btnItemKeep_Click(object sender, RoutedEventArgs e)
        {
            int selection = lbInventory.SelectedIndex;
            if (selection == -1)
                return;
            View.HeroInventory.KeepItem(selection);
            lbInventory.SelectedIndex = selection;
        }

        private ViewModel View
        {
            get => (DataContext as ViewModel);
        }
    }
}
