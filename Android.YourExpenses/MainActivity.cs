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
        TextView totalIncomeText;
        TextView FromDateIncome;
        TextView ToDateIncome;

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
            FromDateIncome = FindViewById<TextView>(Resource.Id.textView3);
            ToDateIncome = FindViewById<TextView>(Resource.Id.textView5);
            var btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);

            Spinner spnrCategorIncAll = FindViewById<Spinner>(Resource.Id.spnCategIncAll);

            var CategoriesAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.IncomeCategoriesAll, Android.Resource.Layout.SimpleSpinnerItem);
            spnrCategorIncAll.Adapter = CategoriesAdapter;

            FromDateIncome.Text = db.GetMinDate<Income>("Incomes").ToShortDateString();
            ToDateIncome.Text = db.GetMaxDate<Income>("Incomes").ToShortDateString();

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

            btnUpdate.Click += delegate
            {
                if (spnrCategorIncAll.SelectedItem.ToString().Equals("All"))
                    totalIncomeText.Text = db.GetTotalAmountFromTo<Income>("Incomes", DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
                else
                    totalIncomeText.Text = db.GetTotalAmountFromTo<Income>("Incomes", spnrCategorIncAll.SelectedItem.ToString(), DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();
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

            var a = db.GetTotalAmount<Income>();
            var b = db.GetTotalAmount<Expense>();

            totalIncomeText.Text = db.GetTotalAmountFromTo<Income>("Incomes", DateTime.Parse(FromDateIncome.Text), DateTime.Parse(ToDateIncome.Text)).ToString();

        }
    }
}

