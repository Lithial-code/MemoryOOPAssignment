using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;

namespace Memory
{
    public partial class GameBoard : Form
    {

        public GameManager GameManager;
        readonly List<PictureBox> pictureBoxes;
        public static HighScoreForm highScoreForm;
        
   

        //internal GameManager GameManager { get => GameManager; set => GameManager = value; }

        public GameBoard()
        { 
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
          
            GameManager = new GameManager();
            pictureBoxes = new List<PictureBox>() { pb1, pb2, pb3, pb4, pb5, pb6, pb7, pb8, pb9, pb10, pb11, pb12, pb13, pb14, pb15, pb16, pb17, pb18, pb19, pb20, pb21, pb22, pb23, pb24, pb25, pb26, pb27, pb28, pb29, pb30, pb31, pb32, pb33, pb34, pb35, pb36 };
            highScoreForm = new HighScoreForm();
            AssembleComponents();
            GameManager.LoadScores();

        }
        #region methods
        /// <summary>
        /// Called to connect the form components to the game manager. Is probably pretty Janky but it seems to work well enough
        /// </summary>
        private void AssembleComponents()
        {
            foreach (var pb in pictureBoxes)
            {
                GameManager.PictureBoxes.Add(pb);
            }
            GameManager.Labels.Add(lbl_time);
            GameManager.Labels.Add(lbl_cards_remaining);
            GameManager.Timers.Add(GameTimer);
            GameManager.Timers.Add(HintTimer);
       
       
            // highScoreForm.AssembleFakeScores();
        }
        #endregion

        #region events
        /// <summary>
        /// Event for when cards are clicked. Checks for first or second card. Adds to the appropriate 
        /// slot in the game manager and then flips the card. When the second card is clicked the wait timer 
        /// starts so that the cards have time to display before they get flipped back. Is a better alternative
        /// to thread.sleep
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardClickEvent(object sender, EventArgs e)
        {
            if (GameManager.GameStart && GameManager.CanClick)
            {


                if (GameManager.FirstCard == null && GameManager.SecondCard == null)
                {
                    GameManager.FirstCard = GameManager.CardByPBID((PictureBox)sender);
                    // Console.WriteLine("click 1: pass 1 " + gameManager.FirstCard.Name);
                    GameManager.FlipCards(GameManager.FirstCard);


                }
                else if (GameManager.FirstCard != null && GameManager.SecondCard == null)
                {
                    GameManager.SecondCard = GameManager.CardByPBID((PictureBox)sender);
                    GameManager.FlipCards(GameManager.SecondCard);

                    //Console.WriteLine("Second card: "+gameManager.SecondCard.ImageLocation);
                    //gameManager.SecondCard.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(gameManager.SecondCard.ImageLocation);
                    Console.WriteLine("Sleep now");
                    WaitTimer.Start();

                }
            }
        }

        /// <summary>
        /// Event for when the card flip delay happens. Makes sure the pictures get to load before they are flipped back to the default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardFlipTimerEvent(object sender, EventArgs e)
        {
            if (GameManager.GameStart && GameManager.CanClick)
            {


                if (GameManager.DoCardsMatch())
                {
                    Console.WriteLine("Yes i got here ok");
                    GameManager.RemoveCardsAndPictureBoxes(GameManager.FirstCard);
                    GameManager.RemoveCardsAndPictureBoxes(GameManager.SecondCard);
                    GameManager.LowerCardCount(2);
                    GameManager.NullCards();
                    //Endgame code
                    if (GameManager.IsGameFinished())
                    {
                        GameManager.EndGame();

                    }
                }
                else
                {
                    GameManager.FlipCardsBack();
                    GameManager.NullCards();
                }
                WaitTimer.Stop();
            }
        }

        /// <summary>
        /// Event for the Game counter. Converts time to proper form (00:00:00)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTimeCounterEvent(object sender, EventArgs e)
        {
            GameManager.Seconds++;
            GameManager.Minutes = (int)Math.Floor((decimal)GameManager.Seconds / 60);
            lbl_time.Text = GameManager.Minutes.ToString("D2") + ":" + (GameManager.Seconds % 60).ToString("D2") + ":00";
            Console.WriteLine("Tick");
        }
        /// <summary>
        /// Starts the game timer when the new game button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameEvent(object sender, EventArgs e)
        {

            GameManager.SetupBoard();
            GameTimer.Start();
            GameManager.GameStart = true;
            GameManager.CanClick = true;

        }
        /// <summary>
        /// Quits the game when the Quit button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitGameEvent(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Cheat button for instant win
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_cheat_Click(object sender, EventArgs e)
        {
            if (GameManager.GameStart && GameManager.CanClick)
            {
                foreach (Card card in GameManager.Deck.UsableDeck)
                {
                    GameManager.RemoveCardsAndPictureBoxes(card);
                    GameManager.LowerCardCount(1);
                }
                if (GameManager.IsGameFinished())
                {
                    GameManager.EndGame();

                }
            }   
        }
        /// <summary>
        /// Hint button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_hint_Click(object sender, EventArgs e)
        {
  
            GameManager.ActivateHint();
        }
        /// <summary>
        /// Hint timer on tick method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HintTick(object sender, EventArgs e)
        {
            GameManager.DeactivateHint();
        }
        /// <summary>
        /// Opens high score screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHighScores(object sender, EventArgs e)
        {
            if (!GameManager.GameStart)
            {
                highScoreForm.Show();
            }
        }

        private void Pause(object sender, EventArgs e)
        {
           
            if (GameManager.GameStart)
            {
                GameManager.CanClick = !GameManager.CanClick;
                GameManager.Paused = !GameManager.Paused;
                if (GameManager.Paused)
                {
                    GameTimer.Stop();
                }
                else
                {
                    GameTimer.Start();
                }
            }
       
        }
    }
    #endregion

    

}
