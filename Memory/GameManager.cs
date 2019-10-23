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
        bool _gameStart;
        int _seconds;
        int _minutes;
        int _cardCount;
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
            Seconds = 0;
            CardCount = Deck.Count() /2;
        }

        /// <summary>
        /// Called at Game Start. Shuffles deck and sets lables to appropriate values
        /// </summary>
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
            Labels[1].Text = CardCount.ToString();
        }
        public void Start()
        {
        

        }
        public bool DoCardsMatch()
        {
            return FirstCard.ImageLocation == SecondCard.ImageLocation;
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
        public void RemoveCardsAndPictureBoxes(Card card)
        {
            card.Pb.Enabled = false;
            card.Pb.Visible = false;
        }
        public void LowerCardCount()
        {
            CardCount--;
            Labels[1].Text = CardCount.ToString();
        }
        public bool IsGameFinished()
        {
            if (CardCount == 0)
            {
                return true;
            }
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
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                pictureBox.Enabled = true;
                pictureBox.Visible = true;
                pictureBox.Image = Properties.Resources.deck;
            }
        }

        ////////////////////////////////////////////////////////////////
        //Added after planning
        ////////////////////////////////////////////////////////////////
        #region getters and setters
        public Card FirstCard { get => _firstCard; set => _firstCard = value; }
        public Card SecondCard { get => _secondCard; set => _secondCard = value; }
        public List<PictureBox> PictureBoxes { get => pictureBoxes; set => pictureBoxes = value; }
        public Deck Deck { get => _deck; set => _deck = value; }
        /// <summary>
        /// [0] is time
        /// [1] is cardcount
        /// </summary>
        public List<Label> Labels { get => labels; set => labels = value; }
        public int Seconds { get => _seconds; set => _seconds = value; }
        public int Minutes { get => _minutes; set => _minutes = value; }
        public int CardCount { get => _cardCount; set => _cardCount = value; }
        public bool GameStart { get => _gameStart; set => _gameStart = value; }
        #endregion

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
