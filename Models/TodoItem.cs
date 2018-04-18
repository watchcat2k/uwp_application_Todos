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
        public string id { get { return this.private_id; } set { this.private_id = value; } }
        public string title { get; set; }
        public string description { get; set; }
        private bool? private_completed;
        public bool? completed { get { return this.private_completed; } set { this.private_completed = value; } }
        public DateTimeOffset duedate { get; set; }
        public Uri imauri { get; set; }
        public BitmapImage coverImage { get; set; }

        public static string defaultImagePath = "ms-appx:///Assets/banana.png";

        public TodoItem(string title, string description, DateTimeOffset duedate, BitmapImage coverImage, Uri imauri)
        {
            this.private_id = Guid.NewGuid().ToString();
            this.id = this.private_id;
            this.title = title;
            this.description = description;
            this.private_completed = false;
            this.coverImage = coverImage;
            this.duedate = duedate;
            this.imauri = imauri;
        }

        public TodoItem(string id, string title, string description, DateTimeOffset duedate, BitmapImage coverImage, Uri imauri)
        {
            this.private_id = id;
            this.id = this.private_id;
            this.title = title;
            this.description = description;
            this.coverImage = coverImage;
            this.duedate = duedate;
            this.private_completed = false;
            this.imauri = imauri;
        }

        public static string dateTimeToString(DateTimeOffset date)
        {
            return date.ToString();
        }
        public static DateTimeOffset stringToDateTime(string datestring)
        {
            return DateTimeOffset.Parse(datestring);
        }

        public static Uri stringToUri(string uristr)
        {
            return new Uri(uristr);
        }

        public static string uriToString(Uri imageUri)
        {
            return imageUri.ToString();
        }

    }
}
