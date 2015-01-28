using LogViewer.Core.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Wpf.Services
{
    public class OpenFileService : IOpenFileService
    {
        public string GetFilename()
        {
            OpenFileDialog dlg = new OpenFileDialog() { Filter = "Log Files|*.log;*.txt|All Files|*.*", Multiselect = false, Title = "Open Log File" };
            var result = dlg.ShowDialog();

            if (result == false)
                return null;

            return dlg.FileName;
        }
    }
}
