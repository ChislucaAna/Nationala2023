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
    // SE APASA BUTONUL START->pornesti timerul
    //1.adaugi n pictureboxuri in form la distante si coordonate bine stabilite
    //2.faci Pictureboxurile toate sa fie clickable si cand dai click pe unu aflii coordonatele clickului
    //3.in functie de coordonate, determini ce index are pictureboxul pe care ai fat click
    //4.stiind picturebox ul, extragi din array ul de imagini imaginea corespunzatoare(daca e pe r1)
    //sau fisierul in cauza din array, daca e pe r2;
    //6.afisezi cardul pt 1 secunda
    //7. daca mai exista un card afisat, verifici concordanta, altfel nimic
    //8. daca sunt in pereche, pe randul 2 apare numarul de ordine al panoului de pe randul 1
    //9. daca nu sunt pereche continuturile redevin ascunse

    //castigat e 1 daca ai completat cele 4 niveluri in sub 100 de secunde
    //DACA TREC 100 DE SECUNDE SI CASTIGAT==0 atunci ai pierdut
    //daca castigat ==1 atunci ai imagine

    public partial class JocMemorie : Form
    {
        public JocMemorie()
        {
            InitializeComponent();
        }

        static int n = 3, nr, i,x;
        static int k = 0;
        static string[] files = { "0", "bloc.png","caine.jpg","caprioara.jpg","iepure.png","leu.jpg","lup.jpg","masina.png","minge.jpg","patine.jpg","pisica.jpg","taur.jpg","urs.png","vulpe.png" };
        static int[] frecv = new int[20];
        static int[] compare = new int[3];
        static int perechi=0;
        static int ticks=0;
        static FlowLayoutPanel flowLayoutPanel1;

        public class panou
        {
            public PictureBox card;
            public int index;
            public Image imagine;
            public string fisier;
            public int shown;

            public panou(int i,Image img,string file,int ok)
            {
                card = new PictureBox();
                card.BackColor = Color.Red;
                card.Click += new EventHandler(swap);
                card.Margin = new Padding(3,3,3,3);
                card.SizeMode = PictureBoxSizeMode.StretchImage;
                

                index = i;
                imagine = img;
                fisier = file;
                shown = ok;
            }

            public void swap(object sender, EventArgs e)
            {
                if (shown == 0)
                {
                    card.Image = Image.FromFile(fisier);
                    card.Refresh();
                    shown = 1;
                    k++;
                    compare[k] = index;
                }
                if (k == 2)
                {
                    Thread.Sleep(500);

                    if (String.Equals(panouri[compare[1]].fisier, panouri[compare[2]].fisier))
                    {
                        using (Font myFont = new Font("Arial", 7))
                        {
                            Graphics g = panouri[compare[2]].card.CreateGraphics();
                            g.Clear(Color.White);
                            string text = String.Format("{0} {1}", panouri[compare[2]].index.ToString(), panouri[compare[2]].fisier.ToString());
                            g.DrawString(text, myFont, Brushes.Black, new Point(2, 2));
                        }
                        perechi++;
                    }
                    else
                    {
                        panouri[compare[1]].card.Image = null;
                        panouri[compare[1]].shown = 0;
                        panouri[compare[1]].card.Refresh();

                        panouri[compare[2]].card.Image = null;
                        panouri[compare[2]].shown = 0;
                        panouri[compare[2]].card.Refresh();
                    }
                    k = 0;
                    if (perechi == nr / 2)
                    {
                        MessageBox.Show("Ai trecut nivelul");
                        n++;
                        perechi = 0;
                        start(n);
                    }
                }
            }
        }

        static public panou[] panouri = new panou[20];

        public static int F(int n)
        {
            if (n <= 2)
            {
                return 1;
            }
            else
            {
                return F(n - 1) + F(n - 2);
            }
        }

        public static void start(int n)
        {
            if(n==6)
            {
                MessageBox.Show("CASTIGAT!");
                Application.Exit();
            }

            nr = 2 * F(n);
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Width = nr / 2 * 110;

            for (i = 1; i <= nr; i++)
            {
                do
                {
                    Random rnd;
                    rnd = new Random();
                    x = rnd.Next(1, nr / 2 + 1);
                    Thread.Sleep(20);
                } while (frecv[x] == 2);
                frecv[x]++;
                Image img = Image.FromFile(files[x]);
                string file = files[x];
                panou val = new panou(i, img, file, 0);
                panouri[i] = val;
                flowLayoutPanel1.Controls.Add(panouri[i].card);
            }

            flowLayoutPanel1.Refresh();
            JocMemorie.ActiveForm.Refresh();
            Array.Clear(frecv, 0, 19);
        }

        public void timer1_Tick(object sender, EventArgs e)
        {            
            label1.Text = ticks.ToString();
            ticks++;

            if (ticks == 100)
            {
                MessageBox.Show("AI PIERDUT");
                Application.Exit();
            }
        }
        
        public void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanel1.Location = new System.Drawing.Point(100, 100);
            flowLayoutPanel1.Height = 120;
            flowLayoutPanel1.BackColor = Color.Green;
            JocMemorie.ActiveForm.Controls.Add(flowLayoutPanel1);

            start(n);
        }
    }
}
