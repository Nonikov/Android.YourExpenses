﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace Android.YourExpenses.Resources.Models.ExpensesModels
{
   public class IncomeArchive
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Categorie { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double TotalAmount { get; set; }
    }
}