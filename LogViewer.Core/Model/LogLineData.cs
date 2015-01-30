using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Model
{
    public class LogLineData : BaseDataItem
    {
        #region Fields

        private String _value;
        //private Int64 _lineNum;
        private FormattingRuleData _appliedRule = null;

        #endregion

        #region Properties

        //public Int64 LineNum
        //{
        //    get { return _lineNum; }
        //    set { SetProperty(ref _lineNum, value); }
        //}

        public String Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        public FormattingRuleData AppliedRule
        {
            get { return _appliedRule; }
            set { SetProperty(ref _appliedRule, value); }
        }

        #endregion

        #region Constructor

        public LogLineData(String value)
        {
            this.Value = value;
        }

        #endregion
    }
}
