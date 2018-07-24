using Android.App;
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
        TextView txtFrom;
        TextView txtTo;
        TextView txtTotal;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            db = new Repository();
            db.CreateDataBase();

            var btnExpense = FindViewById<Button>(Resource.Id.btnToExpenses);
            var btnIncome = FindViewById<Button>(Resource.Id.btnToIncome);

            txtFrom = FindViewById<TextView>(Resource.Id.txtFrom);
            txtTo = FindViewById<TextView>(Resource.Id.txtTo);
            txtTotal = FindViewById<TextView>(Resource.Id.txtTotal);

            txtFrom.Click += delegate
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    txtFrom.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            txtTo.Click += delegate
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    txtTo.Text = time.ToShortDateString();
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            txtTotal.Click += delegate
            {
                txtTotal.Text = db.GetMaxDate<Income>("Incomes").ToShortDateString();
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

            var txtV_Income = FindViewById<TextView>(Resource.Id.txtTotalIncome);
            var txtV_Expense = FindViewById<TextView>(Resource.Id.txtTotalExpense);
            var txtV_Total = FindViewById<TextView>(Resource.Id.txtTotal);

            var a = db.GetTotalAmount<Income>();
            var b = db.GetTotalAmount<Expense>();

            txtV_Income.Text = a.ToString();
            txtV_Expense.Text = b.ToString();
            txtV_Total.Text = (a - b).ToString();

            txtV_Income.Click += delegate
            {
                txtV_Income.Text = db.GetTotalAmountFromTo<Income>("Incomes", "Salary", Convert.ToDateTime(txtFrom.Text), Convert.ToDateTime(txtTo.Text)).ToString();
            };
        }
    }
}

