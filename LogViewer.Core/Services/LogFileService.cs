using LogViewer.Core.Framework;
using LogViewer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Services
{
    public class LogFileService : ILogFileService
    {
        private LogFileData _currentLogFile;

        public LogFileData CurrentLogFile
        {
            get { return _currentLogFile; }
            set
            {
                _currentLogFile = value;
                RaiseLogFileChanged();
            }
        }

        private void RaiseLogFileChanged()
        {
            if (LogFileChanged != null)
                LogFileChanged(this, EventArgs.Empty);
        }

        public event EventHandler LogFileChanged;
    }
}
