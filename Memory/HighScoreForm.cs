using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Memory
{
    public partial class HighScoreForm : Form
    {
        public static GameBoard gameBoard;
        public List<Label> nameLabels;
        public List<Label> scoreLabels;

      
        public HighScoreForm()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            nameLabels = new List<Label>() { lbl_name_0, lbl_name_1, lbl_name_2, lbl_name_3, lbl_name_4, lbl_name_5, lbl_name_6, lbl_name_7, lbl_name_8, lbl_name_9 };
            scoreLabels = new List<Label>() { lbl_score_0, lbl_score_1, lbl_score_2, lbl_score_3, lbl_score_4, lbl_score_5, lbl_score_6, lbl_score_7, lbl_score_8, lbl_score_9 };

            AssembleComponents();


        }



        public void AssembleComponents()
        {
            foreach (Label label in nameLabels)
            {
                GameManager.NameLabels.Add(label);
            }
            foreach (Label label in scoreLabels)
            {
                GameManager.ScoreLabels.Add(label);
            }
        }
        public void UpdateForm()
        {
            for (int i = 0; i < 10; i++)
            {
                if (GameManager.HighScores[i] != null)
                {
                    nameLabels[i].Text = GameManager.HighScores[i].Name;
                    scoreLabels[i].Text = GameManager.HighScores[i].ToReadable();
                }  
            }
        }
 
 
        #region events
        private void QuitEvent(object sender, EventArgs e)
        {
            gameBoard = new GameBoard();
            gameBoard.Show();
            this.Hide();
        }
        #endregion

    }


}
