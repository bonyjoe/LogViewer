using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Model
{
    public class FormattingRuleData : BaseDataItem
    {
        #region Fields

        private String _foregroundColor = "#FFFFFF";
        private String _backgroundColor = "#000000";
        private String _regex = null;
        private UInt16 _priority = 0;

        #endregion

        #region Properties

        public String ForegroundColor
        {
            get { return _foregroundColor; }
            set { SetProperty(ref _foregroundColor, value); }
        }

        public String BackgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }

        public String Regex
        {
            get { return _regex; }
            set { SetProperty(ref _regex, value); }
        }

        public UInt16 Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        #endregion

        #region Constructor

        public FormattingRuleData(UInt16 priority)
        {
            this.Priority = priority;
        }
        
        #endregion

        #region Methods

        public Boolean CheckRule(LogLineData line)
        {
            if (String.IsNullOrEmpty(line.Value))
                return false;

            if(line.Value.Contains(_regex))
            {
                line.AppliedRule = this;
                return true;
            }

            return false;
        }

        public FormattingRuleData Clone()
        {
            return new FormattingRuleData(this.Priority)
            {
                BackgroundColor = this.BackgroundColor,
                ForegroundColor = this.ForegroundColor,
                Regex = this.Regex
            };
        }

        #endregion
    }
}
