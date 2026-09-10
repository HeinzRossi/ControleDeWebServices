using System;
using System.Reflection;

namespace ControleDeWebServices.Diversos
{
    public static class ReflectionDataGrid
    {
        public static T CastObject<T>(object pObject) where T : new()
        {
            var novoObjeto = new T();
            PropertyInfo[] properties = pObject.GetType().GetProperties();
            foreach (var item in properties)
            {
                try
                {
                    if (novoObjeto.GetType().GetProperty(item.Name) == null)
                        continue;
                    novoObjeto.GetType().GetProperty(item.Name).SetValue(novoObjeto,item.GetValue(pObject));                
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return novoObjeto;
        }
    }
}
