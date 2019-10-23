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

        GameManager gameManager = new GameManager();
        List<PictureBox> pictureBoxes;

        public GameBoard()
        {
            InitializeComponent();
            pictureBoxes = new List<PictureBox>() { pb1, pb2,pb3, pb4, pb5, pb6, pb7, pb8, pb9, pb10, pb11, pb12, pb13, pb14, pb15, pb16, pb17, pb18, pb19, pb20, pb21, pb22, pb23, pb24, pb25, pb26, pb27, pb28, pb29, pb30, pb31, pb32, pb33, pb34, pb35, pb36};
            AssembleComponents();
        }
        #region methods
        /// <summary>
        /// Called to connect the form components to the game manager. Is probably pretty Janky but it seems to work well enough
        /// </summary>
        private void AssembleComponents()
        {
            foreach (var pb in pictureBoxes)
            {
                gameManager.PictureBoxes.Add(pb);
            }
            gameManager.Labels.Add(lbl_time);
            gameManager.Labels.Add(lbl_cards_remaining);
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
            if (gameManager.FirstCard == null && gameManager.SecondCard == null)
            {
                gameManager.FirstCard = gameManager.CardByPBID((PictureBox)sender);
               // Console.WriteLine("click 1: pass 1 " + gameManager.FirstCard.Name);
                gameManager.FlipCards(gameManager.FirstCard);
   
                
            }
            else if (gameManager.FirstCard != null && gameManager.SecondCard == null)
            {
                gameManager.SecondCard = gameManager.CardByPBID((PictureBox)sender);
                gameManager.FlipCards(gameManager.SecondCard);

                //Console.WriteLine("Second card: "+gameManager.SecondCard.ImageLocation);
                //gameManager.SecondCard.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(gameManager.SecondCard.ImageLocation);
                Console.WriteLine("Sleep now");
                WaitTimer.Start();
            
            }

        }

        /// <summary>
        /// Event for when the card flip delay happens. Makes sure the pictures get to load before they are flipped back to the default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardFlipTimerEvent(object sender, EventArgs e)
        {
            if (gameManager.DoCardsMatch())
            {
                Console.WriteLine("Yes i got here ok");
                gameManager.RemoveCardsAndPictureBoxes(gameManager.FirstCard);
                gameManager.RemoveCardsAndPictureBoxes(gameManager.SecondCard);
                gameManager.LowerCardCount();
                gameManager.NullCards();
                //TODO - put the game finishing code here maybe? Also make minutes a part of game manager
                if (gameManager.IsGameFinished())
                {
                    MessageBox.Show("Game over", string.Format("You finished the game in{0} minutes and {1} seconds", gameManager.Seconds /60, gameManager.Seconds%60)) ;
                }
            }
            else
            {
                gameManager.FlipCardsBack();
                gameManager.NullCards();
            }
            WaitTimer.Stop();
        }

        /// <summary>
        /// Event for the Game counter. Converts time to proper form (00:00:00)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTimeCounterEvent(object sender, EventArgs e)
        {
            gameManager.Seconds ++;
            gameManager.Minutes = (int)Math.Floor((decimal)gameManager.Seconds / 60);
            lbl_time.Text = gameManager.Minutes.ToString("D2") + ":" + (gameManager.Seconds % 60).ToString("D2") + ":00";
            Console.WriteLine("Tick");
        }
        /// <summary>
        /// Starts the game timer when the new game button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameEvent(object sender, EventArgs e)
        {
            gameManager.SetupBoard();
            GameTimer.Start();

       
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
    }
    #endregion

}
