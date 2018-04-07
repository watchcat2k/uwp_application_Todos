using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public class LineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool ischecked = (bool)value;
            if (ischecked)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class isCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool ischecked = (bool)value;
            if (ischecked)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = ViewModels.TodoItemViewModel.GetInstance();
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)e.Parameter;
            }

            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("mainpage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mainpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["mainpage"] as ApplicationDataCompositeValue;
                    textTitle.Text = (string)composite["title"];
                    textDetail.Text = (string)composite["detail"];
                    DueDate.Date = (DateTimeOffset)composite["date"];

                    ApplicationData.Current.LocalSettings.Values.Remove("mainpage");
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = textTitle.Text;
                composite["detail"] = textDetail.Text;
                composite["date"] = DueDate.Date;
                ApplicationData.Current.LocalSettings.Values["mainpage"] = composite;
            }
        }

        private void checkBox1Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void navToButton(object sender, RoutedEventArgs e)
        {
            if (backgroundGrid.ActualWidth <= 800)
                this.Frame.Navigate(typeof(NewPage), ViewModel);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            textTitle.Text = "";
            textDetail.Text = "";
            DueDate.Date = DateTimeOffset.Now;
            if ((string)Create.Content == "Update")
            {
                Create.Content = "Create";
                Create.Click -= CreateClick;
                Create.Click -= Update;
                Create.Click += CreateClick;
                Delete.Visibility = Visibility.Collapsed;
                ViewModel.SelectedItem = null;
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
            else if (DueDate.Date < DateTime.Now)
            {
                dialog.Content = "日期不正确";
                await dialog.ShowAsync();
            }
            else
            {
                ViewModel.AddTodoItem(textTitle.Text, textDetail.Text, DueDate.Date, MyImage.Source as BitmapImage);
                textTitle.Text = "";
                textDetail.Text = "";
                DueDate.Date = DateTimeOffset.Now;
                
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            if (backgroundGrid.ActualWidth <= 800)
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                if (ViewModel.SelectedItem == null)
                {
                    Create.Content = "Create";
                }
                else
                {
                    Delete.Visibility = Visibility.Visible;
                    Create.Content = "Update";
                    Create.Click -= CreateClick;
                    Create.Click -= Update;
                    Create.Click += Update;
                    textTitle.Text = ViewModel.SelectedItem.title;
                    textDetail.Text = ViewModel.SelectedItem.description;
                    DueDate.Date = ViewModel.SelectedItem.duedate;
                    MyImage.Source = ViewModel.SelectedItem.coverImage;
                }
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
                ViewModel.UpdateTodoItem(textTitle.Text, textDetail.Text, DueDate.Date, MyImage.Source as BitmapImage);
                textTitle.Text = "";
                textDetail.Text = "";
                DueDate.Date = DateTimeOffset.Now;
                if ((string)Create.Content == "Update")
                {
                    Create.Content = "Create";
                    Create.Click -= CreateClick;
                    Create.Click -= Update;
                    Create.Click += CreateClick;
                }
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }

        private void deleteButton(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveTodoItem(textTitle.Text, textDetail.Text, DueDate.Date);
            textTitle.Text = "";
            textDetail.Text = "";
            DueDate.Date = DateTimeOffset.Now;
            if ((string)Create.Content == "Update")
            {
                Create.Content = "Create";
                Create.Click -= CreateClick;
                Create.Click -= Update;
                Create.Click += CreateClick;
                Delete.Visibility = Visibility.Collapsed;
            }
        }

        private async void selectPitureClick(object sender, RoutedEventArgs e)
        {

            var srcImage = new BitmapImage();
            FileOpenPicker openPicker = new FileOpenPicker();
            //选择视图模式  
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List;  
            //初始位置  
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //添加文件类型  
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {

                    await srcImage.SetSourceAsync(stream);
                    MyImage.Source = srcImage;
                }
            }
        }
    }
}
