using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ND.Controls.Helpers
{
    public class MathHelper
    {
        public static Double Constrain(Double value, Double min, Double max)
        {
            if (min > max)
                throw new ArgumentException("Min value cannot be greater than max value");

            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }
    }
}
