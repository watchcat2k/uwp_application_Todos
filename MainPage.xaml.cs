using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void checkBox1Click(object sender, RoutedEventArgs e)
        {
            if (line1.Visibility == Visibility.Visible)
            {
                line1.Visibility = Visibility.Collapsed;
            }
            else
            {
                line1.Visibility = Visibility.Visible;
            }
        }

        private void navToButton(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewPage));
        }
    }
}
