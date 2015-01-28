using Cirrious.MvvmCross.ViewModels;
using LogViewer.Core.Model;
using LogViewer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.ViewModels
{
    public class StatusBarViewModel : MvxViewModel
    {
        #region Fields

        private ILogFileService _logService;

        private LogFileData _currentLog;

        #endregion

        #region Properties

        public LogFileData CurrentLog
        {
            get { return _currentLog; }
            set
            {
                _currentLog = value;
                RaisePropertyChanged("CurrentLog");
            }
        }

        #endregion

        #region Constructor

        public StatusBarViewModel(ILogFileService logService)
        {
            _logService = logService;
            logService.LogFileChanged += logService_LogFileChanged;
            CurrentLog = logService.CurrentLogFile;
        }

        #endregion

        #region Event Handlers

        void logService_LogFileChanged(object sender, EventArgs e)
        {
            this.CurrentLog = _logService.CurrentLogFile;
        }

        #endregion
    }
}
