using Cirrious.CrossCore;
using LogViewer.Core.Framework;
using LogViewer.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;

namespace LogViewer.Core.Services
{
    public class FormattingRuleService : BasePropertyChanged, IFormattingRuleService
    {
        #region Fields

        private ObservableCollection<FormattingRuleData> _rules;
        private IEnumerable<FormattingRuleData> _orderedRules;
        private String _saveDirectory;
        private String _savePath = "rules.xml";

        private Timer _refreshTimer;

        #endregion

        #region Properties

        public ObservableCollection<FormattingRuleData> Rules
        {
            get { return _rules; }
            set
            {
                OnRulesChanging(_rules, value);
                SetProperty(ref _rules, value);
            }
        }

        #endregion

        #region Constructor

        public FormattingRuleService()
        {
            LoadRules();
            _saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NDLogViewer");
            _refreshTimer = new Timer(500);
            _refreshTimer.AutoReset = false;
            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
        }

        #endregion

        #region Methods

        private void OnRulesChanging(ObservableCollection<FormattingRuleData> oldValue, ObservableCollection<FormattingRuleData> newValue)
        {
            //clean up old event handler
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= Rules_CollectionChanged;

                foreach (var rule in oldValue)
                {
                    rule.Changed -= rule_Changed;
                }
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += Rules_CollectionChanged;
                _orderedRules = newValue.OrderBy(x => x.Priority);

                foreach (var rule in newValue)
                {
                    rule.Changed += rule_Changed;
                }
            }
        }

        public void LoadRules()
        {
            if (!Directory.Exists(_saveDirectory))
            {
                Rules = new ObservableCollection<FormattingRuleData>();
                return;
            }
            string path = Path.Combine(_saveDirectory, _savePath);
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    XmlSerializer serialize = new XmlSerializer(typeof(ObservableCollection<FormattingRuleData>));
                    Rules = serialize.Deserialize(reader) as ObservableCollection<FormattingRuleData>;
                }
            }
            catch
            {
                Rules = new ObservableCollection<FormattingRuleData>();
            }
        }

        public void SaveRules()
        {
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);

            string path = Path.Combine(_saveDirectory, _savePath);
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                XmlSerializer serialize = new XmlSerializer(typeof(ObservableCollection<FormattingRuleData>));
                serialize.Serialize(writer, Rules);
            }
        }

        public void CheckRules(LogLineData line)
        {
            foreach (var rule in _orderedRules)
            {
                if (rule.CheckRule(line))
                    break;
            }
        }

        public void CheckRules(IEnumerable<LogLineData> lines)
        {
            foreach (var line in lines)
            {
                CheckRules(line);
            }
        }

        private void QueueRefresh()
        {
            _refreshTimer.Stop();
            _refreshTimer.Start();
        }

        public void RefreshAppliedRules()
        {
            _refreshTimer.Stop();

            ILogFileService logFileService = Mvx.Resolve<ILogFileService>();

            if (logFileService.CurrentLogFile != null)
            {
                CheckRules(logFileService.CurrentLogFile.Lines);
            }
        }

        #endregion

        #region Handlers

        void Rules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _orderedRules = Rules.OrderBy(x => x.Priority);

            if(e.OldItems != null)
            {
                foreach (var item in e.OldItems.Cast<FormattingRuleData>())
                {
                    item.Changed -= rule_Changed;
                }
            }

            if(e.NewItems != null)
            {
                foreach (var item in e.NewItems.Cast<FormattingRuleData>())
                {
                    item.Changed += rule_Changed;
                }
            }

            QueueRefresh();
        }

        void rule_Changed(object sender, EventArgs e)
        {
            QueueRefresh();
        }

        void _refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RefreshAppliedRules();
        }

        #endregion
        
    }
}
