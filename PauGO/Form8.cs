using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PauGO
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private string connectionString = "Data Source=DESKTOP-O04MK08\\SQLEXPRESS;Initial Catalog=PauGo;Integrated Security=True";

        private void button1_Click(object sender, EventArgs e)
        {
            // User_Id'yi Form1'den alıyoruz
            string userId = Form1.CurrentUserId;

            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("Kullanıcı girişi yapılmamış!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TextBox'lardaki verileri alıyoruz
            string scooterId = textBox2.Text;
            string sorunAciklama = textBox1.Text;
            DateTime bakimTarihi = DateTime.Now; // Bugünün tarihini alıyoruz

            // SQL sorgusunu yazıyoruz
            string query = "INSERT INTO Bakim (Scooter_Id, SorunAciklama, BakimTarih, User_id) " +
                           "VALUES (@ScooterId, @SorunAciklama, @BakimTarih, @UserId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    // Parametreleri ekliyoruz
                    command.Parameters.AddWithValue("@ScooterId", scooterId);
                    command.Parameters.AddWithValue("@SorunAciklama", sorunAciklama);
                    command.Parameters.AddWithValue("@BakimTarih", bakimTarihi);
                    command.Parameters.AddWithValue("@UserId", userId);  // Giriş yapan kullanıcının User_id'sini ekliyoruz

                    // Sorguyu çalıştırıyoruz
                    command.ExecuteNonQuery();

                    MessageBox.Show("Teknik destek kaydı başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
