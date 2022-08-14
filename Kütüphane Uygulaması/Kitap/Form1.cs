using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Kitap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection VTBaglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|/kitaplar.accdb");

        public void Baglanti()
        {
            if (VTBaglanti.State != ConnectionState.Open)
            {
                VTBaglanti.Open();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Admin" && textBox2.Text == "12345")
            {
                MessageBox.Show("Giriş başarılı");
                this.Hide();
                Form2 frm2 = new Form2();
                frm2.Show();
            }
            else
            {
                MessageBox.Show("Hatalı giriş");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }




        bool kontrol=false;
        OleDbDataReader oku;
        public static int ogrencino;
        private void button2_Click(object sender, EventArgs e)
        {
            Baglanti();
            OleDbCommand komut = new OleDbCommand("select * from ogrenciler",VTBaglanti);
            oku = komut.ExecuteReader();

            while (oku.Read())
            {
                if (textBox4.Text == oku["OgrenciNo"].ToString() && textBox3.Text == oku["tcNo"].ToString())
                {
                    ogrencino = Convert.ToInt32(oku["OgrenciNo"]);
                    kontrol = true;
                    break;
                }
            }
            if (kontrol == true)
            {
                MessageBox.Show("Giriş başarılı");
                Form3 frm3 = new Form3();
                this.Hide();
                frm3.Show();
            }
            else
            {
                MessageBox.Show("hatalı giriş");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

