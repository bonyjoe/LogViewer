using Cirrious.MvvmCross.ViewModels;
using LogViewer.Core.Model;
using LogViewer.Core.Services;
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

        private IFormattingRuleService _ruleService;

        private String _currentFilename;
        private ObservableCollection<FormattingRuleData> _rules;
        private FormattingRuleData _selectedRule;

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
            }
        }

        #endregion

        #region Constructor

        public SidebarViewModel(IFormattingRuleService ruleService)
        {
            _ruleService = ruleService;
            _ruleService.LoadRules();
            Rules = _ruleService.Rules;
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

        #region RemoveRuleCommand

        private MvxCommand _removeRuleCommand;

        public MvxCommand RemoveRuleCommand
        {
            get
            {
                if (_removeRuleCommand == null)
                {
                    _removeRuleCommand = new MvxCommand(RemoveRule_Executed, RemoveRule_CanExecute);
                    RegisterCommand(_removeRuleCommand);
                }
                return _removeRuleCommand;
            }
        }

        private bool RemoveRule_CanExecute()
        {
            return SelectedRule != null;
        }

        private void RemoveRule_Executed()
        {
            Rules.Remove(SelectedRule);
            SelectedRule = null;
        }

        #endregion

        #region MoveRuleUpCommand

        private MvxCommand _moveRuleUpCommand;

        public MvxCommand MoveRuleUpCommand
        {
            get
            {
                if (_moveRuleUpCommand == null)
                {
                    _moveRuleUpCommand = new MvxCommand(MoveRuleUp_Executed, MoveRuleUp_CanExecute);
                    RegisterCommand(_moveRuleUpCommand);
                }
                return _moveRuleUpCommand;
            }
        }

        private bool MoveRuleUp_CanExecute()
        {
            if (this.SelectedRule == null)
                return false;

            if (Rules.IndexOf(SelectedRule) == 0)
                return false;

            return true;
        }

        private void MoveRuleUp_Executed()
        {
            int oldIndex = Rules.IndexOf(SelectedRule);
            int newIndex = oldIndex - 1;

            SelectedRule.Priority -= 1;
            Rules.Move(oldIndex, newIndex);

            Rules[oldIndex].Priority += 1;
            RaisePropertyChanged(() => SelectedRule);
        }

        #endregion

        #region MoveRuleDownCommand

        private MvxCommand _moveRuleDownCommand;

        public MvxCommand MoveRuleDownCommand
        {
            get
            {
                if (_moveRuleDownCommand == null)
                {
                    _moveRuleDownCommand = new MvxCommand(MoveRuleDown_Executed, MoveRuleDown_CanExecute);
                    RegisterCommand(_moveRuleDownCommand);
                }
                return _moveRuleDownCommand;
            }
        }

        private bool MoveRuleDown_CanExecute()
        {
            if (this.SelectedRule == null)
                return false;

            if (Rules.IndexOf(SelectedRule) == Rules.Count - 1)
                return false;

            return true;
        }

        private void MoveRuleDown_Executed()
        {
            int oldIndex = Rules.IndexOf(SelectedRule);
            int newIndex = oldIndex + 1;

            SelectedRule.Priority += 1;
            Rules.Move(oldIndex, newIndex);

            Rules[oldIndex].Priority -= 1;
            RaisePropertyChanged(() => SelectedRule);
        }

        #endregion

        #endregion

        #region Methods

        #endregion
    }
}
