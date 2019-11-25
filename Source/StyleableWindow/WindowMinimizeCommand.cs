//------------------------------------------------------------------------------
//
// File Name:	WindowMinimizeBehavior.cs
// Author(s):	MSDN Code Gallery
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

namespace DiabloSimulator.StyleableWindow
{
    public class WindowMinimizeCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Disable unused event warning
        #pragma warning disable
        public event EventHandler CanExecuteChanged;
        #pragma warning restore

        public void Execute(object parameter)
        {
            var window = parameter as Window;

            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }
}
