using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos.ViewModels
{

    class TodoItemViewModel
    {
        private static ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return allItems; } }

        private Models.TodoItem selectedItem = null;
        public Models.TodoItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                this.selectedItem = value;
            }
        }

        private TodoItemViewModel()
        {
            allItems.Add(new Models.TodoItem("title1", "description1", DateTimeOffset.Now, new BitmapImage(new Uri("ms-appx:///Assets/banana.png"))));
            allItems.Add(new Models.TodoItem("title2", "description2", DateTimeOffset.Now, new BitmapImage(new Uri("ms-appx:///Assets/banana.png"))));
        }

        public static ObservableCollection<Models.TodoItem> GetInstance()
        {
            return allItems;
        }

        public void AddTodoItem(string title, string description, DateTimeOffset duedate, BitmapImage coverImage)
        {
            allItems.Add(new Models.TodoItem(title, description, duedate, coverImage));
        }

        public void RemoveTodoItem(string title, string description, DateTimeOffset duedate)
        {
            allItems.Remove(this.selectedItem);
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, DateTimeOffset duedate, BitmapImage coverImage)
        {
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.duedate = duedate;
            this.selectedItem.coverImage = coverImage;
            this.selectedItem = null;
        }
    }
}
