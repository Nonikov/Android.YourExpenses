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
using Android.YourExpenses.Resources;
using Android.YourExpenses.Resources.DateHelper;
using Android.YourExpenses.Resources.Model;
using Java.Util;

namespace Android.YourExpenses
{
    [Activity(Label = "ExpenseActivity")]
    public class ExpenseActivity : Activity, Com.Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog.IOnDateSetListener, Com.Wdullaer.MaterialDateTimePicker.Time.TimePickerDialog.IOnTimeSetListener
    {
        ListView lstData;
        List<Expense> lstSource = new List<Expense>();
        Repository db;
        Button btnDate, btnTime;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_expense);

            //Create DataBase
            db = new Repository();

            //DateView
            btnDate = FindViewById<Button>(Resource.Id.btnDate);
            btnTime = FindViewById<Button>(Resource.Id.btnTime);
            btnDate.Text = String.Format("{0:d}", DateTime.Now);
            btnTime.Text = String.Format("{0:t}", DateTime.Now);

            //Categories spinner
            Spinner spinner1 = FindViewById<Spinner>(Resource.Id.spinner1);
            Spinner spinnerMoney = FindViewById<Spinner>(Resource.Id.spinnerMoney);

            var CategoriesAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.ExpenseCategories, Android.Resource.Layout.SimpleSpinnerItem);
            spinner1.Adapter = CategoriesAdapter;

            var categorMoneyAdapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Carency, Android.Resource.Layout.SimpleSpinnerItem);
            spinnerMoney.Adapter = categorMoneyAdapter;

            lstData = FindViewById<ListView>(Resource.Id.listView);

            var edtName = FindViewById<EditText>(Resource.Id.ExpenseEditText);
            var edtAmount = FindViewById<EditText>(Resource.Id.AmountEditText);


            var btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            var btnRemove = FindViewById<Button>(Resource.Id.btnDelete);
            var btnBack = FindViewById<Button>(Resource.Id.btnBack);
            LoadData();

            btnDate.Click += delegate
            {
                Calendar now = Calendar.Instance;
                Com.Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog datePicker = Com.Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog.NewInstance(
                    this,
                    now.Get(CalendarField.Year),
                    now.Get(CalendarField.Month),
                    now.Get(CalendarField.DayOfMonth));
                datePicker.SetTitle("DatePicker Dialog");
                datePicker.Show(FragmentManager, "DatePicker");
            };

            btnTime.Click += delegate
            {
                Calendar now = Calendar.Instance;
                Com.Wdullaer.MaterialDateTimePicker.Time.TimePickerDialog timePicker = Com.Wdullaer.MaterialDateTimePicker.Time.TimePickerDialog.NewInstance(
                   this,
                    now.Get(CalendarField.HourOfDay),
                    now.Get(CalendarField.Minute),
                    true); //true 24, false 12
                timePicker.Title = "TimePicker Dialog";
                timePicker.Show(FragmentManager, "TimePicker");
            };

            btnAdd.Click += delegate
            {
                Expense expense = new Expense()
                {
                    Name = edtName.Text,
                    Categorie = spinner1.SelectedItem.ToString(),
                    Date = Convert.ToDateTime(btnDate.Text + " " + btnTime.Text),
                    Amount = Convert.ToDouble(edtAmount.Text)
                };
                db.InsertIntoTable(expense);
                LoadData();
            };

            btnEdit.Click += delegate
            {
                Expense expense = new Expense()
                {
                    Id = int.Parse(edtName.Tag.ToString()),
                    Name = edtName.Text,
                    Categorie = spinner1.SelectedItem.ToString(),
                    Date = Convert.ToDateTime(btnDate.Text + " " + btnTime.Text),
                    Amount = Convert.ToDouble(edtAmount.Text)
                };
                db.UpdateTable<Expense>(expense);
                LoadData();
            };

            btnRemove.Click += delegate
            {
                Expense expense = new Expense()
                {
                    Id = int.Parse(edtName.Tag.ToString()),
                    Name = edtName.Text,
                    Categorie = spinner1.SelectedItem.ToString(),
                    Date = Convert.ToDateTime(btnDate.Text + " " + btnTime.Text),
                    Amount = Convert.ToDouble(edtAmount.Text)
                };
                db.DeleteTable(expense);
                LoadData();
            };

            btnBack.Click += delegate
            {
                this.Finish();
            };

            lstData.ItemClick += (s, e) =>
            {
                //Set Backround for selected item
                for (int i = 0; i < lstData.Count; i++)
                {
                    if (e.Position.Equals(i))
                        lstData.GetChildAt(i)?.SetBackgroundColor(Android.Graphics.Color.LightBlue);
                    else
                        lstData.GetChildAt(i)?.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }

                //Binding Data
                var txtDate = e.View.FindViewById<TextView>(Resource.Id.txtViewDate);
                var txtName = e.View.FindViewById<TextView>(Resource.Id.txtViewName);
                var txtAmount = e.View.FindViewById<TextView>(Resource.Id.txtViewAmount);

                btnDate.Text = String.Format("{0:d}", Convert.ToDateTime(txtDate.Text));
                btnTime.Text = String.Format("{0:t}", Convert.ToDateTime(txtDate.Text));
                spinner1.SetSelection(0);
                edtName.Text = txtName.Text;
                edtName.Tag = e.Id;

                edtAmount.Text = txtAmount.Text;
            };
        }

        public void OnDateSet(Com.Wdullaer.MaterialDateTimePicker.Date.DatePickerDialog p0, int year, int monthOfYear, int dayOfMonth)
        {
            btnDate.Text = $"{monthOfYear}/{dayOfMonth}/{year}";

        }
        public void OnTimeSet(Com.Wdullaer.MaterialDateTimePicker.Time.TimePickerDialog view, int hourOfDay, int minute, int second)
        {
            btnTime.Text = $"{hourOfDay}:{minute}";
        }

        private void LoadData()
        {
            lstSource = db.SelectTable<Expense>();
            var adapter = new ListViewAdapterExpense(this, lstSource);
            lstData.Adapter = adapter;
        }
    }
}