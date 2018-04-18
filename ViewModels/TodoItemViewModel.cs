using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using SQLitePCL;

namespace Todos.ViewModels
{

    class TodoItemViewModel
    {
        private static TodoItemViewModel instance;

        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

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
            this.allItems = Services.DbContext.getAllTodoItem();
        }

        public static TodoItemViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new TodoItemViewModel();
            }
            return instance;
        }

        public void AddTodoItem(string title, string description, DateTimeOffset duedate, BitmapImage coverImage, Uri imauri)
        {
            Models.TodoItem temp = new Models.TodoItem(title, description, duedate, coverImage, imauri);
            this.allItems.Add(temp);
            Services.DbContext.InsertData(temp.id, title, description, duedate, coverImage, imauri);
        }

        public void RemoveTodoItem(string title, string description, DateTimeOffset duedate)
        {
            this.allItems.Remove(this.selectedItem);
            Services.DbContext.DeleteData(this.selectedItem.id);
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, DateTimeOffset duedate, BitmapImage coverImage, Uri imauri)
        {
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.duedate = duedate;
            this.selectedItem.coverImage = coverImage;
            this.selectedItem.imauri = imauri;
            Services.DbContext.UpdateData(this.selectedItem.id, title, description, duedate, coverImage, imauri);
            this.selectedItem = null;
        }
    }
}
