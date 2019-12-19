using System.ComponentModel;

namespace DiabloSimulator.Game
{
    public enum GameEvents
    {
        // Player
        [Description("Look")]
        PlayerLook,
        [Description("Explore")]
        PlayerExplore,
        [Description("Attack")]
        PlayerAttack,
        [Description("Defend")]
        PlayerDefend,
        [Description("Proceed")]
        PlayerProceed,
        [Description("Back")]
        PlayerBack,
        [Description("Rest")]
        PlayerRest,
        [Description("Flee")]
        PlayerFlee,
        [Description("Town Portal")]
        PlayerTownPortal,
        [Description("Yes")]
        PlayerYes,
        [Description("No")]
        PlayerNo,
        PlayerUsePotion,

        // World Text
        AddWorldEventText,

        // World Event
        WorldMonster,
        WorldZoneDiscovery,
        WorldText,
        WorldItem,
        WorldGold,
        WorldPotion,
        WorldChoice,

        // Game
        GameStart,
        GameSave,
        GameLoad,
        AdvanceTime,

        // Monster
        MonsterDead,
        MonsterAttack,
        SetMonster,
        MonsterSelected,

        // Hero
        HeroDead,
        HeroAttack,
        HeroCreate,

        // World Zone
        SetWorldZone,

        // Audio
        SetAmbientTrack,
        SetBackgroundTrack,
        LoadAudioBank,

        // Items
        ItemEquip,
        ItemDiscard,
        ItemSell,
        ItemJunk,
        ItemKeep,
        ItemUnequip,
    }
}
