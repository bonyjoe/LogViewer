using LogViewer.Core.Framework;
using LogViewer.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Services
{
    public class FormattingRuleService : BasePropertyChanged, IFormattingRuleService
    {
        private ObservableCollection<FormattingRuleData> _rules;

        public ObservableCollection<FormattingRuleData> Rules
        {
            get { return _rules; }
            set { SetProperty(ref _rules, value); }
        }
    }
}
