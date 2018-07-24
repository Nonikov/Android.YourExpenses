using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.YourExpenses.Resources.Models;
using SQLite;

namespace Android.YourExpenses.Resources.Model
{
    [Table("Expenses")]
   public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Categorie { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}