//------------------------------------------------------------------------------
//
// File Name:	StatTable.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    using EquipmentMap = ObservableDictionaryNoThrow<SlotType, Item>;
    using ModifierMap = Dictionary<ModifierType, HashSet<StatModifier>>;

    public class Equipment
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public Equipment()
        {
            Items = new EquipmentMap();
        }

        public void EquipItem(Item item, StatTable heroStats)
        {
            // Handle two-handed weapons
            if (item.slot == SlotType.BothHands)
            {
                Items[SlotType.MainHand] = item;
                Items[SlotType.OffHand] = item;
            }
            // Handle other slots
            else
            {
                Items[item.slot] = item;
            }

            AddItemModsToHeroStats(item, heroStats);
        }

        public Item UnequipItem(SlotType slot, StatTable heroStats)
        {
            Item removedItem = null;
            if (!Items.TryGetValue(slot, out removedItem) 
                || removedItem is null || removedItem.Name == Item.EmptyItemText)
                return null;

            // DON'T actually remove - breaks bindings
            Items[slot] = new Item();

            // Handle two-handed weapons
            if (removedItem.slot == SlotType.BothHands)
            {
                if (slot == SlotType.MainHand)
                {
                    Items[SlotType.OffHand] = new Item();
                }
                else if(slot == SlotType.OffHand)
                {
                    Items[SlotType.MainHand] = new Item();
                }
            }

            RemoveItemModsFromHeroStats(removedItem, heroStats);

            return removedItem;
        }

        public EquipmentMap Items { get; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void AddItemModsToHeroStats(Item itemToEquip, StatTable heroStats)
        {
            // Add stats from item to hero stat mods
            foreach (KeyValuePair<string, float> mod in itemToEquip.stats.LeveledValues)
            {
                // Ignore "required level"
                if (mod.Key == "RequiredLevel")
                    continue;

                heroStats.AddModifier(new StatModifier(mod.Key,
                    itemToEquip.Name, ModifierType.Additive, mod.Value));
            }

            foreach (KeyValuePair<string, ModifierMap> modMap in itemToEquip.stats.Modifiers)
            {
                foreach (KeyValuePair<ModifierType, HashSet<StatModifier>> modSet in modMap.Value)
                {
                    foreach (StatModifier mod in modSet.Value)
                    {
                        heroStats.AddModifier(mod);
                    }
                }
            }
        }

        private void RemoveItemModsFromHeroStats(Item unequipped, StatTable heroStats)
        {
            // Remove stats from item from hero stat mods
            foreach (KeyValuePair<string, float> mod in unequipped.stats.LeveledValues)
            {
                heroStats.RemoveModifier(new StatModifier(mod.Key,
                    unequipped.Name, ModifierType.Additive, mod.Value));
            }

            foreach (KeyValuePair<string, ModifierMap> modMap in unequipped.stats.Modifiers)
            {
                foreach (KeyValuePair<ModifierType, HashSet<StatModifier>> modSet in modMap.Value)
                {
                    foreach (StatModifier mod in modSet.Value)
                    {
                        heroStats.RemoveModifier(mod);
                    }
                }
            }
        }
    }
}
