using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeWebServices.Diversos
{
    public static class ConverterListaGenerica
    {
        public static T ToType<T>(this object obj)
        {
            T tmp = (T)Activator.CreateInstance(typeof(T));

            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                try
                {
                    tmp.GetType().GetProperty(pi.Name).SetValue(tmp, pi.GetValue(obj, null), null);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return tmp;
        }
    }
}
