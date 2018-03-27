using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Models
{
    class TodoItem
    {
        private string id;
        public string title { get; set; }
        public string description { get; set; }
        public bool completed { get; set; }
        public DateTime duedate { get; set; }
        //public string coverImage { get; set; }

        public TodoItem(string title, string description, DateTime duedate)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.completed = false;
            this.duedate = duedate;
        }
    }
}
