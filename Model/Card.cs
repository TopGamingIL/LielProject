using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    internal class Card
    {
        // Properties

        // Id: Card's unique identifier
        public int Id { get; set; }

        // Rank: Card's rank (A, 2-9, T, J, Q, K)
        public char Rank { get; set; }

        // Suit: Card's suit (S, D, C, H)
        public char Suit { get; set; }

        // Value: Card's value in blackjack
        public int Value { get; set; }
        // Hidden: Indicates if the card is hidden (for dealer's second card)
        public bool Hidden { get; set; }
        // Name: Card's name for png (e.g., "AS", "TD")
        public string Name { get; set; }

        private string Ranks = "A23456789TJQK";
        private string Suits = "SDCH";

        // Constructor
        public Card(int id)
        {
            // Initialize card with id, rank, suit, and value
            Id = id;
            Rank = Ranks[id % 13];
            Suit = Suits[id / 13];
            if (Rank == 'A')
            {
                Value = 11;
            }
            else if (Rank == 'T' || Rank == 'J' || Rank == 'Q' || Rank == 'K')
            {
                Value = 10;
            }
            else
            {
                Value = (id % 13) + 1;
            }
            Hidden = false;
            Name = "_" + Rank.ToString().ToLower() + Suit.ToString().ToLower();
        }

    }
}
