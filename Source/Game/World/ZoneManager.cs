//------------------------------------------------------------------------------
//
// File Name:	ZoneManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class ZoneManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public override void Inintialize()
        {
            heroManager = EngineCore.GetModule<HeroManager>();
            audioManager = EngineCore.GetModule<AudioManager>();
            gameManager = EngineCore.GetModule<GameManager>();
            worldEventManager = EngineCore.GetModule<WorldEventManager>();

            AddEventHandler(GameEvents.PlayerLook, OnPlayerLook);
            AddEventHandler(GameEvents.PlayerExplore, OnPlayerExplore);
        }

        public void AdvanceToNextZone()
        {
            SetZone(NextZoneName);
        }

        public void SetZone(string name)
        {
            heroManager.Hero.CurrentZone = name;

            // Only change zones if necessary
            if (CurrentZone != null && heroManager.Hero.CurrentZone == CurrentZone.Name)
                return;

            CurrentZone = zoneFactory.Create(heroManager.Hero.CurrentZone);

            // TO DO: Provide town choices if in town
            gameManager.CurrentChoiceText = GameManager.exploreChoiceText;

            // Set the mood
            if (audioManager.ambientTrack.isValid())
                AudioManager.ErrorCheck(audioManager.ambientTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (CurrentZone.AmbientTrackName != null)
                audioManager.ambientTrack = audioManager.PlayEvent(CurrentZone.AmbientTrackName, 0.5f);

            // Play that funky muzak
            if (audioManager.musicTrack.isValid())
                AudioManager.ErrorCheck(audioManager.musicTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (CurrentZone.MusicTrackName != null)
                audioManager.musicTrack = audioManager.PlayEvent(CurrentZone.MusicTrackName, 0.5f);
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public WorldZone CurrentZone { get; set; }
        public string NextZoneName { get; set; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnPlayerLook(object sender, GameEventArgs args)
        {
            RaiseGameEvent(GameEvents.WorldEventText, this, CurrentZone.LookText);
        }

        private void OnPlayerExplore(object sender, GameEventArgs args)
        {
            WorldEvent worldEvent = CurrentZone.EventTable.GenerateObject(heroManager.Hero);
            worldEventManager.ProcessWorldEvent(worldEvent);
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private WorldZoneFactory zoneFactory = new WorldZoneFactory();

        // Modules
        private HeroManager heroManager;
        private AudioManager audioManager;
        private GameManager gameManager;
        private WorldEventManager worldEventManager;
    }
}
