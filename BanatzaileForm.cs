using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Banatzailearen pantaila.
    // Etxez-etxeko banaketa kudeatzeko.
    public class BanatzaileForm : Form
    {
        private readonly Banatzailea _erabiltzailea;

        private ListBox     lstZain;
        private ListBox     lstBanatzen;
        private RichTextBox rtbInfo;
        private Label       lblZainKop;
        private Label       lblBanKop;
        private Button      btnHartu;
        private Button      btnEntregatu;
        private Button      btnFreshatu;
        private Button      btnItxiSaioa;

        public BanatzaileForm(Banatzailea erabiltzailea)
        {
            _erabiltzailea = erabiltzailea;
            Estiloak.FormEzarri(this,
                "EuskoPizza — Banatzailea · " + erabiltzailea.Izena, 760, 560);
            EraikiForms();
            EskaerаkFreshatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "🛵 Banatzailea",
                Color.FromArgb(50, 100, 200),
                "Kaixo, " + _erabiltzailea.Izena + "! Kudeatu etxez-etxeko entregatak.");

            int Y = 88;


            Panel pEzk = Estiloak.PanelSortu(15, Y, 330, 295, Estiloak.PanelIluna);

            Label lEzk = Estiloak.LabelSortu("📦  Banatzeko prest",
                12, 10, 240, 22, Estiloak.FontH3, Color.FromArgb(100, 180, 255));
            lblZainKop = Estiloak.LabelSortu("0 eskaera",
                245, 10, 75, 22, Estiloak.FontTxiki, Estiloak.TestuArgia);
            lblZainKop.TextAlign = ContentAlignment.MiddleRight;

            lstZain = Estiloak.ListBoxSortu(10, 40, 308, 243);
            lstZain.SelectedIndexChanged += new EventHandler(lst_SelectedChanged);

            pEzk.Controls.AddRange(new Control[] { lEzk, lblZainKop, lstZain });
            this.Controls.Add(pEzk);


            btnHartu = Estiloak.BotoiNagusiaSortu("Hartu »",
                358, Y + 95, 88, 40, Estiloak.Urdina);
            btnHartu.Click += new EventHandler(btnHartu_Click);
            this.Controls.Add(btnHartu);

            btnEntregatu = Estiloak.BotoiNagusiaSortu("✔ Entregatu",
                358, Y + 150, 88, 40, Estiloak.Berdea);
            btnEntregatu.Click += new EventHandler(btnEntregatu_Click);
            this.Controls.Add(btnEntregatu);

            btnFreshatu = Estiloak.BotoiBigarrenaSortu("↺",
                358, Y + 205, 88, 36);
            btnFreshatu.Font   = new Font("Segoe UI", 14);
            btnFreshatu.Click += new EventHandler(btnFreshatu_Click);
            this.Controls.Add(btnFreshatu);

            btnItxiSaioa = Estiloak.BotoiBigarrenaSortu("🔓  Itxi saioa",
                358, Y + 255, 88, 36);
            btnItxiSaioa.ForeColor = Color.FromArgb(255, 120, 120);
            btnItxiSaioa.Click += new EventHandler(btnItxiSaioa_Click);
            this.Controls.Add(btnItxiSaioa);


            Panel pEsk = Estiloak.PanelSortu(458, Y, 288, 295, Estiloak.PanelIluna);

            Label lEsk = Estiloak.LabelSortu("🚗  Bidean",
                12, 10, 200, 22, Estiloak.FontH3, Color.FromArgb(130, 220, 130));
            lblBanKop = Estiloak.LabelSortu("0 eskaera",
                200, 10, 80, 22, Estiloak.FontTxiki, Estiloak.TestuArgia);
            lblBanKop.TextAlign = ContentAlignment.MiddleRight;

            lstBanatzen = Estiloak.ListBoxSortu(10, 40, 265, 243);
            lstBanatzen.SelectedIndexChanged += new EventHandler(lst_SelectedChanged);

            pEsk.Controls.AddRange(new Control[] { lEsk, lblBanKop, lstBanatzen });
            this.Controls.Add(pEsk);


            this.Controls.Add(Estiloak.LabelSortu("Eskaeraren xehetasunak",
                15, Y + 305, 300, 22, Estiloak.FontTxiki, Estiloak.TestuArgia));
            rtbInfo = Estiloak.InfoPanelSortu(15, Y + 328, 730, 80);
            rtbInfo.Text = "Hautatu eskaera bat informazioa ikusteko...";
            this.Controls.Add(rtbInfo);
        }

        public void EskaerаkFreshatu()
        {
            lstZain.Items.Clear();
            lstBanatzen.Items.Clear();

            int zainKop = 0;
            int banKop  = 0;

            try
            {
                List<Eskaera> zain     = DatuBasea.EskaerakEgoeraren(EgoeraMota.BanatzekoZain);
                List<Eskaera> banatzen = DatuBasea.EskaerakEgoeraren(EgoeraMota.Banatzen);

                for (int i = 0; i < zain.Count; i++)
                {
                    if (zain[i].EtxekoEntrega)
                    {
                        lstZain.Items.Add(zain[i]);
                        zainKop++;
                    }
                }

                for (int i = 0; i < banatzen.Count; i++)
                {
                    lstBanatzen.Items.Add(banatzen[i]);
                    banKop++;
                }

                lblZainKop.Text = zainKop + " eskaera";
                lblBanKop.Text  = banKop  + " eskaera";
            }
            catch (Exception ex)
            {
                rtbInfo.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnHartu_Click(object sender, EventArgs e)
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
                DatuBasea.BanaketaHasi(ek.Id, _erabiltzailea.Id);
                EskaerаkFreshatu();
            }
            catch (Exception ex)
            {
                EuskaraElkarrizketa.Mezua("DB errorea: " + ex.Message, "Errorea", errorea: true);
            }
        }

        private void btnEntregatu_Click(object sender, EventArgs e)
        {
            if (lstBanatzen.SelectedItem == null)
            {
                EuskaraElkarrizketa.Mezua("Hautatu eskaera bat 'Bidean' zerrendatik.", "Abisua");
                return;
            }

            Eskaera ek = (Eskaera)lstBanatzen.SelectedItem;

            if (!EuskaraElkarrizketa.GaldeBaiEz(
                "#" + ek.Id + " eskaera entregatu al da?\n" +
                ek.BezeroIzena + " · " + ek.BezeroHelbidea,
                "Berretsi entrega")) return;

            try
            {
                ek.EgoeraPasatu();
                DatuBasea.EskaeraEgoeraPasatu(ek.Id, DatuBasea.EgoeraTestura(ek.Egoera));
                DatuBasea.BanaketaAmaitu(ek.Id);
                EskaerаkFreshatu();
                rtbInfo.Text = "✔  #" + ek.Id + " eskaera entregatu da.";
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

            rtbInfo.Text  = "#" + ek.Id + "  ·  " + ek.BezeroIzena + "\n";
            rtbInfo.Text += "Helbidea: " + ek.BezeroHelbidea + "\n";
            rtbInfo.Text += "Totala: " + ek.PrezioTotala.ToString("F2") + "€";
            rtbInfo.Text += "   ·   Ordua: " + ek.Data.ToString("HH:mm");
        }
    }
}
