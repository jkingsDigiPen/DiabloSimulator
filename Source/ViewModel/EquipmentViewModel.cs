//------------------------------------------------------------------------------
//
// File Name:	EquipmentViewModel.cs
// Author(s):	Jeremy Kings
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using DiabloSimulator.Engine;
using DiabloSimulator.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloSimulator.ViewModel
{
    //------------------------------------------------------------------------------
    // Public Structures:
    //------------------------------------------------------------------------------

    public sealed class EquipmentViewModel
    {
        //------------------------------------------------------------------------------
        // Public Functions:
        //------------------------------------------------------------------------------

        public EquipmentViewModel()
        {
            heroManager = EngineCore.GetModule<HeroManager>();
        }

        //------------------------------------------------------------------------------
        // Public Variables:
        //------------------------------------------------------------------------------

        public Hero Hero { get => heroManager.Hero; }

        //------------------------------------------------------------------------------
        // Private Functions:
        //------------------------------------------------------------------------------



        //------------------------------------------------------------------------------
        // Private Variables:
        //------------------------------------------------------------------------------

        // Modules
        private HeroManager heroManager;
    }
}
