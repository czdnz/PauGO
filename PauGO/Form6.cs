using System;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace PauGO
{
    public partial class Form6 : Form
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserBalance { get; set; }

        public Form6()
        {
            InitializeComponent();
            Load += Form6_Load;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // Kullanıcı bilgilerini formda göster
            label2.Text = UserName; // Kullanıcı adı
            label4.Text = UserBalance + " TL"; // Bakiye
            label5.Text = UserId;
        }
        public void UpdateBalance(string newBalance)
        {
            UserBalance = newBalance;
            label4.Text = newBalance + " TL"; // Bakiyeyi güncelle
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7
            {
                UserBalance = UserBalance // UserBalance'ı Form7'ye aktarıyoruz
            };
            form7.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();
        }
        private decimal GetNumericBalance()
        {
            // "TL" ekini kaldır ve geçerli bir sayıya dönüştür
            string temizBakiye = label4.Text.Replace(" TL", "").Trim();
            if (decimal.TryParse(temizBakiye, out decimal bakiye))
            {
                return bakiye;
            }
            else
            {
                // Eğer bakiye değeri geçersizse, varsayılan olarak 0 döndürüyoruz
                return 0;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            decimal bakiye = GetNumericBalance();

            if (bakiye < 100)
            {
                MessageBox.Show("Sürüşe başlamak için minimum 100 TL olması gerekiyor. Lütfen bakiye yükleyiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Lütfen sürüşü başlatmak için QR'ı okutun.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }




        private void button4_Click(object sender, EventArgs e)
        {
           
            {
                string userId = label5.Text;  // Form6'da label5'e yazdırdık

                // Form9'a kullanıcı id'sini gönderiyoruz
                Form9 form9 = new Form9(userId);  // Parametre olarak User_id'yi gönderiyoruz
                form9.Show();
                this.Hide();
            }

        }
    }
}
