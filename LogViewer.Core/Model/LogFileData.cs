using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using LogViewer.Core.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LogViewer.Core.Model
{
    public class LogFileData : BaseDataItem
    {
        #region Fields

        private FileSystemWatcher _watch;
        private Object _watchLock = new Object();
        private Timer _timer;

        private String _filename;
        private String _fullPath;

        private DateTime _created;
        private DateTime _lastUpdated;

        private Int64 _fileSize;

        private BulkObservableCollection<LogLineData> _lines;

        #endregion

        #region Properties

        public DateTime Created
        {
            get { return _created; }
            protected set { SetProperty(ref _created, value); }
        }

        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            protected set { SetProperty(ref _lastUpdated, value); }
        }

        public String Filename
        {
            get { return _filename; }
            protected set { SetProperty(ref _filename, value); }
        }

        public String FullPath
        {
            get { return _fullPath; }
            protected set { SetProperty(ref _fullPath, value); }
        }

        public Int64 FileSize
        {
            get { return _fileSize; }
            protected set { SetProperty(ref _fileSize, value); }
        }

        public BulkObservableCollection<LogLineData> Lines
        {
            get { return _lines; }
            protected set { SetProperty(ref _lines, value); }
        }

        #endregion

        #region Constructor

        public LogFileData(String filename)
        {
            LoadFile(filename);
        }

        #endregion

        #region Methods

        public void LoadFile(String filename)
        {
            Filename = Path.GetFileName(filename);
            FullPath = filename;

            if (_watch != null)
            {
                _watch.Changed -= watch_Changed;
            }

            _watch = new FileSystemWatcher();

            _watch.Path = Path.GetDirectoryName(filename);
            _watch.Filter = Filename;
            _watch.NotifyFilter = NotifyFilters.LastWrite;
            _watch.Changed += watch_Changed;
            _watch.EnableRaisingEvents = true;

            FileInfo info = new FileInfo(filename);
            Created = info.CreationTime;
            LastUpdated = info.LastWriteTime;
            FileSize = info.Length;

            var tempLines = FileHelper.ReadLines(filename);
            BulkObservableCollection<LogLineData> temp = new BulkObservableCollection<LogLineData>();

            temp.AddRange(tempLines, (line) => new LogLineData(line));

            Lines = temp;

            //create and start timer if null
            if (_timer != null)
                return;

            _timer = new Timer(50);
            _timer.Elapsed += _timer_Elapsed;
            _timer.AutoReset = false;
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FileInfo info = new FileInfo(FullPath);
            Created = info.CreationTime;
            LastUpdated = info.LastWriteTime;
            FileSize = info.Length;

            //We only care about the new lines so skip the old line count
            var newLines = FileHelper.ReadLines(FullPath, Lines.Count).ToList();

            //if less or the same lines then we have to read all lines again to
            //update the view
            if (newLines.Count == 0)
            {
                LoadFile(FullPath);//reload the collection
                return;
            }

            //if more then we just assume lines have been added and read from the point we stopped off

            var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>();

            dispatcher.RequestMainThreadAction(new Action(() =>
            {
                Lines.AddRange(newLines, (line) => new LogLineData(line));
            }));
        }

        void watch_Changed(object sender, FileSystemEventArgs e)
        {
            lock (_watchLock)
            {
                LastUpdated = DateTime.Now;

                if(!_timer.Enabled)
                {
                    _timer.Start();
                }
            }
        }

        #endregion
    }
}
