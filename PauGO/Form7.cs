using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PauGO
{
    public partial class Form7 : Form
    {
        public string UserBalance { get; set; }
        public Form7()
        {
            InitializeComponent();
            Load += Form7_Load;
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            // Form6'dan gelen bakiyeyi label3'e yazdır
            label3.Text = $"{UserBalance} TL";
        }

     
        
        
        private void button1_Click(object sender, EventArgs e)
        {

            decimal yeniBakiye;
            bool isValid = decimal.TryParse(textBox1.Text, out yeniBakiye);

            if (isValid && yeniBakiye > 0)
            {
                // Yeni bakiyeyi hesapla
                UserBalance = (decimal.Parse(UserBalance) + yeniBakiye).ToString();

                // Form6'daki UserBalance'ı güncelle
                Form6 form6 = (Form6)Application.OpenForms["Form6"];
                if (form6 != null)
                {
                    form6.UpdateBalance(UserBalance); // Form6'da bakiyeyi güncelledik
                }

                // Form7'yi kapat
                this.Close();
            }
            else
            {
                MessageBox.Show("Geçersiz bakiye miktarı", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
