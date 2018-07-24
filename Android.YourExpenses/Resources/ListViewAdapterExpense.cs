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
using Android.YourExpenses.Resources.Model;

namespace Android.YourExpenses.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtCategorie { get; set; }
        public TextView txtName { get; set; }
        public TextView txtDate { get; set; }
        public TextView txtAmount { get; set; }
    }

    public class ListViewAdapterExpense : BaseAdapter
    {
        private Activity activity;
        private List<Expense> lstExpense;

        public ListViewAdapterExpense(Activity activity, List<Expense> lstExpense)
        {
            this.activity = activity;
            this.lstExpense = lstExpense;
        }

        public override int Count => lstExpense.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstExpense[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view_dataTemplate, parent, false);

            var txtCategorie = view.FindViewById<TextView>(Resource.Id.txtViewCategorie);
            var txtName = view.FindViewById<TextView>(Resource.Id.txtViewName);
            var txtDate = view.FindViewById<TextView>(Resource.Id.txtViewDate);
            var txtAmount = view.FindViewById<TextView>(Resource.Id.txtViewAmount);

            txtCategorie.Text = lstExpense[position].Categorie;
            txtName.Text =  lstExpense[position].Name;
            txtDate.Text = lstExpense[position].Date.ToString();
            txtAmount.Text = lstExpense[position].Amount.ToString();

            return view;
        }
    }
}