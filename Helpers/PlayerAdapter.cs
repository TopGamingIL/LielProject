using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LielProject.Activities;
using LielProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LielProject.Helpers
{
    internal class PlayerAdapter : BaseAdapter<Player>
    {

        Context context;
        private List<Player> lstPlayers;

        public PlayerAdapter(Context context)
        {
            this.context = context;
        }

        public PlayerAdapter(Context context, List<Player> lstPlayers)
        {
            this.context = context;
            this.lstPlayers = lstPlayers;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((LeaderboardActivity)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.LeaderboardLayout, parent, false);
            TextView lbRowName = view.FindViewById<TextView>(Resource.Id.lbRowName);
            TextView lbRowUsername = view.FindViewById<TextView>(Resource.Id.lbRowUsername);
            TextView lbRowEmail = view.FindViewById<TextView>(Resource.Id.lbRowEmail);
            Player user = lstPlayers[position];

            if (user != null)
            {
                lbRowName.Text = lbRowName.Text + user.FullName;
                lbRowUsername.Text = lbRowUsername.Text + user.Username;
                lbRowEmail.Text = lbRowEmail.Text + user.Email;
            }

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return lstPlayers.Count;
            }
        }

        public override Player this[int position]
        {
            get
            {
                return lstPlayers[position];
            }
        }

        internal class PlayerAdapterViewHolder : Java.Lang.Object
        {
            //Your adapter views to re-use
            //public TextView Title { get; set; }
        }
    }
}