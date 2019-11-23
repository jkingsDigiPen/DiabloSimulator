using System;
using System.Windows;
using System.Windows.Input;

namespace DiabloSimulator.StyleableWindow
{
    public class WindowCloseCommand : ICommand
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
                window.Close();
            }
        }
    }
}
