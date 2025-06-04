using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Interop;
using LielProject.Helpers;
using LielProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LielProject.Activities
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity, IOnSuccessListener, Firebase.Firestore.IEventListener
    {
        EditText profileUsernameEt, profileNameEt, profileCoinsEt;
        FirebaseData fbd;
        Player player;
        string uid;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ProfileLayout);
            uid = Intent.GetStringExtra("uid");
            InitObjects();
            InitViews();
            GetProfileAsync();
        }

        private async void GetProfileAsync()
        {
            await fbd.GetCollection(General.FS_COLLECTION, uid).AddOnSuccessListener(this);
        }

        private void InitViews()
        {
            profileUsernameEt = FindViewById<EditText>(Resource.Id.profileUsernameEt);
            profileNameEt = FindViewById<EditText>(Resource.Id.profileNameEt);
            profileCoinsEt = FindViewById<EditText>(Resource.Id.profileCoinsEt);
        }

        private void InitObjects()
        {
            fbd = new FirebaseData();
            player = new Player();
            fbd.AddCollectionSnapShotListener(this, General.FS_COLLECTION);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            player = new Player(snapshot.Id, snapshot.Get(General.KEY_FULLNAME).ToString(), snapshot.Get(General.KEY_USERNAME).ToString(), snapshot.Get(General.KEY_EMAIL).ToString(), (double)snapshot.Get(General.KEY_CHIPS), snapshot.Get(General.KEY_PASSWORD).ToString());
            PrintPlayer(player);
        }

        private void PrintPlayer(Player player)
        {
            profileUsernameEt.Text = player.Username;
            profileNameEt.Text = player.FullName;
            profileCoinsEt.Text = player.Chips.ToString();
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {
            
        }
    }
}