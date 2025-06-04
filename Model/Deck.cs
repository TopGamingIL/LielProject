using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Blackjack
{
    internal class Deck
    {
        // Properties

        // Cards: List of cards in deck
        public List<Card> Cards { get; set; }

        public Deck() {
            // Initialize deck with 52 cards
            Cards = new List<Card>();
            for (int i = 0; i < 52; i++)
            {
                Cards.Add(new Card(i));
            }
        }

        public Deck(int amount) {
            // Initialize deck with 52 * amount cards
            Cards = new List<Card>();
            for (int i = 0; i < 52 * amount; i++)
            {
                Cards.Add(new Card(i % 52));
            }
        }

        public void DeckOf(int id)
        {
            Cards = new List<Card>();
            for (int i = 0; i < 52; i++)
            {
                Cards.Add(new Card(id));
            }
        }

        public void Shuffle()
        {
            // Shuffle deck
            Random random = new Random();
            for (int i = 0; i < Cards.Count; i++)
            {
                int j = random.Next(i, Cards.Count);
                Card temp = Cards[i];
                Cards[i] = Cards[j];
                Cards[j] = temp;
            }
        }
    }
}
