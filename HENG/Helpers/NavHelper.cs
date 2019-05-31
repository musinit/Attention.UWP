﻿using Windows.UI.Xaml;
using muxc = Microsoft.UI.Xaml.Controls;

namespace HENG.Helpers
{
    public class NavHelper
    {
        public static string GetNavigateTo(muxc.NavigationViewItem item)
        {
            return (string)item.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(muxc.NavigationViewItem item, string value)
        {
            item.SetValue(NavigateToProperty, value);
        }

        public static readonly DependencyProperty NavigateToProperty =
            DependencyProperty.RegisterAttached("NavigateTo", typeof(string), typeof(NavHelper), new PropertyMetadata(null));
    }
}
