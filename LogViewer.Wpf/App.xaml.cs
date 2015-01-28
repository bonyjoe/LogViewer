using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LogViewer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Setup complete indicator.
        /// </summary>
        private bool setupComplete;

        /// <summary>
        /// Does the setup.
        /// </summary>
        private void DoSetup()
        {
            LogViewerPresenter presenter = new LogViewerPresenter((ContentControl)MainWindow.FindName("mainContentControl"),
                (ContentControl)MainWindow.FindName("sidebarContentControl"),
                (ContentControl)MainWindow.FindName("statusBarContentControl"));

            Setup setup = new Setup(Dispatcher, presenter);
            setup.Initialize();
            setup.RegisterTypes();

            //Register Singletons
            Mvx.RegisterSingleton<IMvxWpfViewPresenter>(presenter);

            IMvxAppStart start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            this.setupComplete = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Activated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnActivated(EventArgs e)
        {
            if (!this.setupComplete)
            {
                this.DoSetup();
            }

            base.OnActivated(e);
        }
    }
}
