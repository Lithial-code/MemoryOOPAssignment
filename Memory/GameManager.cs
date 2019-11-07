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
        bool _paused;
        bool _canClick;

        int _seconds;
        int _minutes;
        int _cardCount;

    

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

            PictureBoxes = new List<PictureBox>() { };
            Labels = new List<Label>();    
            Timers = new List<Timer>();
            NameLabels = new List<Label>();
            ScoreLabels = new List<Label>();
            HighScores = new List<HighScore>();
            Paused = false;
            CanClick = false;
            Seconds = 0;
        }

        /// <summary>
        /// Called at Game Start. Shuffles deck and sets lables to appropriate values
        /// </summary>
        public void SetupBoard()
        {
            //full board reset
            Seconds = 0;
            ResetBoard();
            Deck = new Deck();
            CardCount = Deck.Count();
            //deal cards
            for (int i = 0; i < Deck.UsableDeck.Count; i++)
            {
                PictureBoxes[i].Visible = true;
                Deck.UsableDeck[i].Pb = PictureBoxes[i];
            }
            //reset timer to 0
            Labels[0].Text = "00:00:00";
            //reset card count
            Labels[1].Text = CardCount.ToString();
        }
        
        /// <summary>
        /// Creates an array of fake high scores to populate the array for testing
        /// </summary>
        public void AssembleFakeScores()
        {
            for (int i = 0; i < 10; i++)
            {
                HighScore highScore = new HighScore("Score" + i, i * 10);
                HighScores.Add(highScore);
            }
            SaveScores();
        }
        /// <summary>
        /// Compares if cards match
        /// </summary>
        /// <returns>true if they do false if they dont</returns>
        public bool DoCardsMatch()
        {
            if (!IsGameFinished())
            {
                return FirstCard.ImageLocation == SecondCard.ImageLocation;
            }
            return false;
        }
        /// <summary>
        /// Changes the background image of a picture box to the the cards image.
        /// </summary>
        /// <param name="card">cards image to use</param>
        public void FlipCards(Card card)
        {
            Console.WriteLine("Flip card: " + card.Name);

            card.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(card.ImageLocation);
            //SecondCard.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(SecondCard.ImageLocation);
        }
        /// <summary>
        /// resets the image back to null
        /// </summary>
        public void FlipCardsBack()
        {
            Console.WriteLine("Flip cards back");
            if (!IsGameFinished())
            {
                FirstCard.Pb.Image = null;
                SecondCard.Pb.Image = null;
            }
        }
        /// <summary>
        /// clears the placeholder card slots
        /// </summary>
        public void NullCards()
        {
            FirstCard = null;
            SecondCard = null;
        }
        /// <summary>
        /// Removes and disables the pictureboxes so they are unseen
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCardsAndPictureBoxes(Card card)
        {
            card.Pb.Enabled = false;
            card.Pb.Visible = false;
        }
        /// <summary>
        /// Lowers the card count variable by the input
        /// </summary>
        /// <param name="cardsKilled"></param>
        public void LowerCardCount(int cardsKilled)
        {
            CardCount -= cardsKilled;
            Labels[1].Text = CardCount.ToString();
        }
        /// <summary>
        /// Checks if game is finished
        /// </summary>
        /// <returns>true if card count is 0</returns>
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
        /// <summary>
        /// Flips all cards over for 2 seconds. Uses Timer[1]
        /// </summary>
        public void ActivateHint()
        {
            if (GameStart && CanClick)
            {
                Timers[1].Start();
                foreach (Card card in Deck.UsableDeck)
                {
                    card.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(card.ImageLocation);
                }
            }
         
        }
        /// <summary>
        /// Undoes hint image change and stops timer[1]
        /// </summary>
        public void DeactivateHint()
        {
            foreach (Card card in Deck.UsableDeck)
            {
                card.Pb.Image = null;
            }
            Timers[1].Stop();
        }
        /// <summary>
        /// Doesnt actually check score. Instead adds to highscore list and shows high score forms
        /// </summary>
        /// <param name="name">player name</param>
        public void CheckBestScore(string name)
        {
            HighScore highScore = new HighScore(name,Seconds);
            HighScores.Add(highScore);

            RecordLabels();
            
            GameBoard.highScoreForm.Show();
            SaveScores();
        }
 /// <summary>
 /// method for saving the scores from json
 /// </summary>
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
        /// <summary>
        /// method for loading the scores from json
        /// </summary>
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
        /// <summary>
        /// Sorts high score list then dumps them into the appropriate labels
        /// </summary>
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
        /// <summary>
        /// Called at the end of the game. Stops the game timer and asks for player name. then calls CheckBestScore to do high score stuff at the end
        /// </summary>
        public void EndGame()
        {
            Timers[0].Stop();
            GameStart = false;
            Console.WriteLine("Endgame");
            string UserAnswer = Microsoft.VisualBasic.Interaction.InputBox("Please Enter Your Name:  ", "You Win", "name:");
            CheckBestScore(UserAnswer);   
        }
        /// <summary>
        /// Resets the picture boxes to their default state
        /// </summary>
        public void ResetBoard()
        {
            foreach (PictureBox pictureBox in _pictureBoxes)
            {
                pictureBox.Enabled = true;
                pictureBox.Visible = true;
                pictureBox.BackgroundImage = Properties.Resources.DragonCardBack;
                pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                
            }
        }

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
        public bool Paused { get => _paused; set => _paused = value; }
        public bool CanClick { get => _canClick; set => _canClick = value; }

        #endregion
        #region odd methods
        /// <summary>
        /// Takes in a picture box and returns the associated card.
        /// </summary>
        /// <param name="pb"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Used to deserialize list of cards
        /// </summary>
        /// <typeparam name="T">Should be card type</typeparam>
        /// <param name="path">data path</param>
        /// <returns></returns>
        public List<T> Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<List<T>>(path);
        }

        #endregion
    }
}
