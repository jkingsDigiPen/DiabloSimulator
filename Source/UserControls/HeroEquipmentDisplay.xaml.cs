//------------------------------------------------------------------------------
//
// File Name:	HeroEquipmentDisplay.xaml.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Windows.Controls;

namespace DiabloSimulator.UserControls
{
    /// <summary>
    /// Interaction logic for HeroEquipmentDisplay.xaml
    /// </summary>
    public partial class HeroEquipmentDisplay : UserControl
    {
        public HeroEquipmentDisplay()
        {
            InitializeComponent();

            lbEquip0.Items.Clear();
            lbEquip1.Items.Clear();
            lbEquip2.Items.Clear();

            lbEquip0.Items.Add("");
            lbEquip1.Items.Add("Head: " + "Cap");
            lbEquip2.Items.Add("Amulet: " + "Halcyon's Ascent");

            lbEquip0.Items.Add("Gloves: " + "Thin Gloves");
            lbEquip1.Items.Add("Chest: " + "Quilted Armor");
            lbEquip2.Items.Add("Belt: " + "Heavy Belt");

            lbEquip0.Items.Add("Ring 1: " + "Stone of Jordan");
            lbEquip1.Items.Add("Pants: " + "Hammering Pants");
            lbEquip2.Items.Add("Ring 2: " + "Hellfire Ring");

            lbEquip0.Items.Add("Weapon: " + "Wirt's Leg");
            lbEquip1.Items.Add("Boots: " + "Leather Boots");
            lbEquip2.Items.Add("Off-Hand: " + "Buckler");
        }
    }
}
