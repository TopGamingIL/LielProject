using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Blackjack;
using Firebase.Firestore;
using Java.Util;
using LielProject.Helpers;
using LielProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LielProject.Activities
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : Activity, IOnSuccessListener, Firebase.Firestore.IEventListener
    {
        // Properties

        // Objects
        Player Player;
        Player Dealer;
        Deck Deck;

        // Game state
        bool IsPlayerTurn;

        // UI Elements
        LinearLayout dealerCardsLayout;
        LinearLayout playerCardsLayout;
        Button btnHit, btnStand, btnDouble, btnPlayAgain;
        TextView playerHandValue, dealerHandValue, tvChips, tvBet;

        // User
        FirebaseData fbd;
        string uid;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.GameLayout);

            InitViews();
            InitObjects();
            //GetProfileAsync();
        }
        private void InitViews()
        {
            dealerCardsLayout = FindViewById<LinearLayout>(Resource.Id.dealerCardsLayout);
            playerCardsLayout = FindViewById<LinearLayout>(Resource.Id.playerCardsLayout);
            btnHit = FindViewById<Button>(Resource.Id.btnHit);
            btnStand = FindViewById<Button>(Resource.Id.btnStand);
            btnDouble = FindViewById<Button>(Resource.Id.btnDouble);
            playerHandValue = FindViewById<TextView>(Resource.Id.playerHandValue);
            dealerHandValue = FindViewById<TextView>(Resource.Id.dealerHandValue);
            tvBet = FindViewById<TextView>(Resource.Id.tvBet);
            tvChips = FindViewById<TextView>(Resource.Id.tvChips);
            btnPlayAgain = FindViewById<Button>(Resource.Id.btnPlayAgain);

            btnHit.Click += BtnHit_Click;
            btnStand.Click += BtnStand_Click;
            btnDouble.Click += BtnDouble_Click;
            btnPlayAgain.Click += BtnPlayAgain_Click;

            uid = Intent.GetStringExtra("uid");
        }

        private void BtnPlayAgain_Click(object sender, EventArgs e)
        {
            InitGame();
        }

        public void InitGame()
        {
            // Initialize game with a shuffled deck, player and dealer
            System.Random random = new System.Random();
            Deck = new Deck(random.Next(1, 9));
            Deck.Shuffle();
            Dealer = new Player();
            IsPlayerTurn = true;
            Player.Bet = Convert.ToDouble(Intent.GetStringExtra("bet"));

            Player.Chips -= Player.Bet;

            // Clear player and dealer hands
            Player.Hand.Clear();
            Dealer.Hand.Clear();

            // Deal cards to player and dealer
            Player.Deal(Deck);
            Dealer.Deal(Deck);
            Player.Deal(Deck);
            Dealer.Deal(Deck, hidden: true);

            RefreshScreen();

            if (Player.HandValue() == 21)
            {
                IsPlayerTurn = false;
                EndTurnAsync();
            }
        }

        private async void InitObjects()
        {
            fbd = new FirebaseData();
            fbd.GetCollection(General.FS_COLLECTION, uid).AddOnSuccessListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            if (snapshot.Exists())
            {
                Player = new Player
                {
                    Username = snapshot.Get(General.KEY_USERNAME)?.ToString(),
                    Chips = Convert.ToDouble(snapshot.Get(General.KEY_CHIPS)?.ToString()),
                    Bet = Convert.ToDouble(Intent.GetStringExtra("bet"))
                };
                
            }
            InitGame();
        }

        public void RefreshScreen()
        {
            // Clear layouts
            dealerCardsLayout.RemoveAllViews();
            playerCardsLayout.RemoveAllViews();

            // Add dealer cards to layout
            foreach (var card in Dealer.Hand)
            {
                AddCardToLayout(card.Name, dealerCardsLayout, card.Hidden);
            }

            // Add player cards to layout
            foreach (var card in Player.Hand)
            {
                AddCardToLayout(card.Name, playerCardsLayout);
            }

            // Update hand values
            dealerHandValue.Text = Dealer.HandValue().ToString();
            playerHandValue.Text = Player.HandValue().ToString();

            // Update chips and bet
            tvChips.Text = Player.Chips.ToString();
            tvBet.Text = Player.Bet.ToString();

            if (Player.Hand.Count() > 2)
            {
                btnDouble.Visibility = ViewStates.Gone;
            }
            else { btnDouble.Visibility = ViewStates.Visible; }

            btnPlayAgain.Visibility = ViewStates.Gone;
        }

        public async void EndTurnAsync()
        {
            // Set dealer's hidden card to visible
            Dealer.Hand[1].Hidden = false;

            RefreshScreen();

            // Dealer's turn
            while (Dealer.HandValue() < 17)
            {
                Dealer.Deal(Deck);
                await System.Threading.Tasks.Task.Delay(2000);
                RefreshScreen();
            }

            // Determine winner

            // Check for blackjack
            if (Player.HandValue() == 21 && Dealer.HandValue() == 21)
            {
                Toast.MakeText(this, "Push", ToastLength.Short).Show();
                Player.Chips += Player.Bet;
            }
            else if (Player.HandValue() == 21 && Player.Hand.Count() == 2)
            {
                Toast.MakeText(this, "Blackjack! You win!", ToastLength.Short).Show();
                Player.Chips += Player.Bet * 2.5;
            }
            else if (Dealer.HandValue() == 21 && Player.Hand.Count() == 2)
            {
                Toast.MakeText(this, "Dealer has blackjack! You lose.", ToastLength.Short).Show();
            }
            else if (Player.HandValue() > 21)
            {
                Toast.MakeText(this, "You busted! Dealer wins.", ToastLength.Short).Show();
            }
            else if (Dealer.HandValue() > 21)
            {
                Toast.MakeText(this, "Dealer busted! You win!", ToastLength.Short).Show();
                Player.Chips += Player.Bet * 2;
            }
            else if (Player.HandValue() > Dealer.HandValue())
            {
                Toast.MakeText(this, "You win!", ToastLength.Short).Show();
                Player.Chips += Player.Bet * 2;
            }
            else if (Player.HandValue() == Dealer.HandValue())
            {
                Toast.MakeText(this, "Push", ToastLength.Short).Show();
                Player.Chips += Player.Bet;
            }
            else
            {
                Toast.MakeText(this, "Dealer wins!", ToastLength.Short).Show();
            }

            // Update player's chips
            UpdatePlayerChips();
            btnPlayAgain.Visibility = ViewStates.Visible;
        }

        public void UpdatePlayerChips()
        {
            // Update player's chips in Firestore
            var playerRef = fbd.firestore.Collection(General.FS_COLLECTION).Document(uid);
            playerRef.Update(General.KEY_CHIPS, Player.Chips);
        }

        private void BtnDouble_Click(object sender, EventArgs e)
        {
            if (!IsPlayerTurn) return;

            // Double the bet
            Player.Deal(Deck);
            Player.Chips -= Player.Bet;
            Player.Bet *= 2;
            RefreshScreen();

            IsPlayerTurn = false;
            EndTurnAsync();
        }

        private void BtnStand_Click(object sender, EventArgs e)
        {
            if (!IsPlayerTurn) return;

            IsPlayerTurn = false;
            EndTurnAsync();
        }

        private void BtnHit_Click(object sender, EventArgs e)
        {
            if (!IsPlayerTurn) return;

            Player.Deal(Deck);
            RefreshScreen();

            if (Player.HandValue() >= 21)
            {
                IsPlayerTurn = false;
                EndTurnAsync();
            }
        }


        void AddCardToLayout(string cardName, LinearLayout layout, bool hide = false)
        {
            ImageView imageView = new ImageView(this);

            // If hidden, use back of card
            string imageResource = hide ? "_back" : cardName; // match filename without extension

            int resId = Resources.GetIdentifier(imageResource, "drawable", PackageName);
            imageView.SetImageResource(resId);

            // Set image size and margin
            var layoutParams = new LinearLayout.LayoutParams(240, 360); // size in pixels
            layoutParams.SetMargins(8, 0, 8, 0);
            imageView.LayoutParameters = layoutParams;

            layout.AddView(imageView);
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {
            throw new NotImplementedException();
        }
    }
}