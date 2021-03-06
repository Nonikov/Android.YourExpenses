﻿using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using Android.YourExpenses.Resources.DateHelper;
using Android.YourExpenses.Resources.Model;
using Android.Support.V7.App;
using Android.Content;

namespace Android.YourExpenses
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Repository db;
        TextView totalIncomeText;
        TextView totalExpenseText;
        TextView FromDateIncome;
        TextView ToDateIncome;
        TextView FromDateExpense;
        TextView ToDateExpense;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            db = new Repository();
            db.CreateDataBase();

            var btnExpense = FindViewById<Button>(Resource.Id.btnToExpenses);
            var btnIncome = FindViewById<Button>(Resource.Id.btnToIncome);

            totalIncomeText = FindViewById<TextView>(Resource.Id.txtTotal);
            totalExpenseText = FindViewById<TextView>(Resource.Id.txtTotalE);
            FromDateIncome = FindViewById<TextView>(Resource.Id.textView3);
            ToDateIncome = FindViewById<TextView>(Resource.Id.textView5);
            FromDateExpense = FindViewById<TextView>(Resource.Id.textView3E);
            ToDateExpense = FindViewById<TextView>(Resource.Id.textView5E);
            var btnUpdateIncome = FindViewById<Button>(Resource.Id.btnUpdate);
            var btnUpdateExpense = FindViewById<Button>(Resource.Id.btnUpdateE);

            Spinner spnrCategorIncAll = FindViewById<Spinner>(Resource.Id.spnCategIncAll);
            Spinner spnrCategorExpAll = FindViewById<Spinner>(Resource.Id.spnCategExpAll);

            var CategoriesAdapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.IncomeCategoriesAll, Android.Resource.Layout.SimpleSpinnerItem);
            spnrCategorIncAll.Adapter = CategoriesAdapter1;

            var CategoriesAdapter2 = ArrayAdapter.CreateFromResource(this, Resource.Array.ExpenseCategoriesAll, Android.Resource.Layout.SimpleSpinnerItem);
            spnrCategorExpAll.Adapter = CategoriesAdapter2;

            FromDateIncome.Text = db.GetMinDate<Income>("Incomes").ToShortDateString();
            ToDateIncome.Text = db.GetMaxDate<Income>("Incomes").ToShortDateString();
            FromDateExpense.Text = db.GetMinDate<Expense>("Expenses").ToShortDateString();
            ToDateExpense.Text = db.GetMaxDate<Expense>("Expenses").ToShortDateString();

            FromDateIncome.Click += delegate
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    FromDateIncome.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            ToDateIncome.Click += delegate
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    ToDateIncome.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            FromDateExpense.Click += delegate {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    FromDateExpense.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            ToDateExpense.Click += delegate
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    ToDateExpense.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            btnUpdateIncome.Click += delegate
            {
                if (spnrCategorIncAll.SelectedItem.ToString().Equals("All"))
                    totalIncomeText.Text = db.GetTotalAmountFromTo<Income>("Incomes", DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
                else
                    totalIncomeText.Text = db.GetTotalAmountFromTo<Income>("Incomes", spnrCategorIncAll.SelectedItem.ToString(), DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
            };

            btnUpdateExpense.Click += delegate {
                if (spnrCategorExpAll.SelectedItem.ToString().Equals("All"))
                    totalExpenseText.Text = db.GetTotalAmountFromTo<Expense>("Expenses", DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
                else
                    totalExpenseText.Text = db.GetTotalAmountFromTo<Expense>("Expenses", spnrCategorExpAll.SelectedItem.ToString(), DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
            };

            btnExpense.Click += (s, e) =>
            {
                Intent expenseActivity = new Intent(this, typeof(ExpenseActivity));

                StartActivity(expenseActivity);
            };

            btnIncome.Click += (s, e) =>
            {
                Intent incomeActivity = new Intent(this, typeof(IncomeActivity));
                StartActivity(incomeActivity);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            var txtBalance = FindViewById<TextView>(Resource.Id.textView7);

            var a = db.GetTotalAmount<Income>();
            var b = db.GetTotalAmount<Expense>();

            txtBalance.Text = (a - b).ToString();

            totalIncomeText.Text = db.GetTotalAmountFromTo<Income>("Incomes", DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
            totalExpenseText.Text = db.GetTotalAmountFromTo<Expense>("Expenses", DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
        }
    }
}

