using System;
using Cirrious.MvvmCross.ViewModels;
using Microsoft.Win32;
using LogViewer.Core.Model;
using LogViewer.Core.Services;

namespace LogViewer.Core.ViewModels
{
    public class MainViewModel 
		: MvxViewModel
    {
        #region Fields

        private ILogFileService _logService;
        private IOpenFileService _fileService;

        private String _currentFilename;
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
                RaisePropertyChanged("Title");
            }
        }

        public String Title
        {
            get 
            { 
                if(CurrentLog == null)
                    return "Log Viewer";

                return String.Format("{0} - Log Viewer", CurrentLog.FullPath);
            }
        }

        #endregion

        #region Constructor

        public MainViewModel(ILogFileService logService, IOpenFileService fileService)
        {
            _logService = logService;
            _fileService = fileService;
        }
        
        #endregion

        #region Commands

        #region OpenCommand

        private IMvxCommand _openCommand;

        public IMvxCommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new MvxCommand(Open_Executed, Open_CanExecute);
                }
                return _openCommand;
            }
        }

        private bool Open_CanExecute()
        {
            //Command can execute logic
            return true;
        }

        private void Open_Executed()
        {
            //Command execute logic

            var filename = _fileService.GetFilename();
            if (String.IsNullOrWhiteSpace(filename))
                return;


            CurrentLog = new LogFileData(filename);
            _logService.CurrentLogFile = CurrentLog;
        }

        #endregion

        #endregion

        public override void Start()
        {
            ShowViewModel<StatusBarViewModel>();
            ShowViewModel<SidebarViewModel>();
        }
    }
}
