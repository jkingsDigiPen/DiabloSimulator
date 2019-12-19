//------------------------------------------------------------------------------
//
// File Name:	GameManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;

namespace DiabloSimulator.Game
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class GameManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        #region constructors

        public override void Inintialize()
        {
            // Register for player actions
            AddEventHandler(GameEvents.PlayerProceed, OnPlayerProceed);
            AddEventHandler(GameEvents.PlayerBack, OnPlayerBack);

            // Register for monster/hero actions
            AddEventHandler(GameEvents.MonsterDead, OnMonsterDead);
            AddEventHandler(GameEvents.HeroDead, OnHeroDead);
            AddEventHandler(GameEvents.AdvanceTime, OnAdvanceTime);

            // Register for world events
            AddEventHandler(GameEvents.WorldMonster, OnWorldMonster);
            AddEventHandler(GameEvents.WorldZoneDiscovery, OnWorldZoneDiscovery);

            // Load audio shtuff
            RaiseGameEvent(GameEvents.LoadAudioBank, this, "Master.strings");
            RaiseGameEvent(GameEvents.LoadAudioBank, this, "Master");
            RaiseGameEvent(GameEvents.LoadAudioBank, this, "Music");
        }

        #endregion

        #region gameFunctions

        public bool InCombat 
        { 
            get => inCombat; 
            set
            {
                inCombat = value;

                if (value == true)
                    CurrentChoiceText = combatChoiceText;
                else
                    CurrentChoiceText = exploreChoiceText;
            }
        }

        public int Turns { get; set; }

        public PlayerChoiceText CurrentChoiceText { get; set; } = exploreChoiceText;

        #endregion

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region playerActions

        private void OnPlayerProceed(object sender, GameEventArgs e)
        {
            // TO DO: Provide town choices if in town
            CurrentChoiceText = exploreChoiceText;
        }

        private void OnPlayerBack(object sender, GameEventArgs e)
        {
            // TO DO: Provide town choices if in town
            CurrentChoiceText = exploreChoiceText;
        }

        #endregion

        #region monsterAndHeroActions

        private void OnMonsterDead(object sender, GameEventArgs e)
        {
            // Are all monsters gone?
            if(e.Get<int>() == 0)
            {
                InCombat = false;
            }
        }

        private void OnHeroDead(object sender, GameEventArgs e)
        {
            InCombat = false;
        }

        private void OnWorldMonster(object sender, GameEventArgs e)
        {
            Turns = 0;
            InCombat = true;
        }

        #endregion

        #region gameStateFunctions

        private void OnAdvanceTime(object sender, GameEventArgs e)
        {
            if(InCombat) ++Turns;
        }

        private void OnWorldZoneDiscovery(object sender, GameEventArgs e)
        {
            CurrentChoiceText = discoverChoiceText;
        }

        #endregion

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        // Pseudo-constants
        public static PlayerChoiceText exploreChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerExplore),
                PlayerAction.GetDescription(GameEvents.PlayerRest), 
                PlayerAction.GetDescription(GameEvents.PlayerTownPortal));

        public static PlayerChoiceText combatChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerAttack),
                PlayerAction.GetDescription(GameEvents.PlayerDefend), PlayerAction.GetDescription(GameEvents.PlayerFlee));

        public static PlayerChoiceText discoverChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerProceed),
                PlayerAction.GetDescription(GameEvents.PlayerBack), 
                PlayerAction.GetDescription(GameEvents.PlayerTownPortal));

        public static PlayerChoiceText yesNoChoiceText =
            new PlayerChoiceText(PlayerAction.GetDescription(GameEvents.PlayerYes),
                PlayerAction.GetDescription(GameEvents.PlayerNo), PlayerAction.GetDescription(GameEvents.PlayerBack));

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Game state
        private bool inCombat;
    }
}
