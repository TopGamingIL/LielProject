using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Blackjack;
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
        public double Chips { get; set; }
        public string Password { get; set; }
        public double Bet { get; set; }
        public List<Card> Hand { get; set; }

        public Player(string id, string fullName, string username, string email, double chips, string password) {
            Id = id;
            FullName = fullName;
            Username = username;
            Email = email;
            Chips = chips;
            Password = password;
            Hand = new List<Card>();
        }

        public Player() {
            Id = "";
            FullName = "";
            Username = "";
            Email = "";
            Chips = 0;
            Password = "";
            Hand = new List<Card>();
        }

        public void Win()
        {
            // Add bet to balance and reset bet
            Chips += Bet * 2;
            Bet = 0;
        }

        public void Tie()
        {
            // Add bet to balance and reset bet
            Chips += Bet;
            Bet = 0;
        }

        public void DoubleDown(Deck deck)
        {
            Chips -= Bet;
            Deal(deck);
        }

        public void Deal(Deck deck, bool hidden = false)
        {
            Card card = deck.Cards.First();
            card.Hidden = hidden;
            Hand.Add(card);
            deck.Cards.RemoveAt(0);
            AceCheck();
        }

        public int HandValue()
        {
            // Calculate value of hand
            int value = 0;
            foreach (Card card in Hand)
            {
                if (!card.Hidden)
                {
                    value += card.Value;
                }
            }
            return value;
        }

        public void AceCheck()
        {
            // Check for aces in hand and adjust value accordingly
            // If hand value is more than 21, change value of ace to 11
            foreach (Card card in Hand)
            {
                if (card.Rank == 'A' && HandValue() > 21)
                {
                    card.Value = 1;
                }
            }

        }

    }
}