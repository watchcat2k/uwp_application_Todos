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

        public void AddTodoItem(string title, string description, DateTime duedate)
        {
            this.allItems.Add(new Models.TodoItem(title, description, duedate));
        }

        public void RemoveTodoItem(string id)
        {
            //this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string description)
        {
            //this.selectedItem = null;
        }
    }
}
