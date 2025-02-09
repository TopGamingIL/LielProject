using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using LielProject.Helpers;
using LielProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LielProject.Activities
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText etRegUsername, etRegFullName, etRegEmail, etRegPassword;
        Button btnRegister;
        FirebaseData fbd;
        Player player;
        HashMap hm;
        string uid;
        public static string id;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegisterLayout);

            InitObject();
            InitViews();
        }

        private void InitObject()
        {
            fbd = new FirebaseData();
            player = new Player();
        }

        private void InitViews()
        {
            etRegUsername = FindViewById<EditText>(Resource.Id.etRegUsername);
            etRegFullName = FindViewById<EditText>(Resource.Id.etRegFullName);
            etRegEmail = FindViewById<EditText>(Resource.Id.etRegEmail);
            etRegPassword = FindViewById<EditText>(Resource.Id.etRegPassword);
            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);

            btnRegister.Click += BtnRegister_Click;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private async void SaveDocument() {
            if (await Register(etRegFullName.Text, etRegUsername.Text, etRegEmail.Text, etRegPassword.Text)) {
                Toast.MakeText(this, "Registered Successfully", ToastLength.Short).Show();
                etRegUsername.Text = "";
                etRegFullName.Text = "";
                etRegEmail.Text = "";
                etRegPassword.Text = "";
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            } else {
                Toast.MakeText(this, "Register Failed", ToastLength.Short).Show();
            }
        }

        private async Task<bool> Register(string fullName, string username, string email, string password)
        {
            try
            {
                await fbd.auth.CreateUserWithEmailAndPassword(email, password);
                id = fbd.auth.CurrentUser.Uid;
                HashMap userMap = new HashMap();
                userMap.Put(General.KEY_FULLNAME, fullName);
                userMap.Put(General.KEY_EMAIL, email);
                userMap.Put(General.KEY_USERNAME, username);
                userMap.Put(General.KEY_PASSWORD, password);
                userMap.Put(General.KEY_ID, fbd.auth.CurrentUser.Uid);
                DocumentReference userReference = fbd.firestore.Collection(General.FS_COLLECTION).Document(fbd.auth.CurrentUser.Uid);
                await userReference.Set(userMap);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }
    }
}