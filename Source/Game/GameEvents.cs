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

        // World
        WorldEventText,

        // Game
        AdvanceTime,

        // Monster
        MonsterDead,
        MonsterAttack,

        // Hero
        HeroDead,
        HeroAttack,
    }
}
