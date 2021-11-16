using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsiniBul
{
    public partial class Form1 : Form
    {
        List<Button> butonlar;
        List<Button> aciklar;
        const string sembolHavuzu = "$₺€¥₾₽₪₦";
        char[] semboller;
        int kapananAdet = 0;

        public Form1()
        {
            InitializeComponent();
            ButonlariOlustur();
            SembolleriOlustur();
        }

        private void SembolleriOlustur()
        {
            semboller = new char[16];
            Array.Copy(sembolHavuzu.ToArray(), semboller, 8);
            Array.Copy(sembolHavuzu.ToArray(), 0, semboller, 8, 8);
            Karistir(semboller);
        }

        private void Karistir(char[] semboller)
        {
            Random rnd = new Random();
            char temp;
            int talihliIndeks;
            for (int i = 0; i < semboller.Length - 1; i++)
            {
                talihliIndeks = rnd.Next(i, semboller.Length);
                temp = semboller[i];
                semboller[i] = semboller[talihliIndeks];
                semboller[talihliIndeks] = temp;
            }
        }

        private void ButonlariOlustur()
        {
            aciklar = new List<Button>();
            butonlar = new List<Button>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(90, 90);
                    btn.Font = new Font("Times New Roman", 24f);
                    btn.Location = new Point(j * 100, i * 100);
                    btn.Click += Btn_Click;
                    pnlButonlar.Controls.Add(btn);
                    butonlar.Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button tiklanan = (Button)sender;
            if (aciklar.Contains(tiklanan)) return; // zaten açıksa metottan çık

            if (aciklar.Count == 2) // önceden açık olan 2 adet kart varsa, kapat
                AcikKartlariKapat();

            // tıklanan kartın sembolünü göster
            aciklar.Add(tiklanan);
            int i = butonlar.IndexOf(tiklanan);
            tiklanan.Text = semboller[i].ToString();

            // eğer 2. kart açıldıysa ve aynıysa
            if (aciklar.Count == 2 && aciklar[0].Text == aciklar[1].Text)
            {
                Refresh(); // metot sonlanmadan arayüzü tazele (açılanları görmek için)
                Thread.Sleep(500); // gizlemeden önce yarım saniye görsün
                AcikKartlariKapatVeGizle();
                kapananAdet += 2;
            }

            // hiç görünen kart kalmadıysa
            if (kapananAdet == 16)
            {
                MessageBox.Show("Oyun bitti!");
                btnTekrarOyna.Show();
            }
        }

        private void AcikKartlariKapat()
        {
            foreach (Button button in aciklar)
            {
                button.Text = "";
            }
            aciklar.Clear();
        }

        private void AcikKartlariKapatVeGizle()
        {
            foreach (Button button in aciklar)
            {
                button.Text = "";
                button.Hide();
            }
            aciklar.Clear();
        }

        private void btnTekrarOyna_Click(object sender, EventArgs e)
        {
            pnlButonlar.Controls.Clear();
            ButonlariOlustur();
            SembolleriOlustur();
            kapananAdet = 0;
            aciklar.Clear();
            btnTekrarOyna.Hide();
        }
    }
}
