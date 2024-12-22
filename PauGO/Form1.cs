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

namespace PauGO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string connectionString = "Data Source=DESKTOP-O04MK08\\SQLEXPRESS;Initial Catalog=PauGo;Integrated Security=True";


        public static string CurrentUserId { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string kullaniciSifre = textBox2.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Kullanıcı kontrol sorgusu ve bilgileri alma
                    string query = "SELECT User_id, KullaniciAdi, KullaniciBakiye FROM Kullaniciler WHERE KullaniciAdi=@KullaniciAdi AND KullaniciSifre=@KullaniciSifre";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);
                    command.Parameters.AddWithValue("@KullaniciSifre", kullaniciSifre);

                    // Sorguyu çalıştır
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) // Eğer kullanıcı bulunduysa
                    {
                        string userId = reader["User_id"].ToString();
                        string userName = reader["KullaniciAdi"].ToString();
                        string userBalance = reader["KullaniciBakiye"].ToString();

                        MessageBox.Show("Giriş başarılı! Hoş geldiniz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        CurrentUserId = userId;

                        // Form6'ya bilgileri taşı
                        Form6 form6 = new Form6
                        {
                            UserId = userId,
                            UserName = userName,
                            UserBalance = userBalance
                        };

                        form6.Show();
                        this.Hide(); // Form1'i gizle
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
