using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.YourExpenses.Resources.Model;
using Android.YourExpenses.Resources.Models;
using SQLite;

namespace Android.YourExpenses.Resources.DateHelper
{
    public class Repository
    {
        static string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        readonly string path = Path.Combine(folder, "YourExpenses.db");

        public bool CreateDataBase()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.CreateTable<Expense>();
                    connection.CreateTable<Income>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool InsertIntoTable<T>(T table)
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.Insert(table);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public List<T> SelectTable<T>() where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    return connection.Table<T>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool UpdateTable<T>(string tableName, dynamic table) where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.Query<T>($"UPDATE {tableName} set Name=?, Categorie=?, Date=?, Amount =?, Currency=? Where Id=?", table.Name, table.Categorie, table.Date, table.Amount, table.Currency, table.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteTable<T>(T table)
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.Delete(table);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public double GetTotalAmount<T>() where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    var items = connection.Table<T>().ToList();
                    double temp = 0;
                    foreach (dynamic item in items)
                    {
                        temp += item.Amount;
                    }
                    return temp;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return -1;
            }
        }

        public double GetTotalAmountFromTo<T>(string tableName, DateTime fromD, DateTime toD ) where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    var items = connection.Query<T>($"SELECT Amount FROM {tableName} WHERE Date >= ? AND Date <= ?", fromD, toD);
                    double temp = 0;
                    foreach (dynamic item in items)
                    {
                        temp += item.Amount;
                    }
                    return temp;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return -1;
            }
        }

        public double GetTotalAmountFromTo<T>(string tableName, string categorie, DateTime fromD, DateTime toD) where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    var items = connection.Query<T>($"SELECT Amount FROM {tableName} WHERE Categorie LIKE ? AND Date >= ? AND Date <= ?", categorie, fromD, toD);
                    double temp = 0;
                    foreach (dynamic item in items)
                    {
                        temp += item.Amount;
                    }
                    return temp;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return -1;
            }
        }

        public DateTime GetMinDate<T>(string tableName) where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    dynamic item = connection.Query<T>($"SELECT Date FROM {tableName} WHERE Date = (SELECT min(Date) FROM {tableName})").FirstOrDefault<T>();
                  
                    return item.Date;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return DateTime.Parse("01/01/0010");
            }
        }

        public DateTime GetMaxDate<T>(string tableName) where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    dynamic item = connection.Query<T>($"SELECT Date FROM {tableName} WHERE Date = (SELECT max(Date) FROM {tableName})").FirstOrDefault<T>();

                    return item.Date;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return DateTime.Parse("01/01/0002");
            }
        }
    }
}