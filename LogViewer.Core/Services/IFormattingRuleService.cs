using LogViewer.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Services
{
    public interface IFormattingRuleService
    {
        ObservableCollection<FormattingRuleData> Rules { get; set; }
        void LoadRules();
        void SaveRules();
        void CheckRules(LogLineData line);
        void CheckRules(IEnumerable<LogLineData> lines);
        void RefreshAppliedRules();
    }
}
