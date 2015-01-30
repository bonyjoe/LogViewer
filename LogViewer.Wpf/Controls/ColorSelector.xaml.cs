using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LogViewer.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : UserControl, INotifyPropertyChanged
    {
        private static List<Color> _defaultAvailableColors = new List<Color>()
            {
                Colors.Black,
                Colors.White,
                Colors.Red,
                Colors.Blue,
                Colors.Green
            };

        private Int32 _selectedColorIndex = -1;
        private Color _customColor = Colors.White;

        #region Properties

        public Int32 SelectedColorIndex
        {
            get { return _selectedColorIndex; }
            set
            {
                SetProperty(ref _selectedColorIndex, value);
                OnSelectedColorIndexChanged();
            }
        }

        public Color CustomColor
        {
            get { return _customColor; }
            set
            {
                SetProperty(ref _customColor, value);
                OnCustomColorChanged();
            }
        }

        #endregion

        #region DependencyProperties

        public IEnumerable<Color> AvailableColors
        {
            get { return (IEnumerable<Color>)GetValue(AvailableColorsProperty); }
            set { SetValue(AvailableColorsProperty, value); }
        }

        public static readonly DependencyProperty AvailableColorsProperty =
            DependencyProperty.Register("AvailableColors", typeof(IEnumerable<Color>), typeof(ColorSelector), new PropertyMetadata(_defaultAvailableColors));

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorSelector), new PropertyMetadata(null));

        #endregion

        public ColorSelector()
        {
            InitializeComponent();
        }

        public void OnSelectedColorIndexChanged()
        {
            if (SelectedColorIndex != -1)
            {
                SelectedColor = AvailableColors.ElementAt(SelectedColorIndex);
            }
        }

        public void OnCustomColorChanged()
        {
            SelectedColorIndex = -1;
            SelectedColor = CustomColor;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            colorPopup.IsOpen = true;
        }
    }
}
