using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ControleDeWebServices.Diversos
{
    class Funcoes
    {
        public static void AtivarDesativarControles(UIElement pControle, Boolean pAtivado)
        {
            IEnumerable<UIElement> elements = FindVisualChildren<UIElement>(pControle);
            foreach (UIElement element in elements)
            {
                if (element is TextBox)
                {
                    AtivarControle(element, pAtivado);
                }

                else if (element is Button)
                {
                    AtivarControle(element, pAtivado);
                }
                else if (element is ComboBox)
                {
                    AtivarControle(element, pAtivado);
                }
                else if (element is PasswordBox)
                {
                    AtivarControle(element, pAtivado);
                }

            }
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private static void AtivarControle(UIElement element, bool pAtivado)
        {
            if (element.IsEnabled == !pAtivado)
            {
                element.IsEnabled = pAtivado;
            }
            else if (element.IsEnabled == pAtivado)
            {
                element.IsEnabled = !pAtivado;
            }
        }

        public static void LimparCampos(UIElement pControle)
        {
            IEnumerable<UIElement> elements = FindVisualChildren<UIElement>(pControle);
            foreach (UIElement element in elements)
            {
                if (element is TextBox) 
                {
                    ((TextBox)element).Text = string.Empty;
                }
                else if (element is ComboBox)
                {
                    ((ComboBox)element).SelectedIndex = -1;
                }
                else if (element is PasswordBox)
                {
                    ((PasswordBox)element).Password = string.Empty;
                }
            }
        }
        public static List<Estados> GetEnumValues()
        {
            Type t_type = typeof(Estados);

            FieldInfo[] field_infos = t_type.GetFields();

            List<Estados> results = new List<Estados>();
            foreach (FieldInfo field_info in field_infos)
            {
                if (field_info.IsLiteral)
                {
                    Estados value = (Estados)field_info.GetValue(null);
                    results.Add(value);
                }
            }
            return results;
        }
    }
}
