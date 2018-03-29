using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos.Models
{
    class TodoItem
    {
        private string private_id;
        public string id;
        public string title { get; set; }
        public string description { get; set; }
        private bool? private_completed;
        public bool? completed { get { return this.private_completed; } set { this.private_completed = value; } }
        public DateTimeOffset duedate { get; set; }
        public BitmapImage coverImage { get; set; }

        public TodoItem(string title, string description, DateTimeOffset duedate, BitmapImage coverImage)
        {
            this.private_id = Guid.NewGuid().ToString();
            this.id = this.private_id;
            this.title = title;
            this.description = description;
            this.private_completed = false;
            this.coverImage = coverImage;
            this.duedate = duedate;
        }
    }
}
