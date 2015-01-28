using Cirrious.MvvmCross.ViewModels;
using LogViewer.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
        #region Fields

        private String _currentFilename;
        private ObservableCollection<FormattingRuleData> _rules;
        private FormattingRuleData _selectedRule;
        private FormattingRuleData _cachedSelectedRule;

        #endregion

        #region Properties

        public String CurrentFilename
        {
            get { return _currentFilename; }
            set { SetProperty(ref _currentFilename, value); }
        }

        public ObservableCollection<FormattingRuleData> Rules
        {
            get { return _rules; }
            set { SetProperty(ref _rules, value); }
        }

        public FormattingRuleData SelectedRule
        {
            get { return _selectedRule; }
            set 
            { 
                SetProperty(ref _selectedRule, value);
                _cachedSelectedRule = value.Clone();
            }
        }

        #endregion

        #region Constructor

        public SidebarViewModel()
        {
            _rules = new ObservableCollection<FormattingRuleData>();
        }

        #endregion

        #region Commands

        #region AddRuleCommand

        private MvxCommand<UInt16?> _addRuleCommand;

        public MvxCommand<UInt16?> AddRuleCommand
        {
            get
            {
                if (_addRuleCommand == null)
                {
                    _addRuleCommand = new MvxCommand<UInt16?>(AddRule_Executed, AddRule_CanExecute);
                }
                return _addRuleCommand;
            }
        }

        private bool AddRule_CanExecute(UInt16? priority)
        {
            return true;
        }

        private void AddRule_Executed(UInt16? priority)
        {
            if (!priority.HasValue)
            {
                UInt16 lowPriority;
                if (!Rules.Any())
                    lowPriority = 1;
                else
                    lowPriority = (UInt16)(Rules.Max(x => x.Priority) + 1);

                var newRule = new FormattingRuleData(lowPriority);
                Rules.Add(newRule);
                this.SelectedRule = newRule;

                return;
            }

            var templateRule = Rules.FirstOrDefault(x => x.Priority == priority.Value);
            if(templateRule != null)
            {
                var newRule = templateRule.Clone();
                newRule.Priority++;

                foreach (var rule in Rules.Where(x => x.Priority >= newRule.Priority))
                {
                    rule.Priority++;
                }

                Rules.Insert(Rules.IndexOf(templateRule) + 1, newRule);
                this.SelectedRule = newRule;
            }
        }

        #endregion

        #region SaveRuleEditCommand

        private MvxCommand _saveRuleEditCommand;

        public MvxCommand SaveRuleEditCommand
        {
            get
            {
                if (_saveRuleEditCommand == null)
                {
                    _saveRuleEditCommand = new MvxCommand(SaveRuleEdit_Executed, SaveRuleEdit_CanExecute);
                }
                return _saveRuleEditCommand;
            }
        }

        private bool SaveRuleEdit_CanExecute()
        {
            //Command can execute logic
            return true;
        }

        private void SaveRuleEdit_Executed()
        {
            //Command execute logic
            _cachedSelectedRule = SelectedRule.Clone();
        }

        #endregion

        #region CancelRuleEditCommand

        private MvxCommand _cancelRuleEditCommand;

        public MvxCommand CancelRuleEditCommand
        {
            get
            {
                if (_cancelRuleEditCommand == null)
                {
                    _cancelRuleEditCommand = new MvxCommand(CancelRuleEdit_Executed, CancelRuleEdit_CanExecute);
                }
                return _cancelRuleEditCommand;
            }
        }

        private bool CancelRuleEdit_CanExecute()
        {
            //Command can execute logic
            return true;
        }

        private void CancelRuleEdit_Executed()
        {
            SelectedRule = _cachedSelectedRule;
        }

        #endregion

        #endregion

        #region Methods

        #endregion
    }
}
