using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public class Deck
    {
        string _deckBackImageLocation;
        List<Card> _usableDeck;

        public Deck()
        {
            DeckBackImageLocation = "deck";
            UsableDeck = new List<Card>();
            BuildDeck();
            UsableDeck = Shuffle(UsableDeck);
        }

        public string DeckBackImageLocation { get => _deckBackImageLocation; set => _deckBackImageLocation = value; }
        public List<Card> UsableDeck { get => _usableDeck; set => _usableDeck = value; }
      
        /// <summary>
        /// adds all cards twice to the deck
        /// </summary>
        public void BuildDeck()
        {
            for (int i = 0; i < 18; i++)
            {
                Card card = new Card(string.Format("card_{0}", i),string.Format("card_{0}",i),i);
                UsableDeck.Add(card);
            }
            for (int i = 0; i < 18; i++)
            {
                Card card = new Card(string.Format("card_{0}", i), string.Format("card_{0}", i), i+18);
                UsableDeck.Add(card);
            }
        }
        /// <summary>
        /// usabledeck.count
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return UsableDeck.Count;
        }
        /// <summary>
        /// index of card in deck
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Card IndexOf(int position)
        {
            return UsableDeck[position];
        }
        /// <summary>
        /// add card to deck
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            UsableDeck.Add(card);
        }
        /// <summary>
        /// Removes card from deck
        /// </summary>
        /// <param name="position"></param>
        public void RemoveCard(int position)
        {
            UsableDeck.RemoveAt(position);
        }
        /// <summary>
        /// Shuffle deck passed in and return it.
        /// </summary>
        /// <param name="deck"></param>
        /// <returns></returns>
        public List<Card> Shuffle(List<Card> deck)
        {
            List<Card> shuffledDeck = new List<Card>();
            Random rand = new Random();

            int index = 0;
            while (deck.Count > 0)
            {
                index = rand.Next(0, deck.Count);

                shuffledDeck.Add(IndexOf(index));
                RemoveCard(index);
            }
            foreach (Card card in shuffledDeck)
            {
                Console.WriteLine(card.Name);
            }
            return shuffledDeck;
        }
    }
}
