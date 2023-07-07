﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_Game_on_Net_5
{
    public partial class EasyForm : Form
    {

        List<int> questionNumbers = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6 };
        string firstChoice;
        string secondChoice;
        int tries;

        List<PictureBox> Boxes = new List<PictureBox>();

        PictureBox picA;
        PictureBox picB;

        int totalTime = 45;
        int countDownTime;

        bool gameOver = false;
        private Form1 firstForm;


        public EasyForm(Form1 form1)
        {
            InitializeComponent();
            loadPic();
            firstForm = form1;
        }

        private void loadPic()
        {

            int pLeft = 20;
            int pTop = 20;
            int rows = 0;

            for (int i = 0; i < 12; i++)
            {

                PictureBox nwPic = new PictureBox();
                nwPic.Height = 50;
                nwPic.Width = 50;
                nwPic.BackColor = Color.LightGray;
                nwPic.SizeMode = PictureBoxSizeMode.StretchImage;
                nwPic.Click += NwPic_Click;
                Boxes.Add(nwPic);


                if (rows < 3)
                {
                    rows++;
                    nwPic.Left = pLeft;
                    nwPic.Top = pTop;
                    this.Controls.Add(nwPic);
                    pLeft = pLeft + 60;
                }

                if (rows == 3)
                {
                    pLeft = 20;
                    pTop += 60;
                    rows = 0;
                }

            }

            RestartGame();
        }

        private void NwPic_Click(object sender, EventArgs e)
        {
            if (gameOver)
            {
                return;
            }

            string selectedTheme = firstForm.SelectedTheme;
            if (firstChoice == null)
            {
                picA = sender as PictureBox;

                if (picA.Tag != null && picA.Image == null)
                {
                    
                    if(selectedTheme == "Fruits")
                    {
                        picA.Image = Image.FromFile("fruits/" + (string)picA.Tag + ".png");

                    }

                    if(selectedTheme == "Animals")
                    {
                        picA.Image = Image.FromFile("animals/" + (string)picA.Tag + ".png");

                    }

                    if (selectedTheme == "Halloween")
                    {
                        picA.Image = Image.FromFile("halloween/" + (string)picA.Tag + ".png");

                    }

                    //picA.Image = Image.FromFile("fruits/" + (string)picA.Tag + ".png");
                    firstChoice = (string)picA.Tag;
                }

            }
            else if (secondChoice == null)
            {
                picB = sender as PictureBox;

                if (picB.Tag != null && picB.Image == null)
                {
                    if(selectedTheme == "Fruits") {
                        picB.Image = Image.FromFile("fruits/" + (string)picB.Tag + ".png");
                    }

                    if (selectedTheme == "Animals")
                    {
                        picB.Image = Image.FromFile("animals/" + (string)picB.Tag + ".png");
                    }

                    if (selectedTheme == "Halloween")
                    {
                        picB.Image = Image.FromFile("halloween/" + (string)picB.Tag + ".png");
                    }


                    //picB.Image = Image.FromFile("fruits/" + (string)picB.Tag + ".png");
                    secondChoice = (string)picB.Tag;


                }
            }
            else
            {
                runCheck(picA, picB);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            var randomList = questionNumbers.OrderBy(x => Guid.NewGuid()).ToList();

            // save the random list to the question numbers list again
            questionNumbers = randomList;

            for (int i = 0; i < Boxes.Count; i++)
            {
                Boxes[i].Image = null;
                Boxes[i].Tag = questionNumbers[i].ToString();
            }

            tries = 0;

            statusLabel.Text = "Mismatched: " + tries + " Times";
            txtCountDown.Text = "Time Left: " + totalTime;
            gameOver = false;
            GameTImer.Start();
            countDownTime = totalTime;

        }

        private void runCheck(PictureBox A, PictureBox B)
        {

            if (firstChoice == secondChoice)
            {
                A.Tag = null;
                B.Tag = null;
            }
            else
            {
                tries++;

                statusLabel.Text = "Mismatched: " + tries + " Times";
            }

            firstChoice = null;
            secondChoice = null;

            foreach (PictureBox x in Boxes.ToList())
            {
                if (x.Tag != null)
                {
                    x.Image = null;
                }
            }

            if (Boxes.All(o => o.Tag == Boxes[0].Tag))
            {
                GameOver("Great work, you win ");
            }

        }

        private void TimerEvent(object sender, EventArgs e)
        {

            countDownTime--;

            txtCountDown.Text = "Time Left: " + countDownTime;

            string selectedTheme = firstForm.SelectedTheme;
            if (countDownTime < 1)
            {
                GameOver("Times Up, You Lose!!");

                foreach (PictureBox x in Boxes.ToList())
                {
                    if (x.Tag != null)
                    {
                        if(selectedTheme == "Fruits")
                        {
                            x.Image = Image.FromFile("fruits/" + (string)x.Tag + ".png");
                        }

                        if (selectedTheme == "Animals")
                        {
                            x.Image = Image.FromFile("animals/" + (string)x.Tag + ".png");
                        }

                        if (selectedTheme == "Halloween")
                        {
                            x.Image = Image.FromFile("halloween/" + (string)x.Tag + ".png");
                        }

                        //x.Image = Image.FromFile("fruits/" + (string)x.Tag + ".png");
                    }
                }
            }

        }

        private void GameOver(string msg)
        {
            GameTImer.Stop();
            gameOver = true;
            // if all of the picture boxes dont have any tags left in them. 
            MessageBox.Show(msg + " Click Restart to Play Again", "Moo Says");
        }
    }
}
