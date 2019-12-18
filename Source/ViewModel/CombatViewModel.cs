//------------------------------------------------------------------------------
//
// File Name:	CombatViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using System.Collections.ObjectModel;

namespace DiabloSimulator.ViewModel
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class CombatViewModel
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public CombatViewModel()
        {
            monsterManager = EngineCore.GetModule<MonsterManager>();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public ObservableCollection<Monster> MonsterList { get => monsterManager.MonsterList; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------



        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Modules
        private MonsterManager monsterManager;
    }
}
