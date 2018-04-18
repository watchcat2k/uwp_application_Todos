using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos.Services
{
    class DbContext
    {
        // string id, string title, string description, DateTimeOffset duedate, BitmapImage coverImage
        private static String DB_NAME = "SQLiteSample.db";
        private static String TABLE_NAME = "SampleTable";
        private static String SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (Key STRING PRIMARY KEY NOT NULL,Title VARCHAR(140) NOT NULL,Description TEXT,Date VARCHAR(140),Image VARCHAR(350), Completed TEXT);";
        private static string SQL_AllITEMS = "SELECT Key, Title, Description, Date, Image, Completed FROM " + TABLE_NAME;
        private static String SQL_INSERT = "INSERT INTO " + TABLE_NAME + "(Key, Title, Description, Date, Image, Completed) VALUES(?, ?, ?, ?, ?, ?)";
        private static String SQL_UPDATE = "UPDATE " + TABLE_NAME + " SET Title=?, Description=?, Date=?, Image=?, Completed=? WHERE Key = ?";
        private static String SQL_DELETE = "DELETE FROM " + TABLE_NAME + " WHERE Key = ?";
                                       
        public DbContext()
        {
            var conn = new SQLiteConnection(DB_NAME);
            using (var statement = conn.Prepare(SQL_CREATE_TABLE))
            {
                statement.Step();
            }
        }

        public static ObservableCollection<Models.TodoItem> getAllTodoItem()
        {
            ObservableCollection<Models.TodoItem> todoItemList = new ObservableCollection<Models.TodoItem>();
            var con = new SQLiteConnection(DB_NAME);
            var statement = con.Prepare(SQL_AllITEMS);
            while (statement.Step() == SQLiteResult.ROW)
            {
                todoItemList.Add(new Models.TodoItem((string)statement[0], (string)statement[1], (string)statement[2], Models.TodoItem.stringToDateTime((string)statement[3]),
                                                      new BitmapImage(new Uri((string)statement[4])), Models.TodoItem.stringToUri((string)statement[4]), (string)statement[5]));
            }
            return todoItemList;
        }

        public static bool InsertData(string key, string title, string description, DateTimeOffset date, BitmapImage image, Uri imauri)
        {
            var conn = new SQLiteConnection(DB_NAME);
            try
            {
                using (var todo = conn.Prepare(SQL_INSERT))
                {
                    todo.Bind(1, key);
                    todo.Bind(2, title);
                    todo.Bind(3, description);
                    todo.Bind(4, Models.TodoItem.dateTimeToString(date));
                    todo.Bind(5, Models.TodoItem.uriToString(imauri));
                    todo.Step();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static void UpdateData(string key, string title, string description, DateTimeOffset date, BitmapImage image, Uri imauri, bool? comp)
        {
            var connection = new SQLiteConnection(DB_NAME);
            using (var todo = connection.Prepare(SQL_UPDATE))
            {
                todo.Bind(1, title);
                todo.Bind(2, description);
                todo.Bind(3, Models.TodoItem.dateTimeToString(date));
                todo.Bind(4, Models.TodoItem.uriToString(imauri));
                todo.Bind(5, comp.ToString());
                todo.Bind(6, key);
                todo.Step();
            }
        }

        public static void DeleteData(string key)
        {
            var connection = new SQLiteConnection(DB_NAME);
            using (var todo = connection.Prepare(SQL_DELETE))
            {
                todo.Bind(1, key);
                todo.Step();
            }
        }


    }

}
