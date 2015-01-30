using ND.Controls.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ND.Controls
{
    [TemplatePart(Name = "PART_ColorCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_BlackCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Selector", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_HueSelector", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_HueCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_PreviousColor", Type = typeof(UIElement))]
    [TemplatePart(Name = "PART_InitialColor", Type = typeof(UIElement))]
    public class ColorPicker : Control
    {
        #region Fields

        #region TemplatedParts

        private Canvas _colorCanvas;
        private Canvas _bwCanvas;
        private Canvas _selector;
        private Canvas _hueCanvas;
        private Canvas _hueSelector;
        private UIElement _previousColorElement;
        private UIElement _initialColorElement;

        #endregion

        private Boolean _suppressColorChanged = false;
        private Boolean _suppressPosChanged = false;
        private Boolean _suppressValueChanged = false;

        private Double _selectorXValue = 0.0;
        private Double _selectorYValue = 1.0;

        private Double _hueSelectorValue = 0.0;

        private Boolean _isDragging = false;

        #endregion

        #region Properties

        #region Selector Properties

        public Double SelectorXValue
        {
            get { return _selectorXValue; }
            set
            {
                if (_selectorXValue != value)
                {
                    _selectorXValue = value;
                    SelectorXValue_Changed(value);
                }
            }
        }

        private void SelectorXValue_Changed(Double newValue)
        {
            if (_suppressValueChanged || _colorCanvas == null)
                return;

            _suppressPosChanged = true;
            SelectorXPos = newValue * _colorCanvas.ActualWidth;
            _suppressPosChanged = false;
        }

        public Double SelectorYValue
        {
            get { return _selectorYValue; }
            set
            {
                if (_selectorYValue != value)
                {
                    _selectorYValue = value;
                    SelectorYValue_Changed(value);
                }
            }
        }

        private void SelectorYValue_Changed(Double newValue)
        {
            if (_suppressValueChanged || _colorCanvas == null)
                return;

            _suppressPosChanged = true;
            SelectorYPos = newValue * _colorCanvas.ActualHeight;
            _suppressPosChanged = false;
        }

        public Double HueSelectorValue
        {
            get { return _hueSelectorValue; }
            set
            {
                if (_hueSelectorValue != value)
                {
                    _hueSelectorValue = value;
                    HueSelectorValue_Changed(value);
                }
            }
        }

        private void HueSelectorValue_Changed(Double newValue)
        {
            if (_suppressValueChanged || _hueCanvas == null)
                return;

            _suppressPosChanged = true;
            HueSelectorPos = newValue * _hueCanvas.ActualHeight;
            _suppressPosChanged = false;
        }

        #endregion

        #endregion

        #region Dependency Properties

        #region Color Values

        public Color CurrentHue
        {
            get { return (Color)GetValue(CurrentHueProperty); }
            set { SetValue(CurrentHueProperty, value); }
        }

        public static readonly DependencyProperty CurrentHueProperty =
            DependencyProperty.Register("CurrentHue", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Red));

        public Color CurrentSelectedColor
        {
            get { return (Color)GetValue(CurrentSelectedColorProperty); }
            set { SetValue(CurrentSelectedColorProperty, value); }
        }

        public static readonly DependencyProperty CurrentSelectedColorProperty =
            DependencyProperty.Register("CurrentSelectedColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Black, CurrentSelectedColorProperty_Changed));

        private static void CurrentSelectedColorProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker cp = d as ColorPicker;

            if (cp != null && e.NewValue != null)
            {
                cp.CurrentSelectedColor_Changed((Color)e.NewValue);
            }
        }

        private void CurrentSelectedColor_Changed(Color newValue)
        {
            if (_suppressColorChanged)
                return;

            var hsv = ColorHelper.ConvertRGBToHSV(newValue);
            HueSelectorValue = 1.0 - (hsv.H / 360);
            SelectorXValue = hsv.S;
            SelectorYValue = 1 - hsv.V;
            CalculateHue();
        }

        public Color PreviousSelectedColor
        {
            get { return (Color)GetValue(PreviousSelectedColorProperty); }
            set { SetValue(PreviousSelectedColorProperty, value); }
        }

        public static readonly DependencyProperty PreviousSelectedColorProperty =
            DependencyProperty.Register("PreviousSelectedColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Black));

        #endregion

        #region Selector Properties

        public Double SelectorXPos
        {
            get { return (Double)GetValue(SelectorXPosProperty); }
            set { SetValue(SelectorXPosProperty, value); }
        }

        public static readonly DependencyProperty SelectorXPosProperty =
            DependencyProperty.Register("SelectorXPos", typeof(Double), typeof(ColorPicker), new PropertyMetadata(0.0, SelectorXPosProperty_Changed));

        private static void SelectorXPosProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker cp = d as ColorPicker;
            if (cp != null && e.NewValue != null)
            {
                cp.SelectorXPos_Changed((Double)e.NewValue);
            }
        }

        private void SelectorXPos_Changed(Double newValue)
        {
            if (_suppressPosChanged)
                return;

            if (_colorCanvas.ActualWidth != 0)
            {
                SelectorXValue = newValue / _colorCanvas.ActualWidth;
            }
            else
            {
                SelectorXValue = 0.0;
            }
        }

        public Double SelectorYPos
        {
            get { return (Double)GetValue(SelectorYPosProperty); }
            set { SetValue(SelectorYPosProperty, value); }
        }

        public static readonly DependencyProperty SelectorYPosProperty =
            DependencyProperty.Register("SelectorYPos", typeof(Double), typeof(ColorPicker), new PropertyMetadata(0.0, SelectorYPosProperty_Changed));

        private static void SelectorYPosProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker cp = d as ColorPicker;
            if (cp != null && e.NewValue != null)
            {
                cp.SelectorYPos_Changed((Double)e.NewValue);
            }
        }

        private void SelectorYPos_Changed(Double newValue)
        {
            if (_suppressPosChanged)
                return;

            if (_colorCanvas.ActualHeight != 0)
            {
                SelectorYValue = newValue / _colorCanvas.ActualHeight;
            }
            else
            {
                SelectorYValue = 0.0;
            }
        }

        public Double HueSelectorPos
        {
            get { return (Double)GetValue(HueSelectorPosProperty); }
            set { SetValue(HueSelectorPosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HueSelectorPos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HueSelectorPosProperty =
            DependencyProperty.Register("HueSelectorPos", typeof(Double), typeof(ColorPicker), new PropertyMetadata(0.0, HueSelectorPosProperty_Changed));

        private static void HueSelectorPosProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker cp = d as ColorPicker;
            if (cp != null && e.NewValue != null)
            {
                cp.HueSelectorPos_Changed((Double)e.NewValue);
            }
        }

        private void HueSelectorPos_Changed(double newValue)
        {
            if (_suppressPosChanged)
                return;

            if (_hueCanvas.ActualHeight != 0)
            {
                HueSelectorValue = newValue / _hueCanvas.ActualHeight;
            }
            else
            {
                HueSelectorValue = 0.0;
            }
        }

        #endregion

        #endregion

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _colorCanvas = Template.FindName("PART_ColorCanvas", this) as Canvas;
            _bwCanvas = Template.FindName("PART_BlackCanvas", this) as Canvas;
            _selector = Template.FindName("PART_Selector", this) as Canvas;
            _hueCanvas = Template.FindName("PART_HueCanvas", this) as Canvas;
            _hueSelector = Template.FindName("PART_HueSelector", this) as Canvas;
            _previousColorElement = Template.FindName("PART_PreviousColor", this) as UIElement;
            _initialColorElement = Template.FindName("PART_InitialColor", this) as UIElement;

            if (_colorCanvas == null)
                throw new ArgumentNullException("PART_ColorCanvas has not been supplied in the template or is not of type Canvas");
            if (_hueCanvas == null)
                throw new ArgumentException("PART_HueCanvas has not been supplied in the template or is not of type Canvas");

            //Register for color canvas changes
            _colorCanvas.SizeChanged += _colorCanvas_SizeChanged;
            _colorCanvas.MouseDown += _colorCanvas_MouseDown;
            _colorCanvas.MouseMove += _colorCanvas_MouseMove;
            _colorCanvas.MouseUp += _colorCanvas_MouseUp;

            //Register for hue canvas changes
            _hueCanvas.SizeChanged += _hueCanvas_SizeChanged;
            _hueCanvas.MouseDown += _hueCanvas_MouseDown;
            _hueCanvas.MouseMove += _hueCanvas_MouseMove;
            _hueCanvas.MouseUp += _hueCanvas_MouseUp;

            if(_previousColorElement != null)
            {
                _previousColorElement.MouseUp += _previousColorElement_MouseUp;
            }

            if(_initialColorElement != null)
            {
                _initialColorElement.MouseUp += _initialColorElement_MouseUp;
            }
        }

        void _initialColorElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void _previousColorElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            CurrentSelectedColor = PreviousSelectedColor;
        }

        #endregion

        #region Handlers

        #region Color Canvas

        void _colorCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickPos = e.GetPosition(_colorCanvas);

            PreviousSelectedColor = CurrentSelectedColor;

            HandleColorCanvasDrag(clickPos);
            _colorCanvas.CaptureMouse();
            _isDragging = true;
        }

        void _colorCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var clickPos = e.GetPosition(_colorCanvas);

            HandleColorCanvasDrag(clickPos);
            _colorCanvas.ReleaseMouseCapture();
            _isDragging = false;
        }

        void _colorCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                var clickPos = e.GetPosition(_colorCanvas);
                HandleColorCanvasDrag(clickPos);
            }
        }

        private void HandleColorCanvasDrag(Point clickPos)
        {
            _suppressValueChanged = true;
            SelectorXPos = MathHelper.Constrain(clickPos.X, 0, _colorCanvas.ActualWidth);
            SelectorYPos = MathHelper.Constrain(clickPos.Y, 0, _colorCanvas.ActualHeight);
            _suppressValueChanged = false;

            CalculateColor();
        }

        void _colorCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _suppressValueChanged = true;
            SelectorXPos = _selectorXValue * e.NewSize.Width;
            SelectorYPos = _selectorYValue * e.NewSize.Height;
            _suppressValueChanged = false;
        }

        #endregion

        #region HueCanvas

        void _hueCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickPos = e.GetPosition(_colorCanvas);

            PreviousSelectedColor = CurrentSelectedColor;

            _hueCanvas.CaptureMouse();
            HandleHueCanvasDrag(clickPos);
            _isDragging = true;
        }

        private void HandleHueCanvasDrag(Point clickPos)
        {
            _suppressValueChanged = true;
            HueSelectorPos = MathHelper.Constrain(clickPos.Y, 0, _hueCanvas.ActualHeight);
            _suppressValueChanged = false;

            CalculateHue();
            CalculateColor();
        }

        void _hueCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var clickPos = e.GetPosition(_colorCanvas);

            HandleHueCanvasDrag(clickPos);
            _hueCanvas.ReleaseMouseCapture();
            _isDragging = false;
        }

        void _hueCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                var clickPos = e.GetPosition(_hueCanvas);
                HandleHueCanvasDrag(clickPos);
            }
        }

        void _hueCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _suppressValueChanged = true;
            HueSelectorPos = _hueSelectorValue * e.NewSize.Height;
            _suppressValueChanged = false;
        }

        #endregion

        #endregion

        #region Methods

        void CalculateHue()
        {
            Double h = (1.0 - HueSelectorValue) * 360;
            h = h > 359 ? 0 : h;

            CurrentHue = ColorHelper.ConvertHSVToRGB(h, 1.0, 1.0);
        }

        void CalculateColor()
        {
            Double h = (1.0 - HueSelectorValue) * 360;
            h = h > 359 ? 0 : h;

            Double s = SelectorXValue;
            Double v = 1 - SelectorYValue;

            var result = ColorHelper.ConvertHSVToRGB(h, s, v);

            _suppressColorChanged = true;
            CurrentSelectedColor = result;
            _suppressColorChanged = false;
        }

        #endregion


    }
}
