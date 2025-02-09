using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Google.Firestore.V1;
using LielProject.Helpers;
using LielProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LielProject.Activities
{
    [Activity(Label = "PlayerLeaderboardActivity")]
    public class LeaderboardActivity : Activity, IOnCompleteListener, IEventListener
    {
        ListView listLB;
        List<Player> lstPlayers;
        PlayerAdapter pa;
        FirebaseData fbd;
        string uid, result;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.LeaderboardLayout);
            InitObject();
            InitViews();
            GetList();
        }

        private async void GetList()
        {
            Toast.MakeText(this, "Getting List...", ToastLength.Short).Show();
            await fbd.GetCollection(General.FS_COLLECTION).AddOnCompleteListener(this);
        }

        private void InitViews()
        {
            listLB = FindViewById<ListView>(Resource.Id.listLB);
            listLB.ItemClick += ListLB_ItemClick;
        }

        private void ListLB_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitObject()
        {
            fbd = new FirebaseData();
            fbd.AddCollectionSnapShotListener(this, General.FS_COLLECTION);
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                lstPlayers = GetDocuments((QuerySnapshot)task.Result);
                if (lstPlayers.Count != 0)
                {
                    Toast.MakeText(this, "OK", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Empty", ToastLength.Short).Show();
                }
            }
        }

        private List<Player> GetDocuments(QuerySnapshot result)
        {
            lstPlayers = new List<Player>();
            foreach (DocumentSnapshot item in result.Documents) {
                Player player = new Player() {
                    Id = item.Id,
                    FullName = item.Get(General.KEY_FULLNAME).ToString(),
                    Username = item.Get(General.KEY_USERNAME).ToString(),
                    Email = item.Get(General.KEY_EMAIL).ToString()
                };
                lstPlayers.Add(player);
            }
            pa = new PlayerAdapter(this, lstPlayers);
            listLB.Adapter = pa;

            return lstPlayers;
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {

        }
    }
}