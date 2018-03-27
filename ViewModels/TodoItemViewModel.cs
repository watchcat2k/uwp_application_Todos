using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
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

        public TodoItemViewModel()
        {
            this.allItems.Add(new Models.TodoItem("title1", "description1", DateTimeOffset.Now));
            this.allItems.Add(new Models.TodoItem("title2", "description2", DateTimeOffset.Now));
        }

        public void AddTodoItem(string title, string description, DateTimeOffset duedate)
        {
            this.allItems.Add(new Models.TodoItem(title, description, duedate));
        }

        public void RemoveTodoItem(string id)
        {
            //this.selectedItem = null;
        }

        public void UpdateTodoItem(string title, string description, DateTimeOffset duedate)
        {
            this.selectedItem.title = title;
            this.selectedItem.description = description;
            this.selectedItem.duedate = duedate;
            this.selectedItem = null;
        }
    }
}
