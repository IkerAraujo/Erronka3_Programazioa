using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    public class LangileForm : Form
    {
        private readonly LangileArrunta _erabiltzailea;

        private ListBox  lstKatalogoa;
        private ListBox  lstEskaerako;
        private TextBox  txtBezeroIzena;
        private CheckBox chkEtxe;
        private TextBox  txtHelbidea;
        private Label    lblHelbidea;
        private Label    lblTotala;
        private Label    lblEgoera;
        private Button   btnGehitu;
        private Button   btnKendu;
        private Button   btnGorde;
        private Button   btnIkusi;

        private List<Pizza> _pizzaHautatuak = new List<Pizza>();

        public LangileForm(LangileArrunta erabiltzailea)
        {
            _erabiltzailea = erabiltzailea;
            Estiloak.FormEzarri(this,
                "EuskoPizza — Langile Arrunta · " + erabiltzailea.Izena, 760, 620);
            EraikiForms();
            PizzakKargatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "🧑 Eskaera Berria",
                Estiloak.Laranja, "Kaixo, " + _erabiltzailea.Izena + "! Sortu eskaera berria.");

            int Y = 90;

            this.Controls.Add(Estiloak.LabelSortu("Bezeroaren izena",
                20, Y, 160, 22, Estiloak.FontH3, Estiloak.TestuArgia));
            txtBezeroIzena = Estiloak.TextBoxSortu(20, Y + 25, 260);
            txtBezeroIzena.PlaceholderText = "Adib: Miren, Aitor...";
            this.Controls.Add(txtBezeroIzena);

            chkEtxe = new CheckBox();
            chkEtxe.Text      = "Etxez-etxe bidali";
            chkEtxe.Location  = new Point(300, Y + 27);
            chkEtxe.Size      = new Size(180, 25);
            chkEtxe.ForeColor = Estiloak.Testua;
            chkEtxe.Font      = Estiloak.FontNormal;
            chkEtxe.BackColor = Estiloak.Iluna;
            chkEtxe.CheckedChanged += new EventHandler(chkEtxe_CheckedChanged);
            this.Controls.Add(chkEtxe);

            lblHelbidea = Estiloak.LabelSortu("Helbidea",
                20, Y + 65, 160, 22, Estiloak.FontH3, Estiloak.TestuArgia);
            lblHelbidea.Visible = false;
            this.Controls.Add(lblHelbidea);

            txtHelbidea = Estiloak.TextBoxSortu(20, Y + 88, 450);
            txtHelbidea.PlaceholderText = "Adib: Gran Via 5, Bilbo";
            txtHelbidea.Visible = false;
            this.Controls.Add(txtHelbidea);

            this.Controls.Add(Estiloak.LerroaSortu(20, Y + 122, 710));

            int Y2 = Y + 135;
            this.Controls.Add(Estiloak.LabelSortu("Pizza katalogoa",
                20, Y2, 200, 22, Estiloak.FontH3, Estiloak.TestuArgia));
            lstKatalogoa = Estiloak.ListBoxSortu(20, Y2 + 25, 310, 195);
            lstKatalogoa.DoubleClick += new EventHandler(lstKatalogoa_DoubleClick);
            this.Controls.Add(lstKatalogoa);

            btnGehitu = Estiloak.BotoiNagusiaSortu("Gehitu »",
                345, Y2 + 80, 90, 36, Estiloak.Urdina);
            btnGehitu.Click += new EventHandler(btnGehitu_Click);
            this.Controls.Add(btnGehitu);

            btnKendu = Estiloak.BotoiBigarrenaSortu("« Kendu",
                345, Y2 + 125, 90, 32);
            btnKendu.Click += new EventHandler(btnKendu_Click);
            this.Controls.Add(btnKendu);

            this.Controls.Add(Estiloak.LabelSortu("Eskaerako pizzak",
                450, Y2, 200, 22, Estiloak.FontH3, Estiloak.TestuArgia));
            lstEskaerako = Estiloak.ListBoxSortu(450, Y2 + 25, 290, 195);
            this.Controls.Add(lstEskaerako);

            lblTotala = Estiloak.LabelSortu("Totala: 0.00€",
                450, Y2 + 228, 290, 26, Estiloak.FontH3, Estiloak.Berdea);
            lblTotala.TextAlign = ContentAlignment.MiddleRight;
            this.Controls.Add(lblTotala);

            this.Controls.Add(Estiloak.LerroaSortu(20, Y2 + 260, 710));

            int Ybot = Y2 + 275;
            btnGorde = Estiloak.BotoiNagusiaSortu("✔  Eskaera gorde",
                20, Ybot, 220, 46, Estiloak.Berdea);
            btnGorde.Click += btnGorde_Click;
            this.Controls.Add(btnGorde);

            btnIkusi = Estiloak.BotoiBigarrenaSortu("📋  Dendan prest",
                260, Ybot + 5, 200, 36);
            btnIkusi.Click += btnIkusi_Click;
            this.Controls.Add(btnIkusi);

            lblEgoera = Estiloak.LabelSortu("",
                20, Ybot + 55, 710, 22, Estiloak.FontTxiki, Estiloak.Berdea);
            lblEgoera.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblEgoera);
        }

        private void chkEtxe_CheckedChanged(object sender, EventArgs e)
        {
            lblHelbidea.Visible = chkEtxe.Checked;
            txtHelbidea.Visible = chkEtxe.Checked;
        }

        private void lstKatalogoa_DoubleClick(object sender, EventArgs e)
        {
            PizzaGehitu();
        }

        private void btnGehitu_Click(object sender, EventArgs e)
        {
            PizzaGehitu();
        }

        private void btnKendu_Click(object sender, EventArgs e)
        {
            PizzaKendu();
        }

        private void PizzakKargatu()
        {
            lstKatalogoa.Items.Clear();
            try
            {
                List<Pizza> pizzak = DatuBasea.PizzakLortu();
                for (int i = 0; i < pizzak.Count; i++)
                    lstKatalogoa.Items.Add(pizzak[i]);
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea pizzak kargatzean: " + ex.Message;
            }
        }

        private void PizzaGehitu()
        {
            if (lstKatalogoa.SelectedItem == null) return;

            Pizza pizza = (Pizza)lstKatalogoa.SelectedItem;
            _pizzaHautatuak.Add(pizza);           
            lstEskaerako.Items.Add(pizza);        
            TotalaEguneratu();
            lblEgoera.Text = "";
        }

        private void PizzaKendu()
        {
            int i = lstEskaerako.SelectedIndex;
            if (i < 0) return;

            _pizzaHautatuak.RemoveAt(i);           
            lstEskaerako.Items.RemoveAt(i);
            TotalaEguneratu();
        }

        private void TotalaEguneratu()
        {
            double totala = 0;
            for (int i = 0; i < _pizzaHautatuak.Count; i++)
                totala += _pizzaHautatuak[i].Prezioa;

            lblTotala.Text = "Totala: " + totala.ToString("F2") + "€";

            if (totala > 0)
                lblTotala.ForeColor = Estiloak.Berdea;
            else
                lblTotala.ForeColor = Estiloak.TestuArgia;
        }

        private void btnGorde_Click(object sender, EventArgs e)
        {
            lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);

            if (string.IsNullOrEmpty(txtBezeroIzena.Text.Trim()))
            { lblEgoera.Text = "⚠ Idatzi bezeroaren izena."; return; }

            if (_pizzaHautatuak.Count == 0)
            { lblEgoera.Text = "⚠ Gehitu gutxienez pizza bat."; return; }

            if (chkEtxe.Checked && string.IsNullOrEmpty(txtHelbidea.Text.Trim()))
            { lblEgoera.Text = "⚠ Etxez-etxe bada, helbidea idatzi."; return; }

            try
            {
                string helbidea = chkEtxe.Checked ? txtHelbidea.Text.Trim() : "";
                Eskaera eskaera = new Eskaera(0, txtBezeroIzena.Text.Trim(),
                                              chkEtxe.Checked, helbidea);
                for (int i = 0; i < _pizzaHautatuak.Count; i++)
                    eskaera.PizzaGehitu(_pizzaHautatuak[i]);

                int idBerria = DatuBasea.EskaeraGorde(eskaera); 

                txtBezeroIzena.Clear();
                txtHelbidea.Clear();
                chkEtxe.Checked = false;
                _pizzaHautatuak.Clear();
                lstEskaerako.Items.Clear();
                TotalaEguneratu();

                lblEgoera.ForeColor = Estiloak.Berdea;
                lblEgoera.Text = "✔  #" + idBerria + " eskaera gordeta! " +
                                 "(" + eskaera.PrezioTotala.ToString("F2") + "€) — Sukaldeko zerrendan dago.";
            }
            catch (Exception ex)
            {
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnIkusi_Click(object sender, EventArgs e)
        {
            try
            {
                List<Eskaera> eskaerак =
                    DatuBasea.EskaerakEgoeraren(EgoeraMota.BanatzekoZain);

                string mezua = "Dendan jasotzeko prest:\n\n";
                bool bat = false;
                for (int i = 0; i < eskaerак.Count; i++)
                {
                    if (!eskaerак[i].EtxekoEntrega)
                    {
                        mezua += "  " + eskaerак[i].ToString() + "\n";
                        bat = true;
                    }
                }
                if (!bat) mezua += "(Oraingoz ez dago eskaerarik prest)";

                MessageBox.Show(mezua, "Dendan jasotzeko",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB errorea: " + ex.Message, "Errorea",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
