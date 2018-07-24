﻿using System;
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

        public bool InsertIntoTable<T>(T expense)
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.Insert(expense);
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

        public bool UpdateTable<T>(dynamic expense) where T : new()
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.Query<T>("UPDATE Incomes set Name=?, Categorie=?, Date=?, Amount =? Where Id=?", expense.Name, expense.Categorie, expense.Date, expense.Amount, expense.Id);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool DeleteTable<T>(T expense)
        {
            try
            {
                using (var connection = new SQLiteConnection(path))
                {
                    connection.Delete(expense);
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
                    var items = connection.Query<T>($"SELECT * FROM {tableName} WHERE Date >= ? AND Date <= ?", fromD, toD);
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
    }
}