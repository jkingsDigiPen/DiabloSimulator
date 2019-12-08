//------------------------------------------------------------------------------
//
// File Name:	AudioManager.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using FMOD.Studio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiabloSimulator.Engine
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    using FMODSystem = FMOD.Studio.System;

    public class AudioManager : IModule
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public AudioManager()
        { 
            ErrorCheck(FMODSystem.create(out fmodStudioSystem));
            ErrorCheck(fmodStudioSystem.initialize(16, INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));
        }

        public override void Inintialize()
        {
            // Make sure update gets called periodically
            var dueTime = TimeSpan.FromSeconds(1);
            var interval = TimeSpan.FromSeconds(2);

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            // TODO: Figure out whether warning needs to be fixed.
            #pragma warning disable 4014
            RunPeriodicAsync(Update, dueTime, interval, CancellationToken.None);
            #pragma warning restore 4014

            // Register for events
            AddEventHandler(Game.GameEvents.SetBackgroundTrack, OnSetBackgroundTrack);
            AddEventHandler(Game.GameEvents.SetAmbientTrack, OnSetAmbientTrack);
            AddEventHandler(Game.GameEvents.LoadAudioBank, OnLoadAudioBank);
        }

        public EventInstance PlayEvent(string eventName, float volume = 1.0f)
        {
            EventInstance eventInstance;
            EventDescription eventDescription;

            ErrorCheck(fmodStudioSystem.getEvent(eventPrefix + eventName, out eventDescription));
            ErrorCheck(eventDescription.createInstance(out eventInstance));
            ErrorCheck(eventInstance.setVolume(volume));
            ErrorCheck(eventInstance.start());
            ErrorCheck(eventInstance.release());

            return eventInstance;
        }

        public void LoadBank(string bankName)
        {
            Bank bank;
            string fullPath = audioFilePath + bankName + ".bank";
            ErrorCheck(fmodStudioSystem.loadBankFile(fullPath, 
                LOAD_BANK_FLAGS.NORMAL, out bank));

            PrintBankInfo(bank, bankName);
        }

        static public void ErrorCheck(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                throw new System.Exception("FMOD error! (" + FMOD.Error.String(result) + ")");
            }
        }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------

        #region eventHandlers
        private void OnSetAmbientTrack(object sender, GameEventArgs e)
        {
            string ambientTrackName = e.Get<string>();

            // Set the mood
            if (ambientTrack.isValid())
                AudioManager.ErrorCheck(ambientTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (ambientTrackName != null)
                ambientTrack = PlayEvent(ambientTrackName, 0.5f);
        }

        private void OnSetBackgroundTrack(object sender, GameEventArgs e)
        {
            string backgroundTrackName = e.Get<string>();

            // Play that funky muzak
            if (backgroundTrack.isValid())
                AudioManager.ErrorCheck(backgroundTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
            if (backgroundTrackName != null)
                backgroundTrack = PlayEvent(backgroundTrackName, 0.5f);
        }

        private void OnLoadAudioBank(object sender, GameEventArgs e)
        {
            LoadBank(e.Get<string>());
        }

        #endregion

        #region audioEngine


        private void Update()
        {
            ErrorCheck(fmodStudioSystem.update());
        }

        private void PrintBankInfo(Bank bank, string bankName)
        {
            // See the following link for potential ConsoleManager system:
            // https://stackoverflow.com/questions/160587/no-output-to-console-from-a-wpf-application

            Console.WriteLine("Loaded FMOD Bank \"" + bankName + "\" successfully.");

            // Determine number of events in bank
            EventDescription[] events = null;
            bank.getEventList(out events);
            string eventPath;

            // No events - no need to print any more info
            if (events.Length == 0)
            {
                Console.WriteLine();
                return;
            }

            Console.WriteLine("Events in bank:");
            for (int i = 0; i < events.Length; ++i)
            {
                // Display each event
                events[i].getPath(out eventPath);
                //string eventName = eventPath;
                //eventName = eventName.Substring(eventName.IndexOf("/") + 1, eventName.Length);
                Console.WriteLine(i + ": " + "\"" + eventPath + "\"");

                // Determine number of parameters in event
                int paramCount;
                events[i].getParameterDescriptionCount(out paramCount);
                Console.WriteLine("Parameters for event:");

                for (int j = 0; j < paramCount; ++j)
                {
                    // Display each event parameter
                    PARAMETER_DESCRIPTION param;
                    events[i].getParameterDescriptionByIndex(j, out param);
                    Console.WriteLine(param);
                }
            }
            Console.WriteLine();
        }

        // The `onTick` method will be called periodically unless cancelled.
        private static async Task RunPeriodicAsync(Action onTick,
                                                   TimeSpan dueTime,
                                                   TimeSpan interval,
                                                   CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Call our onTick function.
                onTick?.Invoke();

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token);
            }
        }

        #endregion

        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        private FMODSystem fmodStudioSystem;

        private const string audioFilePath = "Assets\\Audio\\";
        private const string eventPrefix = "event:/";

        private EventInstance backgroundTrack;
        private EventInstance ambientTrack;
    }
}
