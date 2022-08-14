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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }


        OleDbConnection VTBaglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|/kitaplar.accdb");
        DataTable SanalTablo = new DataTable();
        public void Baglanti()
        {
            if (VTBaglanti.State != ConnectionState.Open)
            {
                VTBaglanti.Open();
            }
        }

        void listele()
        {
            SanalTablo.Clear();
            string sorgu = "SELECT * FROM kitapOgrenci WHERE OgrenciNo = " + Convert.ToInt32(Form1.ogrencino);
            OleDbDataAdapter Adaptor = new OleDbDataAdapter(sorgu, VTBaglanti);

            Adaptor.Fill(SanalTablo);
            dataGridView1.DataSource = SanalTablo;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Today;
            listele();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[3].Value);
            dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells[4].Value);
        }

        int gün;
        string eklegün;

        private void button1_Click(object sender, EventArgs e)
        {
            gün = Convert.ToInt32(comboBox1.SelectedItem);
            dateTimePicker2.Value = dateTimePicker2.Value.AddDays(gün);
            eklegün = dateTimePicker2.Value.ToShortDateString();
            string guncelleOgrenciSQL = "UPDATE kitapOgrenci SET verimTarihi='" + eklegün + "' WHERE OgrenciNo=" + Form1.ogrencino;
            OleDbCommand guncelleOgrenciCMD = new OleDbCommand(guncelleOgrenciSQL, VTBaglanti);
            Baglanti();
            guncelleOgrenciCMD.ExecuteNonQuery();

            listele();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.Show();
        }
    }
}
