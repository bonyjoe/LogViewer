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
        private String _foreground = "#000000";
        private String _background = "#CCCCCC";
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

        public String Foreground
        {
            get { return _foreground; }
            set { SetProperty(ref _foreground, value); }
        }

        public String Background
        {
            get { return _background; }
            set { SetProperty(ref _background, value); }
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
