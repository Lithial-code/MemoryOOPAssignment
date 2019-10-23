using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

        private void CardClickEvent(object sender, EventArgs e)
        {
            if (gameManager.FirstCard == null && gameManager.SecondCard == null)
            {
                gameManager.FirstCard = gameManager.CardByPBID((PictureBox)sender);
                Console.WriteLine("click 1: pass 1 " + gameManager.FirstCard.Name);

                gameManager.FlipCards(gameManager.FirstCard);
                //Console.WriteLine(gameManager.FirstCard.ImageLocation);
           
                //gameManager.FirstCard.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(gameManager.FirstCard.ImageLocation);
                //Console.WriteLine(gameManager.FirstCard.Pb.Image.ToString());
                
            }
            else if (gameManager.FirstCard != null && gameManager.SecondCard == null)
            {
                gameManager.SecondCard = gameManager.CardByPBID((PictureBox)sender);
                Console.WriteLine("click 1 pass 2 " + gameManager.FirstCard.Name);

                Console.WriteLine("click 2: pass 2" + gameManager.SecondCard.Name);

                gameManager.FlipCards(gameManager.SecondCard);

                //Console.WriteLine("Second card: "+gameManager.SecondCard.ImageLocation);
                //gameManager.SecondCard.Pb.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(gameManager.SecondCard.ImageLocation);
                Console.WriteLine("Sleep now");

                System.Threading.Thread.Sleep(2500);

                if (gameManager.docardsmatch())
                {
                    Console.WriteLine("Yes i got here ok");
                }
                else
                {
                   gameManager.FlipCardsBack();
                   gameManager.NullCards();
                }
            }

        }
        private void AssembleComponents()
        {
            foreach (var pb in pictureBoxes)
            {
                gameManager.PictureBoxes.Add(pb);
            }
            gameManager.Labels.Add(lbl_time);
            gameManager.Labels.Add(lbl_cards_remaining);
        }
    }


}
