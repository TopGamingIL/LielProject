using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LielProject.Activities
{
    [Activity(Label = "LoggedInActivity")]
    public class LoggedInActivity : Activity
    {
        Button btnLeaderboard, btnPlay;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoggedIdLayout);
            InitViews();
        }

        private void InitViews()
        {
            btnLeaderboard = FindViewById<Button>(Resource.Id.btnLeaderboard);
            btnPlay = FindViewById<Button>(Resource.Id.btnPlay);

            btnLeaderboard.Click += BtnLeaderboard_Click;
            btnPlay.Click += BtnPlay_Click;
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(BetActivity));
            intent.PutExtra("uid", Intent.GetStringExtra("uid"));
            StartActivity(intent);
        }

        private void BtnLeaderboard_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LeaderboardActivity));
            StartActivity(intent);
        }
    }
}