using DiabloSimulator.Engine;
using DiabloSimulator.Game.Factories;
using System;

namespace DiabloSimulator.Game.World
{
    public class ZoneManager : IModule
    {
        public ZoneManager()
        {

        } 

        public void SetZone(string name)
        {
            hero.CurrentZone = name;

            // Only change zones if necessary
            if (zone != null && hero.CurrentZone == zone.Name)
                return;

            zone = zoneFactory.Create(hero.CurrentZone);

            // TO DO: Provide town choices if in town
            currentChoiceText = exploreChoiceText;

            // Set the mood
            if (ambientTrack.isValid())
                AudioManager.ErrorCheck(ambientTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (zone.AmbientTrackName != null)
                ambientTrack = audio.PlayEvent(zone.AmbientTrackName, 0.5f);

            // Play that funky muzak
            if (musicTrack.isValid())
                AudioManager.ErrorCheck(musicTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (zone.MusicTrackName != null)
                musicTrack = audio.PlayEvent(zone.MusicTrackName, 0.5f);
        }

        private WorldZoneFactory zoneFactory;
        private WorldZone zone;
        private string nextZoneName;
    }
}
