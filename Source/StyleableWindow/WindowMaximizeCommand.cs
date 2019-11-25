//------------------------------------------------------------------------------
//
// File Name:	WindowMaximizeBehavior.cs
// Author(s):	MSDN Code Gallery
// Project:		DiabloSimulator
//
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Input;

namespace DiabloSimulator.StyleableWindow
{
    public class WindowMaximizeCommand : ICommand
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
                if (window.WindowState == WindowState.Maximized)
                {
                    window.WindowState = WindowState.Normal;
                }
                else
                {
                    window.WindowState = WindowState.Maximized;
                }
            }
        }
    }
}

