using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer.Core.Framework
{
    public class BulkObservableCollection<TItem> : ObservableCollection<TItem>
    {
        #region SuppressNotifyCollectionChanged

        private Boolean _suppressNotifyCollectionChanged = false;

        public Boolean SuppressNotifyCollectionChanged
        {
            get { return _suppressNotifyCollectionChanged; }
            set
            {
                _suppressNotifyCollectionChanged = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuppressNotifyCollectionChanged"));
            }
        } 

        #endregion

        #region Constructors

        public BulkObservableCollection(List<TItem> list)
            : base(list)
        {

        }

        public BulkObservableCollection(IEnumerable<TItem> collection)
            : base(collection)
        {

        }

        public BulkObservableCollection() : base() { }

        #endregion

        #region Range Related Methods

        /// <summary>
        /// Adds a range of items and calls the most performant oncollectionchanged to update any watchers
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<TItem> items)
        {
            this.CheckReentrancy();

            //if less than 15 there probably will be a performance hit from the reset so just add normally
            int insertCount = items.Count();
            if (insertCount < 15 && insertCount < Items.Count)
            {
                foreach (var item in items)
                {
                    this.Add(item);
                }
                return;
            }

            foreach (var item in items)
            {
                this.Items.Add(item);
            }

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        /// <summary>
        /// Adds a range of items, calling transform function on each item first, then calls the most performant oncollectionchanged to update any watchers
        /// </summary>
        /// <typeparam name="TIn">Input list item type</typeparam>
        /// <param name="items">List of items of TIn type</param>
        /// <param name="transform">Function to transform item from TIn to TItem</param>
        public void AddRange<TIn>(IEnumerable<TIn> items, Func<TIn, TItem> transform)
        {
            this.CheckReentrancy();

            //if less than 15 there probably will be a performance hit from the reset so just add normally
            int insertCount = items.Count();
            if (insertCount < 15 && insertCount < Items.Count)
            {
                foreach (var item in items)
                {
                    this.Add(transform(item));
                }
                return;
            }

            foreach (var item in items)
            {
                this.Items.Add(transform(item));
            }

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        public void ClearAndAddRange(IEnumerable<TItem> items)
        {
            this.Items.Clear();

            this.AddRange(items);
        }

        /// <summary>
        /// Calls a reset even if SuppressNotifyCollectionChanged is true
        /// </summary>
        public void Reset()
        {
            var temp = _suppressNotifyCollectionChanged;
            _suppressNotifyCollectionChanged = false;

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

            _suppressNotifyCollectionChanged = temp;
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressNotifyCollectionChanged)
            {
                base.OnCollectionChanged(e);
            }
        } 
        #endregion
    }
}
