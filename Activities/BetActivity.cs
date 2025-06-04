using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LielProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LielProject.Activities
{
    [Activity(Label = "BetActivity")]
    public class BetActivity : Activity
    {
        EditText etBet;
        Button btnPlaceBet;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.BetLayout);

            InitViews();
        }

        private void InitViews()
        {
            btnPlaceBet = FindViewById<Button>(Resource.Id.btnPlaceBet);
            etBet = FindViewById<EditText>(Resource.Id.etBet);
            btnPlaceBet.Click += BtnPlaceBet_Click;
        }

        private void BtnPlaceBet_Click(object sender, EventArgs e)
        {
            string uid = Intent.GetStringExtra("uid");
            Intent intent = new Intent(this, typeof(GameActivity));
            intent.PutExtra("uid", uid);
            intent.PutExtra("bet", etBet.Text);
            StartActivity(intent);
        }
    }
}