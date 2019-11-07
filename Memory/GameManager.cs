using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public class GameManager
    {

        bool _gameStart;

        int _seconds;
        int _minutes;
        int _cardCount;

        string _playerName;

        static Deck _deck;

        List<Label> _labels;
        List<PictureBox> _pictureBoxes;
        List<Timer> _timers;
        static List<Label> _nameLabels;
        static List<Label> _scoreLabels;

        static List<HighScore> _highScores;

        Card _firstCard;
        Card _secondCard;
        
        public GameManager()
        {
            Deck = new Deck();
            PictureBoxes = new List<PictureBox>() { };
            Labels = new List<Label>();
            Seconds = 0;
            CardCount = Deck.Count();
            Timers = new List<Timer>();
            NameLabels = new List<Label>();
            ScoreLabels = new List<Label>();
            HighScores = new List<HighScore>();
            //LoadScores();
        }

        /// <summary>
        /// Called at Game Start. Shuffles deck and sets lables to appropriate values
        /// </summary>
        public void SetupBoard()
        {
            //Deck.UsableDeck = Deck.Shuffle();
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
        public void AssembleFakeScores()
        {
            for (int i = 0; i < 10; i++)
            {
                HighScore highScore = new HighScore("Score" + i, i * 10);
                HighScores.Add(highScore);
            }
            SaveScores();
        }
        public bool DoCardsMatch()
        {
            if (!IsGameFinished())
            {
                return FirstCard.ImageLocation == SecondCard.ImageLocation;
            }
            return false;
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
            if (!IsGameFinished())
            {
                FirstCard.Pb.Image = Properties.Resources.deck;
                SecondCard.Pb.Image = Properties.Resources.deck;
            }
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
        public void LowerCardCount(int cardsKilled)
        {
            CardCount -= cardsKilled;
            Labels[1].Text = CardCount.ToString();
        }
        public bool IsGameFinished()
        {
            if (CardCount == 0)
            {
                Console.WriteLine("Is game finished?");
                //EndGame();
                return true;
            }
            return false;
        }
        public void ActivateHint()
        {
            if (GameStart)
            {
                Timers[1].Start();
                foreach (Card card in Deck.UsableDeck)
                {
                    card.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(card.ImageLocation);
                }
            }
         
        }
        public void DeactivateHint()
        {
            foreach (Card card in Deck.UsableDeck)
            {
                card.Pb.Image = Properties.Resources.deck;
            }
            Timers[1].Stop();
        }
        public void CheckBestScore(string name)
        {
            HighScore highScore = new HighScore(name,Seconds);
            HighScores.Add(highScore);

            RecordLabels();
        
            GameBoard.highScoreForm.Show();
            SaveScores();
        }
        public void RecordTopScore()
        {

        }
        public void SaveScores()
        {
            for (int i = HighScores.Count-1; i > 10 ; i--)
            {
                HighScores.RemoveAt(i);
            }
            StreamWriter writer = new StreamWriter("save.json");
            string data = JsonConvert.SerializeObject(HighScores);
            writer.Write(data);
            writer.Close();
        }
        public void LoadScores()
        {
            try
            {
                using (StreamReader reader = new StreamReader("save.json"))
                {
                    string data = reader.ReadToEnd();
                    Console.WriteLine(data);
                    HighScores = Deserialize<HighScore>(data);
                    reader.Close();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
     
            RecordLabels();
         }
        public void RecordLabels()
        {
            int cap = 10;
            HighScores.Sort((p1, p2) =>
            {
                return p1.Time - p2.Time;
            });
            if (HighScores.Count < cap)
            {
                cap = HighScores.Count;
            }
        
            if (HighScores.Count != 0)
            {
                for (int i = 0; i < cap; i++)
                {
                    NameLabels[i].Text = HighScores[i].Name;
                    ScoreLabels[i].Text = HighScores[i].ToReadable();
                }
            }    
        }
        public void EndGame()
        {
            Timers[0].Stop();
            Console.WriteLine("Endgame");
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your Name:  ", "You Win", "name:");
            CheckBestScore(UserAnswer);

        }
        public void ResetBoard()
        {
            foreach (PictureBox pictureBox in _pictureBoxes)
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
        public List<PictureBox> PictureBoxes { get => _pictureBoxes; set => _pictureBoxes = value; }
        public Deck Deck { get => _deck; set => _deck = value; }
        /// <summary>
        /// [0] is time
        /// [1] is cardcount
        /// </summary>
        public List<Label> Labels { get => _labels; set => _labels = value; }
        public int Seconds { get => _seconds; set => _seconds = value; }
        public int Minutes { get => _minutes; set => _minutes = value; }
        public int CardCount { get => _cardCount; set => _cardCount = value; }
        public bool GameStart { get => _gameStart; set => _gameStart = value; }
        /// <summary>
        /// Accessing my timers. 0 is game timer, 1 is hint timer
        /// </summary>
        public List<Timer> Timers { get => _timers; set => _timers = value; }
       
        public static List<Label> NameLabels { get => _nameLabels; set => _nameLabels = value; }
        public static List<Label> ScoreLabels { get => _scoreLabels; set => _scoreLabels = value; }
        public static List<HighScore> HighScores { get => _highScores; set => _highScores = value; }

        #endregion
        #region odd methods
        public Card CardByPBID(PictureBox pb)
        {
            for (int i = 0; i < _pictureBoxes.Count; i++)
            {
                if (pb == _pictureBoxes[i])
                {
                    Console.WriteLine("returning card: " + i);
                    Deck.IndexOf(i).Pb = PictureBoxes[i];
                    return Deck.IndexOf(i);
                }
            }
            return null;
        }

   
        public List<T> Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<List<T>>(path);
        }

        #endregion
    }
}
