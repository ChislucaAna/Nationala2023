using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Jocuri
{
    public partial class JocLitere : Form
    {
        public JocLitere()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        int secunde, i;
        Random rnd;
        static string[] files = { "0", "bloc.png", "caine.jpg", "caprioara.jpg", "iepure.png", "leu.jpg", "lup.jpg", "masina.png", "minge.jpg", "patine.jpg", "pisica.jpg", "taur.jpg", "urs.png", "vulpe.png" };
        string cuv1;
        string cuv2;
        int bx, by;
        int dir;
        int j;
        int castigat;

        public class litera
        {
            public Label frame;
            public int x;
            public int y;
            public char l;

            public litera(int pozx, int pozy, char c)
            {
                frame = new Label();
                frame.Width = 30;
                frame.Height = 30;
                frame.Location = new System.Drawing.Point(pozx, pozy);
                frame.Font = new Font("Arial", 20);
                x = pozx;
                y = pozy;
                l = c;
            }
        }

        litera[] lit = new litera[30];


        private void timer1_Tick(object sender, EventArgs e)
        {
            secunde++;
            label1.Text = (secunde / 10).ToString();
            if (secunde == 1000)
            {
                MessageBox.Show("Game Over");
            }

            if (dir == 0)
            {
                pictureBox3.Location = new System.Drawing.Point(bx, by - 5);
                by = by - 10;
            }
            if (dir == 1)
            {
                pictureBox3.Location = new System.Drawing.Point(bx - 10, by - 5);
                bx = bx - 10;
                dir = 0;
            }
            if (dir == 2)
            {
                pictureBox3.Location = new System.Drawing.Point(bx + 10, by - 5);
                bx = bx + 10;
                dir = 0;
            }

            //intersectia
            Rectangle ball = new Rectangle(bx, by, 20, 20);
            for (i = 0; i < cuv1.Length + cuv2.Length; i++)
            {
                Rectangle letter = new Rectangle(lit[i].x, lit[i].y, 30, 30);
                if (ball.IntersectsWith(letter))
                {
                    label2.Text += lit[i].l;
                    JocLitere.ActiveForm.Controls.Remove(lit[i].frame);
                    JocLitere.ActiveForm.Refresh();
                    lit[i].x = 0;
                    lit[i].y = 0;
                    label2.Refresh();
                }
            }

            //verificare daca a iesit din joc mingea
            try
            {
                if (bx < 0 || bx > JocLitere.ActiveForm.Width || by < 0 || by > JocLitere.ActiveForm.Height)
                {
                    bx = JocLitere.ActiveForm.Width / 2;
                    by = JocLitere.ActiveForm.Height - 100;
                    pictureBox3.Location = new System.Drawing.Point(bx, by);
                }
            }
            catch(Exception Ex)
            {
                
            }

            //cuvant format
            if(String.Equals(label2.Text,cuv1))
            {
                JocLitere.ActiveForm.Controls.Remove(pictureBox1);
                label2.Text = null;
                label2.Refresh();
                JocLitere.ActiveForm.Refresh();
                castigat++;
            }
            if (String.Equals(label2.Text, cuv2))
            {
                JocLitere.ActiveForm.Controls.Remove(pictureBox2);
                label2.Text = null;
                label2.Refresh();
                JocLitere.ActiveForm.Refresh();
                castigat++;
            }

            if(castigat==2)
            {
                MessageBox.Show("Ai castigat!");
                Application.Exit();
            }
        }


        private void JocLitere_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue=='A' || e.KeyValue=='a')
            {
                dir = 1;
            }
            if (e.KeyValue == 'D' || e.KeyValue == 'd')
            {
                dir = 2;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = null;

            rnd = new Random();
            i = rnd.Next(1, 5);
            pictureBox1.Image = Image.FromFile(files[i]);
            string[] bucatele = files[i].Split('.');
            cuv1 = bucatele[0];

            Thread.Sleep(20);

            rnd = new Random();
            i = rnd.Next(6, 13);
            pictureBox2.Image = Image.FromFile(files[i]);
            bucatele = files[i].Split('.');
            cuv2 = bucatele[0];

            int start = 100 ;
            int p = 20;

            for(i=0; i<cuv1.Length; i++)
            {
                litera val = new litera(start + 2*p, JocLitere.ActiveForm.Height / 10, cuv1[i]); ;
                lit[i] = val;
                lit[i].frame.Text = lit[i].l.ToString();
                JocLitere.ActiveForm.Controls.Add(lit[i].frame);
                JocLitere.ActiveForm.Refresh();

                start = start + 2*p;
            }

            for (j = 0; j < cuv2.Length; j++)
            {
                litera val = new litera(start + 2*p, JocLitere.ActiveForm.Height / 10, cuv2[j]); ;
                lit[i] = val;
                lit[i].frame.Text = lit[i].l.ToString();
                JocLitere.ActiveForm.Controls.Add(lit[i].frame);
                JocLitere.ActiveForm.Refresh();
                start = start + 2*p;
                i++;
            }

            pictureBox3.Image = Image.FromFile("ball.png");
            bx = JocLitere.ActiveForm.Width / 2;
            by = JocLitere.ActiveForm.Height - 100;
            pictureBox3.Location = new System.Drawing.Point(bx, by);
            JocLitere.ActiveForm.Controls.Add(pictureBox3);

            timer1.Start();
        }
    }
}
