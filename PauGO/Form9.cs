using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PauGO
{
    public partial class Form9 : Form
    {
        // Veritabanı bağlantı dizesi
        private string connectionString = "Data Source=DESKTOP-O04MK08\\SQLEXPRESS;Initial Catalog=PauGo;Integrated Security=True";
        private string userId; // Kullanıcı ID'sini tutmak için değişken

        public Form9(string userId)
        {
            InitializeComponent();
            this.userId = userId;

            // Form yüklendiğinde çağrılacak olay
            Load += Form9_Load;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            // Kullanıcı ID kontrolü
            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("Kullanıcı ID'si boş. Form kapatılıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Formu kapatıyoruz çünkü işlem yapılamaz
                return;
            }

            // Veritabanına bağlantı testi
            if (!TestConnection())
            {
                MessageBox.Show("Veritabanına bağlanılamadı. Form kapatılıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Formu kapatıyoruz çünkü bağlantı kurulamadı
                return;
            }

            // Veritabanından geçmiş sürüşleri yükle
            LoadGecmisSurusler();
        }

        // Veritabanına bağlantıyı test eder
        private bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Bağlantıyı test et
                    return true; // Bağlantı başarılı
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Bağlantı hatası
            }
        }

        // Geçmiş sürüşleri veritabanından yükler
        private void LoadGecmisSurusler()
        {
            try
            {
                // Doğru sütun adlarını kontrol ederek sorguyu düzenledik
                string query = "SELECT Surus_Id, [Scooter_ıd], BaslamaSaati, BitisSüresi, Tarih FROM GecmisSurusler WHERE User_id = @User_id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@User_id", userId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    // Verileri doldur
                    adapter.Fill(dataTable);

                    // Eğer veri varsa DataGridView'e ata, yoksa uyarı ver
                    if (dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dataTable;
                    }
                    else
                    {
                        MessageBox.Show("Hiçbir geçmiş sürüş kaydı bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri çekme hatası: " + ex.Message, "Veri Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form6 form6 = new Form6();
            form6.Show();
            this.Close();
        }
    }
}
