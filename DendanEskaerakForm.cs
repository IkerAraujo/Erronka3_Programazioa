using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Dendan jasotzeko dauden eskaerak erakutsi eta entregatuta markatzeko.
    // Langile Arruntaren "Dendan prest" botoitik irekitzen da.
    public class DendanEskaerakForm : Form
    {
        private ListBox lstEskaerak;
        private Button  btnEntregatu;
        private Button  btnEzabatu;
        private Button  btnFreshatu;
        private Button  btnItxi;
        private Label   lblEgoera;

        // Zerrendan dauden eskaeren kopiak (index bat-etortzea)
        private List<Eskaera> _eskaerak = new List<Eskaera>();

        public DendanEskaerakForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — Dendan jasotzeko", 520, 470);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            EraikiForms();
            EskaerakKargatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "🏪  Dendan jasotzeko eskaerak",
                Estiloak.Urdina, "Eskaera bat hautatu eta entregatu.");

            this.Controls.Add(Estiloak.LabelSortu("Jasotzeko prest dauden eskaerak:",
                20, 90, 460, 22, Estiloak.FontH3, Estiloak.TestuArgia));

            lstEskaerak = Estiloak.ListBoxSortu(20, 115, 470, 185);
            this.Controls.Add(lstEskaerak);


            btnEntregatu = Estiloak.BotoiNagusiaSortu("✔  Entregatuta markatu",
                20, 315, 220, 44, Estiloak.Berdea);
            btnEntregatu.Click += new EventHandler(btnEntregatu_Click);
            this.Controls.Add(btnEntregatu);

            btnEzabatu = Estiloak.BotoiNagusiaSortu("✖  Eskaera ezabatu",
                250, 315, 220, 44, Color.FromArgb(180, 40, 40));
            btnEzabatu.Click += new EventHandler(btnEzabatu_Click);
            this.Controls.Add(btnEzabatu);

            btnFreshatu = Estiloak.BotoiBigarrenaSortu("↺  Freskatu", 20, 370, 110, 28);
            btnFreshatu.Click += new EventHandler(btnFreshatu_Click);
            this.Controls.Add(btnFreshatu);

            btnItxi = Estiloak.BotoiBigarrenaSortu("Itxi", 390, 370, 100, 28);
            btnItxi.Click += new EventHandler(btnItxi_Click);
            this.Controls.Add(btnItxi);

            lblEgoera = Estiloak.LabelSortu("",
                20, 408, 470, 22, Estiloak.FontTxiki, Estiloak.Berdea);
            lblEgoera.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblEgoera);
        }

        private void EskaerakKargatu()
        {
            lstEskaerak.Items.Clear();
            _eskaerak.Clear();
            lblEgoera.Text = "";

            try
            {
                List<Eskaera> guztiak =
                    DatuBasea.EskaerakEgoeraren(EgoeraMota.BanatzekoZain);

                for (int i = 0; i < guztiak.Count; i++)
                {
                    // Etxez-etxekoak baztertu — horiek Banatzailearenak dira
                    if (!guztiak[i].EtxekoEntrega)
                    {
                        _eskaerak.Add(guztiak[i]);
                        lstEskaerak.Items.Add(guztiak[i]);
                    }
                }

                if (_eskaerak.Count == 0)
                {
                    lblEgoera.ForeColor = Estiloak.TestuArgia;
                    lblEgoera.Text = "Oraingoz ez dago eskaerarik dendan jasotzeko.";
                }
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnEntregatu_Click(object sender, EventArgs e)
        {
            if (lstEskaerak.SelectedIndex < 0)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Hautatu eskaera bat zerrendan.";
                return;
            }

            Eskaera eskaera = _eskaerak[lstEskaerak.SelectedIndex];

            if (!EuskaraElkarrizketa.GaldeBaiEz(
                "#" + eskaera.Id + " eskaera — " + eskaera.BezeroIzena +
                "\nEntregatuta markatu nahi duzu?",
                "Berretsi entrega")) return;

            try
            {
                DatuBasea.EskaeraEgoeraPasatu(eskaera.Id, "Entregatuta");

                lblEgoera.ForeColor = Estiloak.Berdea;
                lblEgoera.Text = "✔  #" + eskaera.Id + " (" + eskaera.BezeroIzena +
                                 ") entregatuta.";

                EskaerakKargatu();
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnEzabatu_Click(object sender, EventArgs e)
        {
            if (lstEskaerak.SelectedIndex < 0)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Hautatu eskaera bat zerrendan.";
                return;
            }

            Eskaera eskaera = _eskaerak[lstEskaerak.SelectedIndex];

            if (!EuskaraElkarrizketa.GaldeBaiEz(
                "#" + eskaera.Id + " eskaera — " + eskaera.BezeroIzena +
                "\nEskaera honek ezabatuko da behin betiko. Ziur zaude?",
                "Eskaera ezabatu")) return;

            try
            {
                DatuBasea.EskaeraEzabatu(eskaera.Id);

                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "✖  #" + eskaera.Id + " (" + eskaera.BezeroIzena + ") ezabatuta.";

                EskaerakKargatu();
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnFreshatu_Click(object sender, EventArgs e)
        {
            EskaerakKargatu();
        }

        private void btnItxi_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
