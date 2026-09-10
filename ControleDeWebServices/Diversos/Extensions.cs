using System;

namespace ControleDeWebServices.Diversos
{
    public static class Extensions
    {
        public static int asInt(this object pValue)
        {
            return string.IsNullOrEmpty(Convert.ToString(pValue))? 0: Convert.ToInt32(pValue);
        }

        public static int asIFElse(this string pValue)
        {
            return string.IsNullOrEmpty(pValue) ? 0 : Convert.ToInt32(pValue);
        }
    }
}
