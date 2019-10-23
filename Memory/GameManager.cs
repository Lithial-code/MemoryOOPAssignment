using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
     class GameManager
    {
        static Deck _deck;
        DateTime timer;
        string _playerName;
        static HighScore[] _highScores;

        List<Label> labels;
        Card _firstCard;
        Card _secondCard;
        List<PictureBox> pictureBoxes;
        
        public GameManager()
        {
            Deck = new Deck();
            _highScores = new HighScore[10];
            PictureBoxes = new List<PictureBox>() { };
            Labels = new List<Label>();
        }

        public void SetupBoard()
        {
            Deck.UsableDeck = Deck.Shuffle();
            //deal cards
            for (int i = 0; i < Deck.UsableDeck.Count; i++)
            {
                Deck.UsableDeck[i].Pb = PictureBoxes[i];
            }
            //reset timer to 0
            Labels[0].Text = "0";
            //reset card count
            Labels[1].Text = Deck.Count().ToString();
        }
        public void Start()
        {
        

        }
        public bool docardsmatch()
        {
            return FirstCard == SecondCard;
        }

        public void FlipCards(Card card)
        {
            Console.WriteLine("Flip card: " + card.Name);

            card.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(card.ImageLocation);
            //SecondCard.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(SecondCard.ImageLocation);
        }
        public void FlipCardsBack()
        {
            Console.WriteLine("Flip cards back");
            FirstCard.Pb.Image = Properties.Resources.deck;
            SecondCard.Pb.Image = Properties.Resources.deck;
        }
        public void NullCards()
        {
            FirstCard = null;
            SecondCard = null;
        }
        public void LowerCardCount()
        {

        }
        public bool IsGameFinished()
        {
            return false;
        }
        public void CheckBestScore()
        {

        }
        public void RecordTopScore()
        {

        }
        public void SaveAllScores()
        {

        }
        public void EndGame()
        {

        }
        public void ResetBoard()
        {

        }

        ////////////////////////////////////////////////////////////////
        //Added after planning
        ////////////////////////////////////////////////////////////////
        public Card FirstCard { get => _firstCard; set => _firstCard = value; }
        public Card SecondCard { get => _secondCard; set => _secondCard = value; }
        public List<PictureBox> PictureBoxes { get => pictureBoxes; set => pictureBoxes = value; }
        public Deck Deck { get => _deck; set => _deck = value; }
        public List<Label> Labels { get => labels; set => labels = value; }

        public Card CardByPBID(PictureBox pb)
        {
            for (int i = 0; i < pictureBoxes.Count; i++)
            {
                if (pb == pictureBoxes[i])
                {
                    Console.WriteLine("returning card: " + i);
                    Deck.IndexOf(i).Pb = PictureBoxes[i];
                    return Deck.IndexOf(i);
                }
            }
            return null;
        }
    }
}
