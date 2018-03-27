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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
        }

        private ViewModels.TodoItemViewModel ViewModel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ViewModel = (ViewModels.TodoItemViewModel)e.Parameter;
            if (ViewModel.SelectedItem == null)
            {
                Create.Content = "Create";
            }
            else
            {
                Create.Content = "Update";
                Create.Click -= CreateClick;
                Create.Click += Update;
                textTitle.Text = ViewModel.SelectedItem.title;
                textDetail.Text = ViewModel.SelectedItem.description;
                DueDate.Date = ViewModel.SelectedItem.duedate;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            textTitle.Text = "";
            textDetail.Text = "";
            DueDate.Date = DateTimeOffset.Now;
            if ((string)Create.Content == "Update")
            {
                Create.Content = "Create";
                Create.Click -= Update;
                Create.Click += CreateClick;
            }
        }

        private async void CreateClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "提示信息",
                PrimaryButtonText = "确定"
            };

            if (textTitle.Text == "")
            {
                dialog.Content = "标题不能为空";
                await dialog.ShowAsync();
            }
            else if (textDetail.Text == "")
            {
                dialog.Content = "内容主体不能为空";
                await dialog.ShowAsync();
            }
            else if (DueDate.Date < DateTimeOffset.Now)
            {
                dialog.Content = "日期不正确";
                await dialog.ShowAsync();
            }
            else
            {
                ViewModel.AddTodoItem(textTitle.Text, textDetail.Text, DueDate.Date);
                textTitle.Text = "";
                textDetail.Text = "";
                DueDate.Date = DateTimeOffset.Now;
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }

        private async void Update(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "提示信息",
                PrimaryButtonText = "确定"
            };

            if (textTitle.Text == "")
            {
                dialog.Content = "标题不能为空";
                await dialog.ShowAsync();
            }
            else if (textDetail.Text == "")
            {
                dialog.Content = "内容主体不能为空";
                await dialog.ShowAsync();
            }
            else if (DueDate.Date < DateTimeOffset.Now)
            {
                dialog.Content = "日期不正确";
                await dialog.ShowAsync();
            }
            else
            {
                ViewModel.UpdateTodoItem(textTitle.Text, textDetail.Text, DueDate.Date);
                textTitle.Text = "";
                textDetail.Text = "";
                DueDate.Date = DateTimeOffset.Now;
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
    }
}
