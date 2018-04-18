using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Popups;
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
            Uri imageUri = new Uri("ms-appx:///Assets/banana.png");
            this.ImageStreamRef = RandomAccessStreamReference.CreateFromUri(imageUri);

            tileCreate();
        }

        private StorageFile imageFile = null;
        private Uri imauri = new Uri(Models.TodoItem.defaultImagePath);
        ViewModels.TodoItemViewModel ViewModel { get; set; }
        public RandomAccessStreamReference ImageStreamRef { get; private set; }

        private void tileCreate()
        {
            Windows.Data.Xml.Dom.XmlDocument document = new Windows.Data.Xml.Dom.XmlDocument();
            document.LoadXml(System.IO.File.ReadAllText("XMLFile1.xml"));
            Windows.Data.Xml.Dom.XmlNodeList Texttitle = document.GetElementsByTagName("text");

            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            for (int i = 0; i < ViewModel.AllItems.Count; i++)
            {
                if (i < 5)
                {
                    Texttitle[0].InnerText = Texttitle[2].InnerText = Texttitle[4].InnerText = ViewModel.AllItems[i].title;
                    Texttitle[1].InnerText = Texttitle[3].InnerText = Texttitle[5].InnerText = ViewModel.AllItems[i].description;
                    TileNotification newTile = new TileNotification(document);
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(newTile);
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.SelectedItem = null;
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("mainpage");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values["image"] != null)
                {
                    StorageFile temp;
                    temp = await StorageApplicationPermissions.FutureAccessList.GetFileAsync((string)ApplicationData.Current.LocalSettings.Values["image"]);
                    IRandomAccessStream ir = await temp.OpenAsync(FileAccessMode.Read);
                    BitmapImage bi = new BitmapImage();
                    await bi.SetSourceAsync(ir);
                    MyImage.Source = bi;
                    ApplicationData.Current.LocalSettings.Values["image"] = null;
                }

                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mainpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["mainpage"] as ApplicationDataCompositeValue;
                    for (int i = 0; i < ViewModel.AllItems.Count(); i++)
                    {
                        ViewModel.AllItems[i].completed = (bool)composite["ischecked" + i];
                    }
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
                for (int i = 0; i < ViewModel.AllItems.Count(); i++)
                {
                    composite["ischecked" + i] = ViewModel.AllItems[i].completed;
                }
                composite["title"] = textTitle.Text;
                composite["detail"] = textDetail.Text;
                composite["date"] = DueDate.Date;
                ApplicationData.Current.LocalSettings.Values["mainpage"] = composite;
            }
            foreach (var item in this.ViewModel.AllItems)
            {
                Services.DbContext.UpdateData(item.id, item.title, item.description, item.duedate, item.coverImage, item.imauri, item.completed);
            }
        }

        private void checkBox1Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void navToButton(object sender, RoutedEventArgs e)
        {
            if (backgroundGrid.ActualWidth <= 800)
                this.Frame.Navigate(typeof(NewPage));
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
                Share.Visibility = Visibility.Collapsed;
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
                if (imageFile != null)
                {
                    string imageName = imageFile.Name;
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile newImageFile = await imageFile.CopyAsync(localFolder, imageName, NameCollisionOption.ReplaceExisting);
                    imauri = new Uri(newImageFile.Path);
                }

                ViewModel.AddTodoItem(textTitle.Text, textDetail.Text, DueDate.Date, MyImage.Source as BitmapImage, imauri);
                textTitle.Text = "";
                textDetail.Text = "";
                DueDate.Date = DateTimeOffset.Now;
                tileCreate();
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            if (backgroundGrid.ActualWidth <= 800)
            {
                Frame.Navigate(typeof(NewPage));
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
                    Share.Visibility = Visibility.Visible;
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
                if (imageFile != null)
                {
                    string imageName = imageFile.Name;
                    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                    StorageFile newImageFile = await imageFile.CopyAsync(localFolder, imageName, NameCollisionOption.ReplaceExisting);
                    imauri = new Uri(newImageFile.Path);
                }

                ViewModel.UpdateTodoItem(textTitle.Text, textDetail.Text, DueDate.Date, MyImage.Source as BitmapImage, imauri);
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
                Frame.Navigate(typeof(MainPage));
                tileCreate();
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
                Share.Visibility = Visibility.Collapsed;
            }
            tileCreate();
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
                ApplicationData.Current.LocalSettings.Values["image"] = StorageApplicationPermissions.FutureAccessList.Add(file);
                imageFile = file;
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {

                    await srcImage.SetSourceAsync(stream);
                    MyImage.Source = srcImage;
                }
            }
        }

        private void shareButton(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;

            request.Data.SetText("share");
            request.Data.Properties.Title = ViewModel.SelectedItem.title;
            request.Data.Properties.Description = ViewModel.SelectedItem.description;
            request.Data.SetBitmap(ImageStreamRef);
        }

        private async void searchButtonClick(object sender, RoutedEventArgs e)
        {
            String str = "";
            bool isFound = false;
            if (search.Text.Length == 0) return;
            foreach (var item in this.ViewModel.AllItems)
            {
                if (item.title.Contains(search.Text))
                {
                    str = str + "Title: " + item.title + "  Description: "
                        + item.description + "  Time: " + item.duedate.ToString() + "\n";
                    isFound = true;
                }
            }
            if (isFound)
            {
                var i = await new MessageDialog(str.ToString()).ShowAsync();
            }
            else
            {
                var i = await new MessageDialog("No Such Item!").ShowAsync();
            }
        }
    }
}
