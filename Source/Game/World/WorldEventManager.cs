//------------------------------------------------------------------------------
//
// File Name:	WorldEventManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using System.Collections.Generic;

namespace DiabloSimulator.Game.World
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public class WorldEventManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public override void Inintialize()
        {
            // Register for events
            AddEventHandler(GameEvents.AddWorldEventText, OnAddWorldEventText);

            // Initial game text (new or otherwise)
            RaiseGameEvent(GameEvents.AddWorldEventText, this, "Welcome to the world of Sanctuary!");
        }

        public List<string> WorldEventsText
        {
            get
            {
                List<string> result = new List<string>(worldEventsText);
                worldEventsText.Clear();
                return result;
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        private void OnAddWorldEventText(object sender, GameEventArgs e)
        {
            worldEventsText.Add(e.Get<string>());
        }

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private List<string> worldEventsText = new List<string>();
    }
}
