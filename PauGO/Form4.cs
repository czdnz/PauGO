using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PauGO
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }

        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-O04MK08\\SQLEXPRESS;Initial Catalog=PauGo;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string personelNo = textBox2.Text;
            string sifre = textBox3.Text;
            string sifreTekrar = textBox4.Text;

            // Şifre doğrulaması
            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // PersonelNo boşsa işlem yapılmasın
            if (string.IsNullOrEmpty(personelNo))
            {
                MessageBox.Show("Personel numarası boş olamaz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(); // Transaction başlat

                // Kullanıcı ekle ve User_id'yi al
                SqlCommand kullaniciKomut = new SqlCommand(
                    "INSERT INTO Kullaniciler (KullaniciAdi, KullaniciSifre, KullaniciTuru,KullaniciBakiye) OUTPUT INSERTED.User_id VALUES (@kAdi, @kSifre, @kTuru,@bakiye)", connection, transaction);
                kullaniciKomut.Parameters.AddWithValue("@kAdi", kullaniciAdi);
                kullaniciKomut.Parameters.AddWithValue("@kSifre", sifre);
                kullaniciKomut.Parameters.AddWithValue("@kTuru", "Personel"); // KullaniciTuru sütununa "Personel" ekliyoruz
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

                // Personel tablosuna ekleme
                SqlCommand personelKomut = new SqlCommand(
                    "INSERT INTO Personel (User_id, Personel_no) VALUES (@kID, @perNo)", connection, transaction);
                personelKomut.Parameters.AddWithValue("@kID", kullaniciiD);
                personelKomut.Parameters.AddWithValue("@perNo", personelNo);

                int affectedRows = personelKomut.ExecuteNonQuery(); // Personel tablosuna veri ekle
                if (affectedRows == 0)
                {
                    transaction.Rollback(); // Eğer ekleme yapılmazsa işlemi geri al
                    MessageBox.Show("Personel kaydını oluştururken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                connection.Close();
            }
        }
    }
}

