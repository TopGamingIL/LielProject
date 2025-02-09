using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LielProject.Model
{
    internal class Player
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Coins { get; set; }
        public string Password { get; set; }

        public Player(string id, string fullName, string username, string email, int coins, string password) {
            this.Id = id;
            this.FullName = fullName;
            this.Username = username;
            this.Email = email;
            this.Coins = coins;
            this.Password = password;
            
        }

        public Player() {
            this.Id = "";
            this.FullName = "";
            this.Username = "";
            this.Email = "";
            this.Coins = 0;
            this.Password = "";
        }

    }
}