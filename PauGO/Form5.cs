using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PauGO
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-O04MK08\\SQLEXPRESS;Initial Catalog=PauGo;Integrated Security=True");


        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string ad = textBox3.Text;
            string soyad = textBox2.Text;
            string tcKimlikNo = maskedTextBox2.Text;
            string telNo = maskedTextBox1.Text;
            string sifre = textBox5.Text;
            string sifreTekrar = textBox7.Text;

            // Şifre doğrulaması
            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connection.Open(); // Bağlantıyı aç

                SqlTransaction transaction = connection.BeginTransaction(); // Transaction başlat

                // Kullanıcı ekle ve User_id'yi al
                SqlCommand kullaniciKomut = new SqlCommand(
                    "INSERT INTO Kullaniciler (KullaniciAdi, KullaniciSifre, KullaniciTuru,KullaniciBakiye) OUTPUT INSERTED.User_id VALUES (@kAdi, @kSifre, @kTuru,@bakiye)", connection, transaction);
                kullaniciKomut.Parameters.AddWithValue("@kAdi", kullaniciAdi);
                kullaniciKomut.Parameters.AddWithValue("@kSifre", sifre);
                kullaniciKomut.Parameters.AddWithValue("@kTuru", "Misafir");
                kullaniciKomut.Parameters.AddWithValue("@bakiye", 0);

                // ExecuteScalar ile User_id'yi al
                object result = kullaniciKomut.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                {
                    transaction.Rollback(); // İşlemi geri al
                    MessageBox.Show("Kullanıcı eklenirken bir hata oluştu. User_id değeri alınamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int kullaniciiD = Convert.ToInt32(result);

                // Misafir tablosuna ekleme
                SqlCommand misafirKomut = new SqlCommand(
                    "INSERT INTO Misafir (User_id, Ad, Soyad, TcNo, TelNo) VALUES (@kID, @ad, @soyad, @tcNo, @telNo)", connection, transaction);
                misafirKomut.Parameters.AddWithValue("@kID", kullaniciiD);
                misafirKomut.Parameters.AddWithValue("@ad", ad);
                misafirKomut.Parameters.AddWithValue("@soyad", soyad);
                misafirKomut.Parameters.AddWithValue("@tcNo", tcKimlikNo);
                misafirKomut.Parameters.AddWithValue("@telNo", telNo);


                int affectedRows = misafirKomut.ExecuteNonQuery(); // Misafir tablosuna veri ekle
                if (affectedRows == 0)
                {
                    transaction.Rollback(); // Eğer ekleme yapılmazsa işlemi geri al
                    MessageBox.Show("Misafir kaydını oluştururken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                transaction.Commit(); // Tüm işlemleri başarılı bir şekilde kaydet
                MessageBox.Show("Kayıt başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 form1 = new Form1(); // Giriş yap formunu aç
                form1.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close(); // Bağlantıyı kapat
            }
        }
    }

}
    

