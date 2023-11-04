using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Jocuri
{
    public partial class Inregistrare : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        public Inregistrare()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.Environment.CurrentDirectory.Replace(@"\bin\Debug", ""));
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Jocuri.mdf;Integrated Security=True");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string adresa = textBox1.Text;
                string nume = textBox2.Text;
                string parola = textBox3.Text;
                string parola2 = textBox4.Text;

                // fa validarea structurii adresei

                conn.Open();
                cmd = new SqlCommand(String.Format("SELECT * FROM Utilizatori WHERE(EmailUtilizator='{0}' AND Parola='{1}');", adresa, parola), conn);
                if (cmd.ExecuteScalar() != null)
                {
                    MessageBox.Show("contul exista deja!");
                }
                else
                {
                    conn.Close();
                    this.Close();
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            
        }
    }
}
