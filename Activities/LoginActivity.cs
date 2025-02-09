using Android;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LielProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LielProject.Activities
{

    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        EditText etEmail, etPassword;
        TextView tvDisplay;
        Button btnLogin;
        FirebaseData fbd;

        string uid;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);
            InitViews();
            InitObject();
        }

        // initialize FireBaseData object
        private void InitObject()
        {
            fbd = new FirebaseData();
        }

        // initialize components from XML file
        private void InitViews()
        {
            etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            // button click event
            btnLogin.Click += BtnLogin_ClickAsync;
        }

        // login when clicking on "Login" button
        private async void BtnLogin_ClickAsync(object? sender, EventArgs e)
        {
            // if login is successfull
            if (await Login(etEmail.Text, etPassword.Text))
            {
                // print
                Toast.MakeText(this, "Logged in Successfully", ToastLength.Short).Show();

                // initialize password input box
                etPassword.Text = "";

                // go to ProfileActivity after login
                Intent intent = new Intent(this, typeof(ProfileActivity));
                intent.PutExtra("uid", uid);
                StartActivity(intent);
            }
            else
            {
                // print
                Toast.MakeText(this, "Login Failed", ToastLength.Short).Show();
            }
        }

        // tries to log in and catch any errors
        private async Task<bool> Login(string email, string password)
        {
            try
            {
                // log in with email and password
                await fbd.auth.SignInWithEmailAndPassword(email, password);

                // get uid
                uid = fbd.auth.CurrentUser.Uid;
            }
            catch (System.Exception e)
            {
                string s = e.Message;
                return false;
            }
            return true;
        }
    }
}