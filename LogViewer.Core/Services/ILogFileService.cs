using LogViewer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Services
{
    public interface ILogFileService
    {
        LogFileData CurrentLogFile { get; set; }
        event EventHandler LogFileChanged;
    }
}
