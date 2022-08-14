using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace Kitap
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        OleDbConnection VTBaglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source=|DataDirectory|/kitaplar.accdb");

        //Sanal Tablolar

        DataTable SanalTablo = new DataTable();
        DataTable SanalTablo1 = new DataTable();
        DataTable SanalTablo2 = new DataTable();
        DataTable SanalTablo3 = new DataTable();

        DataTable STAra = new DataTable();


        public static string sorgu,sorgu2,sorgu3,sorgu4,sorgum;


        public void Baglanti()
        {
            if (VTBaglanti.State != ConnectionState.Open)
            {
                VTBaglanti.Open();
            }
        }
        private void tabControl1_Deselected(object sender, TabControlEventArgs e)         //TAB GEÇİŞ KONTROLÜ
        {
            SanalTablo2.Clear();
            Baglanti();
            sorgu2 = "SELECT * FROM kitapOgrenci ";
            OleDbDataAdapter Adaptor2 = new OleDbDataAdapter(sorgu2, VTBaglanti);
            Adaptor2.Fill(SanalTablo2);
            dataGridView3.DataSource = SanalTablo2;

            SanalTablo1.Clear();
            Baglanti();
            sorgu = "SELECT * FROM kitaplar ";
            OleDbDataAdapter Adaptor = new OleDbDataAdapter(sorgu, VTBaglanti);
            Adaptor.Fill(SanalTablo1);
            dataGridView2.DataSource = SanalTablo1;

            SanalTablo3.Clear();
            Baglanti();
            sorgu3 = "SELECT * FROM ogrenciler";
            OleDbDataAdapter Adaptor3 = new OleDbDataAdapter(sorgu3, VTBaglanti);
            Adaptor3.Fill(SanalTablo3);
            dataGridView1.DataSource = SanalTablo3;
        }


           //TAB1 Başlangıç

        string k_adi, tur, k_tarih, y_evi, yazar;
        int b_no, s_sayisi;
        public void KayitlariGoster(string kelime) //TAB1 Kayıtları Göster Method
        {
            SanalTablo.Clear();
            Baglanti();
            sorgu = "SELECT * FROM kitaplar WHERE kitapAdi LIKE '%" + kelime + "%' OR Yazar LIKE '%" + kelime + "%' ORDER BY BarkodNo ASC"; //arama yapma
            OleDbDataAdapter Adaptor = new OleDbDataAdapter(sorgu, VTBaglanti);
            Adaptor.Fill(SanalTablo);
            dataGridView2.DataSource = SanalTablo;
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();                                                       
            textBox2.Clear();
            textBox3.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.SelectedItem = null;                                   
        }   
        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)   //TAB1 DataClick
        {
            textBox6.Text = b_no.ToString();
            b_no = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value);
            textBox1.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            comboBox1.SelectedItem = dataGridView2.CurrentRow.Cells[4].Value;
            textBox5.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView2.CurrentRow.Cells[6].Value);
        }
        private void button1_Click(object sender, EventArgs e)   //TAB1 kaydet  
        {
            try
            {
                k_adi = textBox1.Text;
                y_evi = textBox3.Text;
                yazar = textBox2.Text;
                s_sayisi = Convert.ToInt32(textBox5.Text);
                tur = comboBox1.SelectedItem.ToString();
                b_no = Convert.ToInt32(textBox6.Text);
                k_tarih = dateTimePicker1.Value.ToShortDateString();
                tur = comboBox1.SelectedItem.ToString();
                string kayitSQL = "INSERT INTO kitaplar (BarkodNo,kitapAdi, Yazar, yayinEvi, Turu,sayfaSayisi, kayitTarihi) VALUES ('" + b_no + "','" + k_adi + "','" + yazar + "','" + y_evi + "','" + tur + "','" + s_sayisi + "','" + k_tarih + "')";
                OleDbCommand kayitCMD = new OleDbCommand(kayitSQL, VTBaglanti);
                Baglanti();
                kayitCMD.ExecuteNonQuery();
                VTBaglanti.Close();
                KayitlariGoster("");
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Boş alan bırakmayınız");
            }
        }
        private void button3_Click(object sender, EventArgs e)  //TAB1 güncelle 
        {
            try
            {
                k_adi = textBox1.Text;
                y_evi = textBox3.Text;
                yazar = textBox2.Text;
                s_sayisi = Convert.ToInt32(textBox5.Text);
                tur = comboBox1.SelectedItem.ToString();
                b_no = Convert.ToInt32(textBox6.Text);
                k_tarih = dateTimePicker1.Value.ToShortDateString();
                tur = comboBox1.SelectedItem.ToString();
                string guncelleSQL = "UPDATE kitaplar SET kitapAdi='" + k_adi + "', Yazar='" + yazar + "',yayinEvi='" + y_evi + "', Turu='" + tur + "',sayfaSayisi='" + s_sayisi + "', kayitTarihi='" + k_tarih + "' WHERE BarkodNo=" + b_no;
                OleDbCommand guncelleCMD = new OleDbCommand(guncelleSQL, VTBaglanti);
                Baglanti();
                guncelleCMD.ExecuteNonQuery();
                VTBaglanti.Close();
                KayitlariGoster("");
            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Boş alan bırakmayınız.");
            }
        }
        private void button4_Click(object sender, EventArgs e)  //TAB1 Sil  
        {
            DialogResult silmeOnayi = MessageBox.Show("Seçili kaydı silmek istiyor musunuz?", "Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (silmeOnayi == DialogResult.Yes)
            {
                string silmeSQL = "DELETE FROM kitaplar WHERE BarkodNo=" + b_no;
                OleDbCommand silmeCMD = new OleDbCommand(silmeSQL, VTBaglanti);
                Baglanti();
                silmeCMD.ExecuteNonQuery();
                VTBaglanti.Close();
                KayitlariGoster("");
            }
        }
        private void textBox7_Click_1(object sender, EventArgs e)  //TAB1 Arama 
        {
            textBox7.Clear();
            textBox7.ForeColor = Color.Black;
        }
        private void button2_Click_1(object sender, EventArgs e)     //TAB1 Arama Buttonu
        {
            textBox7.Focus();
            textBox7.Clear();
            textBox7.ForeColor = Color.Black;
            KayitlariGoster("");
        }
        private void textBox7_TextChanged_1(object sender, EventArgs e)  //TAB1 Arama 
        {
            KayitlariGoster(textBox7.Text);
        }
        private void textBox7_Leave_1(object sender, EventArgs e)   //TAB1 Arama 
        {
            textBox7.ForeColor = Color.Gray;

        }





        //TAB2 Başlangıç

        int o_no;       
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)  //DataClick
        {
            

            o_no = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            textBox4.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox2.SelectedItem = dataGridView1.CurrentRow.Cells[2].Value;
            textBox16.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox10.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox17.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }  
        private void button8_Click(object sender, EventArgs e)  //TAB2 Kaydet
        {
            try
            {
                o_no = Convert.ToInt32(textBox4.Text);
                string kayitOgrenciSQL = "INSERT INTO ogrenciler (OgrenciNo,adiSoyadi,fakulte,bolum,tcNo,mail,telNo,ceza) VALUES ('" + o_no + "','" + textBox9.Text + "','" + comboBox2.SelectedItem + "','" + textBox16.Text + "','" + textBox8.Text + "','" + textBox10.Text + "','" + textBox17.Text + "','" + "YOK" + "') ";
                OleDbCommand kayitOgrenciCMD = new OleDbCommand(kayitOgrenciSQL, VTBaglanti);
                Baglanti();
                kayitOgrenciCMD.ExecuteNonQuery();
                //VTBaglanti.Close();

                SanalTablo3.Clear();
                Baglanti();
                sorgu3 = "SELECT * FROM ogrenciler";
                OleDbDataAdapter Adaptor3 = new OleDbDataAdapter(sorgu3, VTBaglanti);
                Adaptor3.Fill(SanalTablo3);
                dataGridView1.DataSource = SanalTablo3;
                textBox4.Clear();
                textBox9.Clear();
                textBox16.Clear();
                textBox8.Clear();
                textBox10.Clear();
                textBox17.Clear();
                comboBox2.SelectedItem = null;

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Boş alan bırakmayınız");
            }
        }
        private void button9_Click(object sender, EventArgs e)  //TAB2 Güncelle
        {
            try
            {
                o_no = Convert.ToInt32(textBox4.Text);
                string guncelleOgrenciSQL = "UPDATE ogrenciler SET adiSoyadi='" + textBox9.Text + "', fakulte='" + comboBox2.SelectedItem + "', bolum='" + textBox16.Text + "', tcNo='" + textBox8.Text + "', mail='" + textBox10.Text + "', telNo='" + textBox17.Text + "', ceza='" + "YOK" + "' WHERE OgrenciNo=" + o_no;
                OleDbCommand guncelleOgrenciCMD = new OleDbCommand(guncelleOgrenciSQL, VTBaglanti);
                Baglanti();
                guncelleOgrenciCMD.ExecuteNonQuery();
                //VTBaglanti.Close();

                SanalTablo3.Clear();
                Baglanti();
                sorgu3 = "SELECT * FROM ogrenciler"; 
                OleDbDataAdapter Adaptor3 = new OleDbDataAdapter(sorgu3, VTBaglanti);
                Adaptor3.Fill(SanalTablo3);
                dataGridView1.DataSource = SanalTablo3;
                textBox4.Clear();
                textBox9.Clear();
                textBox16.Clear();
                textBox8.Clear();
                textBox10.Clear();
                textBox17.Clear();
                comboBox2.SelectedItem = null;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Güncellenemedi.");
            }
        }
        private void button10_Click(object sender, EventArgs e)  //TAB2 Sil
        {
            DialogResult silmeOnayi = MessageBox.Show("Seçili kaydı silmek istiyor musunuz?", "Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (silmeOnayi == DialogResult.Yes)
            {
                o_no = Convert.ToInt32(textBox4.Text);
                string silmeSQL = "DELETE FROM ogrenciler WHERE OgrenciNo=" + o_no;
                OleDbCommand silmeCMD = new OleDbCommand(silmeSQL, VTBaglanti);
                Baglanti();
                silmeCMD.ExecuteNonQuery();
                //VTBaglanti.Close();
                SanalTablo3.Clear();
                Baglanti();
                sorgu3 = "SELECT * FROM ogrenciler";
                OleDbDataAdapter Adaptor3 = new OleDbDataAdapter(sorgu3, VTBaglanti);
                Adaptor3.Fill(SanalTablo3);
                dataGridView1.DataSource = SanalTablo3;
                textBox4.Clear();
                textBox9.Clear();
                textBox16.Clear();
                textBox8.Clear();
                textBox10.Clear();
                textBox17.Clear();
                comboBox2.SelectedItem = null;
            }
        }
        public void ARA(string sozcuk) //TAB2 Arama Method
        {
            STAra.Clear();
            Baglanti();
            sorgum = "SELECT * FROM ogrenciler WHERE adiSoyadi LIKE '%" + sozcuk + "%' ORDER BY OgrenciNo ASC"; //arama yapma
            OleDbDataAdapter AdaptorAra = new OleDbDataAdapter(sorgum, VTBaglanti);
            AdaptorAra.Fill(STAra);
            dataGridView1.DataSource = STAra;
        }
        private void textBox11_Click(object sender, EventArgs e)
        {
            textBox11.Clear();
            ARA("");
        } //TAB2 Ara

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            ARA(textBox11.Text);
        } //TAB2 Ara
        private void button6_Click(object sender, EventArgs e)  //TAB2 Arama
        {
            ARA("");
        }





        //TAB3 Başlangıç

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)   //TAB3 DataClick
        {
            textBox12.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            b_no = Convert.ToInt32(dataGridView3.CurrentRow.Cells[0].Value);
            textBox13.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();
            textBox14.Text = dataGridView3.CurrentRow.Cells[2].Value.ToString();            
            dateTimePicker2.Value = Convert.ToDateTime(dataGridView3.CurrentRow.Cells[3].Value);
            dateTimePicker3.Value = Convert.ToDateTime(dataGridView3.CurrentRow.Cells[4].Value);
            textBox15.Text = dataGridView3.CurrentRow.Cells[5].Value.ToString();
        }
        private void button5_Click(object sender, EventArgs e) //TAB3 Ödünç Ver 
        {
            //DateTime date = DateTime.Now.AddDays(10);
            b_no = Convert.ToInt32(textBox12.Text);
            dateTimePicker3.Value = DateTime.Today.AddDays(10);
            string oduncVer="INSERT INTO kitapOgrenci (BarkodNo,kitapAdi, OgrenciNo, alimTarihi, verimTarihi, durum) VALUES ('" + b_no + "','" + textBox13.Text + "','" + textBox14.Text + "','" + dateTimePicker2.Value.ToShortDateString() + "','" + dateTimePicker3.Value.ToShortDateString() + "','"+ "Teslim Edilmedi" +"')";
            OleDbCommand kayitCMD = new OleDbCommand(oduncVer, VTBaglanti);
            Baglanti();
            kayitCMD.ExecuteNonQuery();
            
            SanalTablo2.Clear();
            Baglanti();
            sorgu2 = "SELECT * FROM kitapOgrenci ";
            OleDbDataAdapter Adaptor2 = new OleDbDataAdapter(sorgu2, VTBaglanti);
            Adaptor2.Fill(SanalTablo2);
            dataGridView3.DataSource = SanalTablo2;
        }
        private void button7_Click(object sender, EventArgs e)  //TAB3 Teslim Edildi
        {
            b_no = Convert.ToInt32(textBox12.Text);
            string guncelleDurumSQL = "UPDATE kitapOgrenci SET  durum='" + "Teslim Edildi" + "' WHERE BarkodNo=" + b_no;
            OleDbCommand guncelleDurumCMD = new OleDbCommand(guncelleDurumSQL, VTBaglanti);
            Baglanti();
            guncelleDurumCMD.ExecuteNonQuery();
            VTBaglanti.Close();

            SanalTablo2.Clear();
            Baglanti();
            sorgu2 = "SELECT * FROM kitapOgrenci ";
            OleDbDataAdapter Adaptor = new OleDbDataAdapter(sorgu2, VTBaglanti);
            Adaptor.Fill(SanalTablo2);
            dataGridView3.DataSource = SanalTablo2;
        }
        OleDbDataReader oku;
        private void textBox12_TextChanged(object sender, EventArgs e) // TAB3 BarkodNo'ya göre Kitap Araması
        {
            Baglanti();
            OleDbCommand komut = new OleDbCommand("select * from kitaplar", VTBaglanti);
            oku = komut.ExecuteReader();
            while (oku.Read())
            {
                if (textBox12.TextLength == 6)
                {
                    if (textBox12.Text == oku[0].ToString())
                    {
                        textBox13.Text = oku[1].ToString();
                        label21.Text = " ";
                        break;
                    }
                    else
                    {
                        label21.Text = "Bu kitap kütüphanede yok lütfen ekleyiniz.";
                    }
                }

            }

        }
        private void button11_Click(object sender, EventArgs e)  //Çıkış 
        {
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.Show();
        }







        




        
        

        private void Form2_Load(object sender, EventArgs e)
        {
            KayitlariGoster("");
            ARA("");
        }
    }
}
