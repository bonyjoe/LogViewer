using Cirrious.MvvmCross.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LogViewer.Wpf
{
    public class LogViewerPresenter : MvxWpfViewPresenter
    {
        private readonly ContentControl _mainContentControl;
        private readonly ContentControl _sidebarContentControl;
        private readonly ContentControl _statusBarContentControl;

        public LogViewerPresenter(ContentControl mainContentControl, ContentControl sidebarContentControl, ContentControl statusBarContentControl)
        {
            _mainContentControl = mainContentControl;
            _sidebarContentControl = sidebarContentControl;
            _statusBarContentControl = statusBarContentControl;
        }

        public override void Present(System.Windows.FrameworkElement frameworkElement)
        {
            //decide what kind of content it is
            if (frameworkElement as IContentView != null)
            {
                _mainContentControl.Content = frameworkElement;
                return;
            }
            else if (frameworkElement as ISidebarView != null)
            {
                _sidebarContentControl.Content = frameworkElement;
                return;
            }
            else if (frameworkElement as IStatusBarView != null)
            {
                _statusBarContentControl.Content = frameworkElement;
                return;
            }

            throw new Exception("Needs to implement a view interface");
        }

        public Boolean ToggleSidebarVisibility()
        {
            if (_sidebarContentControl.Visibility == System.Windows.Visibility.Visible)
            {
                _sidebarContentControl.Visibility = System.Windows.Visibility.Collapsed;
                return false;
            }

            _sidebarContentControl.Visibility = System.Windows.Visibility.Visible;
            return true;
        }
    }

    public interface IContentView { }

    public interface ISidebarView { }

    public interface IStatusBarView { }
}
