using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec.Data;//pt codul qr
using System.IO;
using System.Data.SqlClient;

namespace Jocuri
{
    public partial class Autentificare : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        StreamReader reader;
        AlegeJoc callable = new AlegeJoc();
        Inregistrare callable2 = new Inregistrare();
        JocMemorie callable3 = new JocMemorie();
        string line,qr;
        public static string adresa;
        public  static string parola;

        public Autentificare()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.Environment.CurrentDirectory.Replace(@"\bin\Debug", ""));
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Jocuri.mdf;Integrated Security=True");
            callable3.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                conn.Open();

                cmd = new SqlCommand("DELETE FROM Rezultate;", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("DELETE FROM Utilizatori;", conn);
                cmd.ExecuteNonQuery();

                reader = new StreamReader("Utilizatori.txt");
                while((line = reader.ReadLine()) != null)
                {
                    string[] bucati = line.Split(';');
                    cmd = new SqlCommand(String.Format("INSERT INTO Utilizatori VALUES('{0}','{1}','{2}');",bucati[0], bucati[1], bucati[2]), conn);
                    cmd.ExecuteNonQuery();
                }

                reader = new StreamReader("Rezultate.txt");
                while ((line = reader.ReadLine()) != null)
                {
                    string[] bucati = line.Split(';');
                    cmd = new SqlCommand(String.Format("INSERT INTO Rezultate VALUES({0},'{1}',{2},'{3}');", bucati[0], bucati[1], bucati[2],bucati[3]), conn);
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                adresa = textBox1.Text;
                parola = textBox2.Text;
                conn.Open();
                cmd = new SqlCommand(String.Format("SELECT * FROM Utilizatori WHERE(EmailUtilizator='{0}' AND Parola='{1}');",adresa,parola), conn);
                if(cmd.ExecuteScalar() != null)
                {
                    callable.ShowDialog();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Date de autentificare invalide!");
                    textBox1.Clear();
                    textBox2.Clear();
                }
                conn.Close();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            callable2.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title="SELECTATI CODUL QR";
            dialog.InitialDirectory = System.Environment.CurrentDirectory.Replace(@"\Jocuri\Jocuri\bin\Debug", @"\ONTI_2023_C#_Resurse\QRCode");
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                qr = dialog.FileName;
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Image.FromFile(qr);
            // DECRIPTAREA CODULUI
            MessagingToolkit.QRCode.Codec.QRCodeDecoder objDecodare = new  MessagingToolkit.QRCode.Codec.QRCodeDecoder();
            string sirCodare = objDecodare.decode(new MessagingToolkit.QRCode.Codec.Data.QRCodeBitmapImage(pictureBox1.Image as Bitmap));
            string[] bucati = sirCodare.Split('\n');
            textBox1.Text = bucati[1];
            textBox2.Text = bucati[2];
        }
    }
}
