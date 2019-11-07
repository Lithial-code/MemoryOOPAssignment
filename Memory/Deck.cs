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
            //TODO add a better deck backing
            DeckBackImageLocation = "deck";
            UsableDeck = new List<Card>();
            BuildDeck();
            UsableDeck = Shuffle(UsableDeck);
        }

        public string DeckBackImageLocation { get => _deckBackImageLocation; set => _deckBackImageLocation = value; }
        public List<Card> UsableDeck { get => _usableDeck; set => _usableDeck = value; }

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
        public int Count()
        {
            return UsableDeck.Count;
        }

        public Card IndexOf(int position)
        {
            return UsableDeck[position];
        }
        
        public void AddCard(Card card)
        {
            UsableDeck.Add(card);
        }
        public void RemoveCard(int position)
        {
            UsableDeck.RemoveAt(position);
        }

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
