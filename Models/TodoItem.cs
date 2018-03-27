using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Models
{
    class TodoItem
    {
        private string private_id;
        public string id;
        public string title { get; set; }
        public string description { get; set; }
        public bool completed { get; set; }
        public DateTimeOffset duedate { get; set; }
        //public string coverImage { get; set; }

        public TodoItem(string title, string description, DateTimeOffset duedate)
        {
            this.private_id = Guid.NewGuid().ToString();
            this.id = this.private_id;
            this.title = title;
            this.description = description;
            this.completed = false;
            this.duedate = duedate;
        }
    }
}
