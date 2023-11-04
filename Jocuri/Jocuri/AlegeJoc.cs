using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Jocuri
{
    public partial class AlegeJoc : Form
    {
        SqlDataReader reader;
        SqlConnection conn;
        SqlCommand cmd;
        JocMemorie callable = new JocMemorie();
        string data, joc, punctaj, dataant, jocant, punctajant;

        private void button1_Click(object sender, EventArgs e)
        {
            callable.ShowDialog();
        }

        int index0, index1;
        public AlegeJoc()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.Environment.CurrentDirectory.Replace(@"\bin\Debug", ""));
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Jocuri.mdf;Integrated Security=True");
        }

        private void AlegeJoc_Load(object sender, EventArgs e)
        {
            label1.Text = Autentificare.adresa;

            try
            {
                conn.Open();
                cmd = new SqlCommand(String.Format("SELECT * FROM Rezultate WHERE EmailUtilizator='{0}'", Autentificare.adresa), conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    joc = reader[1].ToString();
                    punctaj = reader[3].ToString();
                    data = reader[4].ToString();
                    //0 reprezintă jocul Testeaza memoria
                    //1 reprezintă jocul Popice cu litere
                    if (Convert.ToInt32(joc)==0)
                    {
                        
                        if (Convert.ToInt32(joc) == Convert.ToInt32(jocant) && String.Equals(data, dataant) == true && index0!=1)
                        {
                            if (Convert.ToInt32(punctaj) > Convert.ToInt32(punctajant))
                            {
                                chart1.Series["Testeaza Memoria"].Points.AddXY(data, punctaj);
                                index0 = chart1.Series["Testeaza Memoria"].Points.Count();
                                chart1.Series["Testeaza Memoria"].Points.RemoveAt(index0 - 1);
                            }
                        }
                        else
                        {
                            chart1.Series["Testeaza Memoria"].Points.AddXY(data, punctaj);
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(joc) == Convert.ToInt32(jocant) && String.Equals(data, dataant) == true && index1 != 1)
                        {
                            if (Convert.ToInt32(punctaj) > Convert.ToInt32(punctajant))
                            {
                                chart1.Series["Popice cu litere"].Points.AddXY(data, punctaj);
                                index1 = chart1.Series["Popice cu litere"].Points.Count();
                                chart1.Series["Popice cu litere"].Points.RemoveAt(index1 - 1);
                            }
                        }
                        else
                        {
                            chart1.Series["Popice cu litere"].Points.AddXY(data, punctaj);
                        }
                    }
                    dataant = data;
                    jocant = joc;
                    punctajant = punctaj;
                }
                reader.Close();
                conn.Close();
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}
