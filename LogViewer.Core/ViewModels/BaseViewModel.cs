using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.ViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        private List<IMvxCommand> _commands = new List<IMvxCommand>();

        #region Commands

        protected void RegisterCommand(IMvxCommand command)
        {
            if (_commands.Contains(command))
                return;

            if (_commands.Count == 0)
                RegisterCanExecuteChangedHandler();

            _commands.Add(command);
        }

        private void RegisterCanExecuteChangedHandler()
        {
            this.PropertyChanged += CommandCanExecuteHandler;
        }

        private void CommandCanExecuteHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            foreach (var item in _commands)
            {
                item.RaiseCanExecuteChanged();
            }
        }

        #endregion
    }
}
