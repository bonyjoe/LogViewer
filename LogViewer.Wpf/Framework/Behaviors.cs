using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LogViewer.Wpf.Framework
{
    public class ItemControlBehaviors
    {
        public static Boolean GetAutoScrollToBottom(ListBox obj)
        {
            return (Boolean)obj.GetValue(AutoScrollToBottomProperty);
        }

        public static void SetAutoScrollToBottom(ListBox obj, Boolean value)
        {
            obj.SetValue(AutoScrollToBottomProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoScrollToBottom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoScrollToBottomProperty =
            DependencyProperty.RegisterAttached("AutoScrollToBottom", typeof(Boolean), typeof(ItemControlBehaviors), new PropertyMetadata(false, OnAutoScrollToBottomPropertyChanged));

        private static void OnAutoScrollToBottomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = d as ListBox;
            var data = listBox.Items.SourceCollection as INotifyCollectionChanged;

            if (listBox == null || data == null)
                return;

            var scrollToEndHandler = new NotifyCollectionChangedEventHandler(
                (s1, e1) =>
                {
                    if(listBox.Items.Count > 0)
                    {
                        object lastItem = listBox.Items[listBox.Items.Count - 1];
                        listBox.Items.MoveCurrentTo(lastItem);
                        listBox.ScrollIntoView(lastItem);
                    }
                });

            if((bool)e.NewValue)
            {
                data.CollectionChanged += scrollToEndHandler;
            }
            else
            {
                data.CollectionChanged -= scrollToEndHandler;
            }
        }


    }
}
