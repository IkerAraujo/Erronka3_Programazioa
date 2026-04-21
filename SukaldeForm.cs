using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Sukaldariaren pantaila: eskaera aktiboak kudeatzeko
    public class SukaldeForm : Form
    {
        private readonly Sukaldaria _erabiltzailea;

        private ListBox     lstZain;
        private ListBox     lstSukaldatzen;
        private RichTextBox rtbInfo;
        private Label       lblZainKop;
        private Label       lblSukKop;
        private Button      btnHasi;
        private Button      btnPrest;
        private Button      btnFreshatu;
        private Button      btnItxiSaioa;

        public SukaldeForm(Sukaldaria erabiltzailea)
        {
            _erabiltzailea = erabiltzailea;
            Estiloak.FormEzarri(this,
                "EuskoPizza — Sukaldea · " + erabiltzailea.Izena, 760, 580);
            EraikiForms();
            EskaerаkFreshatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "👨‍🍳 Sukaldea",
                Color.FromArgb(180, 60, 40),
                "Kaixo, " + _erabiltzailea.Izena + "! Kudeatu eskaera aktiboak.");

            int Y = 88;

            Panel pEzk = Estiloak.PanelSortu(15, Y, 340, 310, Estiloak.PanelIluna);

            Label lEzk = Estiloak.LabelSortu("⏳  Prestatzeko zain",
                12, 10, 280, 22, Estiloak.FontH3, Color.FromArgb(255, 180, 60));
            lblZainKop = Estiloak.LabelSortu("0 eskaera",
                250, 10, 80, 22, Estiloak.FontTxiki, Estiloak.TestuArgia);
            lblZainKop.TextAlign = ContentAlignment.MiddleRight;

            lstZain = Estiloak.ListBoxSortu(10, 40, 318, 258);
            lstZain.SelectedIndexChanged += new EventHandler(lst_SelectedChanged);

            pEzk.Controls.AddRange(new Control[] { lEzk, lblZainKop, lstZain });
            this.Controls.Add(pEzk);

            btnHasi = Estiloak.BotoiNagusiaSortu("Hasi »",
                365, Y + 105, 85, 40, Estiloak.Laranja);
            btnHasi.Click += new EventHandler(btnHasi_Click);
            this.Controls.Add(btnHasi);

            btnPrest = Estiloak.BotoiNagusiaSortu("✔ Prest",
                365, Y + 160, 85, 40, Estiloak.Berdea);
            btnPrest.Click += new EventHandler(btnPrest_Click);
            this.Controls.Add(btnPrest);

            btnFreshatu = Estiloak.BotoiBigarrenaSortu("↺",
                365, Y + 215, 85, 36);
            btnFreshatu.Font   = new Font("Segoe UI", 14);
            btnFreshatu.Click += new EventHandler(btnFreshatu_Click);
            this.Controls.Add(btnFreshatu);

            btnItxiSaioa = Estiloak.BotoiBigarrenaSortu("🔓  Itxi saioa",
                365, Y + 265, 85, 36);
            btnItxiSaioa.ForeColor = Color.FromArgb(255, 120, 120);
            btnItxiSaioa.Click += new EventHandler(btnItxiSaioa_Click);
            this.Controls.Add(btnItxiSaioa);

            Panel pEsk = Estiloak.PanelSortu(460, Y, 280, 310, Estiloak.PanelIluna);

            Label lEsk = Estiloak.LabelSortu("🔥  Sukaldatzen",
                12, 10, 200, 22, Estiloak.FontH3, Color.FromArgb(255, 120, 50));
            lblSukKop = Estiloak.LabelSortu("0 eskaera",
                190, 10, 80, 22, Estiloak.FontTxiki, Estiloak.TestuArgia);
            lblSukKop.TextAlign = ContentAlignment.MiddleRight;

            lstSukaldatzen = Estiloak.ListBoxSortu(10, 40, 258, 258);
            lstSukaldatzen.SelectedIndexChanged += new EventHandler(lst_SelectedChanged);

            pEsk.Controls.AddRange(new Control[] { lEsk, lblSukKop, lstSukaldatzen });
            this.Controls.Add(pEsk);

            this.Controls.Add(Estiloak.LabelSortu("Eskaeraren xehetasunak",
                15, Y + 320, 300, 22, Estiloak.FontTxiki, Estiloak.TestuArgia));
            rtbInfo = Estiloak.InfoPanelSortu(15, Y + 343, 725, 90);
            rtbInfo.Text = "Hautatu eskaera bat informazioa ikusteko...";
            this.Controls.Add(rtbInfo);
        }

        public void EskaerаkFreshatu()
        {
            lstZain.Items.Clear();
            lstSukaldatzen.Items.Clear();

            try
            {
                List<Eskaera> zain = DatuBasea.EskaerakEgoeraren(EgoeraMota.PrestatzekoZain);
                List<Eskaera> suk  = DatuBasea.EskaerakEgoeraren(EgoeraMota.Sukaldatzen);

                for (int i = 0; i < zain.Count; i++)
                    lstZain.Items.Add(zain[i]);

                for (int i = 0; i < suk.Count; i++)
                    lstSukaldatzen.Items.Add(suk[i]);

                lblZainKop.Text = zain.Count + " eskaera";
                lblSukKop.Text  = suk.Count  + " eskaera";
            }
            catch (Exception ex)
            {
                rtbInfo.Text = "⚠ DB errorea freskatzen: " + ex.Message;
            }
        }

        private void btnHasi_Click(object sender, EventArgs e)
        {
            if (lstZain.SelectedItem == null)
            {
                EuskaraElkarrizketa.Mezua("Hautatu eskaera bat ezkerreko zerrendatik.", "Abisua");
                return;
            }

            try
            {
                Eskaera ek = (Eskaera)lstZain.SelectedItem;
                ek.EgoeraPasatu();
                DatuBasea.EskaeraEgoeraPasatu(ek.Id, DatuBasea.EgoeraTestura(ek.Egoera));
                EskaerаkFreshatu();
            }
            catch (Exception ex)
            {
                EuskaraElkarrizketa.Mezua("DB errorea: " + ex.Message, "Errorea", errorea: true);
            }
        }

        private void btnPrest_Click(object sender, EventArgs e)
        {
            if (lstSukaldatzen.SelectedItem == null)
            {
                EuskaraElkarrizketa.Mezua("Hautatu eskaera bat eskuineko zerrendatik.", "Abisua");
                return;
            }

            try
            {
                Eskaera ek = (Eskaera)lstSukaldatzen.SelectedItem;
                ek.EgoeraPasatu();
                DatuBasea.EskaeraEgoeraPasatu(ek.Id, DatuBasea.EgoeraTestura(ek.Egoera));
                EskaerаkFreshatu();
                rtbInfo.Text = "✔  #" + ek.Id + " eskaera prest dago.";
            }
            catch (Exception ex)
            {
                EuskaraElkarrizketa.Mezua("DB errorea: " + ex.Message, "Errorea", errorea: true);
            }
        }

        private void btnFreshatu_Click(object sender, EventArgs e)
        {
            EskaerаkFreshatu();
        }

        private void btnItxiSaioa_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is LoginForm login)
                {
                    login.EremuakGarbitu();
                    login.Show();
                    break;
                }
            }
            this.Close();
        }

        private void lst_SelectedChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            if (lb.SelectedItem == null) return;

            Eskaera ek = (Eskaera)lb.SelectedItem;

            string mota = "";
            if (ek.EtxekoEntrega)
                mota = "🛵 Etxez-etxe → " + ek.BezeroHelbidea;
            else
                mota = "🏠 Dendan jasoko du";

            rtbInfo.Text = "#" + ek.Id + "  ·  " + ek.BezeroIzena + "  ·  " + mota + "\n";
            rtbInfo.Text += "Pizzak: ";

            for (int i = 0; i < ek.Pizzak.Count; i++)
                rtbInfo.Text += ek.Pizzak[i].ToString() + "  |  ";

            rtbInfo.Text += "\nTotala: " + ek.PrezioTotala.ToString("F2") + "€";
            rtbInfo.Text += "   ·   Ordua: " + ek.Data.ToString("HH:mm");
        }
    }
}
