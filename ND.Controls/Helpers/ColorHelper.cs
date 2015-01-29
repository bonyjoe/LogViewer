using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ND.Controls.Helpers
{
    public class ColorHelper
    {
        /// <summary>
        /// Converts HSV values to an RGB color
        /// </summary>
        /// <param name="h">hue value</param>
        /// <param name="s">saturation value</param>
        /// <param name="v">value</param>
        /// <returns>Color object</returns>
        public static Color ConvertHSVToRGB(Double h, Double s, Double v)
        {
            Double r = 0.0;
            Double g = 0.0;
            Double b = 0.0;

            var chroma = s * v;
            var hdash = h / 60;
            var x = chroma * (1.0 - Math.Abs((hdash % 2) - 1.0));

            if (hdash < 1.0)
            {
                r = chroma;
                g = x;
            }
            else if (hdash < 2.0)
            {
                r = x;
                g = chroma;
            }
            else if (hdash < 3.0)
            {
                g = chroma;
                b = x;
            }
            else if (hdash < 4.0)
            {
                g = x;
                b = chroma;
            }
            else if (hdash < 5.0)
            {
                r = x;
                b = chroma;
            }
            else
            {
                r = chroma;
                b = x;
            }

            var modifier = v - chroma;
            r += modifier;
            g += modifier;
            b += modifier;

            var result = Color.FromRgb((Byte)(r * 255), (Byte)(g * 255), (Byte)(b * 255));
            return result;
        }

        public static HSVColor ConvertRGBToHSV(Color rgbColor)
        {
            Double min;
            Double max;
            Double chroma;
            HSVColor result = new HSVColor();

            min = Math.Min(Math.Min(rgbColor.ScR, rgbColor.ScG), rgbColor.ScB);
            max = Math.Max(Math.Max(rgbColor.ScR, rgbColor.ScG), rgbColor.ScB);
            chroma = max - min;

            if(chroma != 0)
            {
                if(rgbColor.ScR == max)
                {
                    result.H = (rgbColor.ScG - rgbColor.ScB) / chroma;
                    if (result.H < 0.0)
                    {
                        result.H += 6;
                    }
                }
                else if(rgbColor.ScG == max)
                {
                    result.H = ((rgbColor.ScB - rgbColor.ScR) / chroma) + 2.0;
                }
                else
                {
                    result.H = ((rgbColor.ScR - rgbColor.ScG) / chroma) + 4.0;
                }

                result.H *= 60.0;
                result.S = chroma / max;
            }

            result.V = max;

            return result;
        }
    }

    public class HSVColor
    {
        public Double H { get; set; }
        public Double S { get; set; }
        public Double V { get; set; }

        public HSVColor()
        {
            this.H = 0.0;
            this.S = 0.0;
            this.V = 0.0;
        }

        public HSVColor(Double h, Double s, Double v)
        {
            this.H = h;
            this.S = s;
            this.V = v;
        }
    }
}
