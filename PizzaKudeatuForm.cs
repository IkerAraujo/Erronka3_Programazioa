using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Admin-eko pizza kudeaketa pantaila
    public class PizzaKudeatuForm : Form
    {
        private DataGridView gridPizzak;
        private Button       btnGehitu;
        private Button       btnEditatu;
        private Button       btnDesaktibatu;
        private Button       btnFreshatu;
        private Label        lblEgoera;

        public PizzaKudeatuForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — Pizza Kudeaketa", 820, 560);
            this.MaximizeBox = true;
            EraikiForms();
            PizzakKargatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "🍕 Pizza Kudeaketa",
                Estiloak.Gorria, "Pizzak gehitu, editatu edo desaktibatu.");


            gridPizzak = new DataGridView();
            gridPizzak.Location            = new Point(15, 80);
            gridPizzak.Size                = new Size(775, 360);
            gridPizzak.BackgroundColor     = Estiloak.PanelIluna;
            gridPizzak.ForeColor           = Estiloak.Testua;
            gridPizzak.GridColor           = Estiloak.Muga;
            gridPizzak.Font                = Estiloak.FontTxiki;
            gridPizzak.BorderStyle         = BorderStyle.None;
            gridPizzak.RowHeadersVisible   = false;
            gridPizzak.AllowUserToAddRows  = false;
            gridPizzak.SelectionMode       = DataGridViewSelectionMode.FullRowSelect;
            gridPizzak.MultiSelect         = false;
            gridPizzak.ReadOnly            = true;
            gridPizzak.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Kolore estiloa
            gridPizzak.DefaultCellStyle.BackColor          = Estiloak.PanelArgia;
            gridPizzak.DefaultCellStyle.ForeColor          = Estiloak.Testua;
            gridPizzak.DefaultCellStyle.SelectionBackColor = Estiloak.Gorria;
            gridPizzak.DefaultCellStyle.SelectionForeColor = Color.White;
            gridPizzak.ColumnHeadersDefaultCellStyle.BackColor = Estiloak.PanelIluna;
            gridPizzak.ColumnHeadersDefaultCellStyle.ForeColor = Estiloak.TestuArgia;
            gridPizzak.ColumnHeadersDefaultCellStyle.Font      =
                new Font("Segoe UI", 9, FontStyle.Bold);
            gridPizzak.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(45, 45, 58);

            this.Controls.Add(gridPizzak);


            int Ybot = 455;
            btnGehitu = Estiloak.BotoiNagusiaSortu("➕  Gehitu", 15, Ybot, 140, 40, Estiloak.Berdea);
            btnGehitu.Click += btnGehitu_Click;
            this.Controls.Add(btnGehitu);

            btnEditatu = Estiloak.BotoiNagusiaSortu("✏  Editatu", 165, Ybot, 140, 40, Estiloak.Urdina);
            btnEditatu.Click += btnEditatu_Click;
            this.Controls.Add(btnEditatu);

            btnDesaktibatu = Estiloak.BotoiNagusiaSortu("🚫  Desaktibatu",
                315, Ybot, 160, 40, Color.FromArgb(180, 50, 50));
            btnDesaktibatu.Click += btnDesaktibatu_Click;
            this.Controls.Add(btnDesaktibatu);

            btnFreshatu = Estiloak.BotoiBigarrenaSortu("↺  Freskatu", 490, Ybot, 130, 40);
            btnFreshatu.Click += new EventHandler(btnFreshatu_Click);
            this.Controls.Add(btnFreshatu);

            lblEgoera = Estiloak.LabelSortu("", 15, 502, 775, 22,
                Estiloak.FontTxiki, Estiloak.Berdea);
            lblEgoera.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblEgoera);
        }

        private void PizzakKargatu()
        {
            try
            {
                List<Pizza> pizzak = DatuBasea.PizzakDenakLortu();

                gridPizzak.Columns.Clear();
                gridPizzak.Rows.Clear();


                gridPizzak.Columns.Add("id",           "ID");
                gridPizzak.Columns.Add("izena",         "Izena");
                gridPizzak.Columns.Add("mota",          "Mota");
                gridPizzak.Columns.Add("prezioa",        "Prezioa");
                gridPizzak.Columns.Add("ingredienteak",  "Ingredienteak");
                gridPizzak.Columns.Add("eskuragarri",    "Eskuragarri");

                gridPizzak.Columns["id"].Width       = 40;
                gridPizzak.Columns["prezioa"].Width  = 70;
                gridPizzak.Columns["eskuragarri"].Width = 80;

                // Datuak gehitu
                foreach (Pizza p in pizzak)
                {
                    string eskBal;
                    if (p.Eskuragarri)
                        eskBal = "✔ Bai";
                    else
                        eskBal = "✗ Ez";

                    int errenkada = gridPizzak.Rows.Add(
                        p.Id,
                        p.Izena,
                        p.Mota,
                        p.Prezioa.ToString("F2") + "€",
                        p.Ingredienteak,
                        eskBal
                    );

                    // Inaktiboak grisa
                    if (!p.Eskuragarri)
                        gridPizzak.Rows[errenkada].DefaultCellStyle.ForeColor =
                            Color.FromArgb(100, 100, 100);
                }

                lblEgoera.Text = pizzak.Count + " pizza kargatu dira.";
                lblEgoera.ForeColor = Estiloak.TestuArgia;
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnGehitu_Click(object sender, EventArgs e)
        {
            using (PizzaEditatuForm editatuForm = new PizzaEditatuForm(null))
            {
                if (editatuForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DatuBasea.PizzaGehitu(editatuForm.PizzaBerria);
                        PizzakKargatu();
                        lblEgoera.ForeColor = Estiloak.Berdea;
                        lblEgoera.Text = "✔  '" + editatuForm.PizzaBerria.Izena + "' pizza gehitu da.";
                    }
                    catch (Exception ex)
                    {
                        lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                        lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
                    }
                }
            }
        }

        private void btnEditatu_Click(object sender, EventArgs e)
        {
            Pizza pizza = HautatutakoPizzaLortu();
            if (pizza == null) return;

            using (PizzaEditatuForm editatuForm = new PizzaEditatuForm(pizza))
            {
                if (editatuForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DatuBasea.PizzaEguneratu(editatuForm.PizzaBerria);
                        PizzakKargatu();
                        lblEgoera.ForeColor = Estiloak.Berdea;
                        lblEgoera.Text = "✔  '" + editatuForm.PizzaBerria.Izena + "' pizza eguneratu da.";
                    }
                    catch (Exception ex)
                    {
                        lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                        lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
                    }
                }
            }
        }

        private void btnDesaktibatu_Click(object sender, EventArgs e)
        {
            Pizza pizza = HautatutakoPizzaLortu();
            if (pizza == null) return;

            if (!pizza.Eskuragarri)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Pizza hau jadanik inaktibo dago.";
                return;
            }

            if (!EuskaraElkarrizketa.GaldeBaiEz(
                "'" + pizza.Izena + "' pizza desaktibatu nahi duzu?\n" +
                "(Katalogoan ez da agertuko, baina historia gordeta egongo da)",
                "Berretsi desaktibatzea")) return;

            try
            {
                DatuBasea.PizzaDesaktibatu(pizza.Id);
                PizzakKargatu();
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "'" + pizza.Izena + "' desaktibatu da.";
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnFreshatu_Click(object sender, EventArgs e)
        {
            PizzakKargatu();
        }

        // Hautatutako errenkadaren Pizza objektua lortu
        private Pizza HautatutakoPizzaLortu()
        {
            if (gridPizzak.SelectedRows.Count == 0)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Hautatu pizza bat zerrendan.";
                return null;
            }

            DataGridViewRow errenkada = gridPizzak.SelectedRows[0];
            int id = Convert.ToInt32(errenkada.Cells["id"].Value);

            string izena = "";
            if (errenkada.Cells["izena"].Value != null)
                izena = errenkada.Cells["izena"].Value.ToString();

            string mota = "";
            if (errenkada.Cells["mota"].Value != null)
                mota = errenkada.Cells["mota"].Value.ToString();

            string prStr = "0";
            if (errenkada.Cells["prezioa"].Value != null)
                prStr = errenkada.Cells["prezioa"].Value.ToString().Replace("€", "");

            double prezioa = 0;
            double.TryParse(prStr,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out prezioa);

            string osag = "";
            if (errenkada.Cells["ingredienteak"].Value != null)
                osag = errenkada.Cells["ingredienteak"].Value.ToString();

            bool eskuragarri = false;
            if (errenkada.Cells["eskuragarri"].Value != null)
                eskuragarri = errenkada.Cells["eskuragarri"].Value.ToString().StartsWith("✔");

            return new Pizza(id, izena, mota, prezioa, osag, eskuragarri);
        }
    }

    // ── Pizza Gehitu / Editatu elkarrizketa ───────────────────
    // Dialog txiki bat pizza baten datuak editatzeko.
    public class PizzaEditatuForm : Form
    {
        public Pizza PizzaBerria { get; private set; }

        private TextBox  txtIzena;
        private TextBox  txtMota;
        private TextBox  txtPrezioa;
        private TextBox  txtIngredienteak;
        private CheckBox chkEskuragarri;
        private Button   btnGorde;
        private Button   btnUtzi;

        private readonly Pizza _jatorrizkoa;   // null = pizza berria

        public PizzaEditatuForm(Pizza pizza)
        {
            _jatorrizkoa = pizza;
            bool berria  = pizza == null;

            Estiloak.FormEzarri(this,
                berria ? "Pizza berria gehitu" : "Pizza editatu",
                400, 360);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            EraikiForms(berria);

            if (!berria) DatuakBetetzatu(pizza);
        }

        private void EraikiForms(bool berria)
        {
            int Y = 20;
            int ZAB = 310;

            Label lblTit = Estiloak.LabelSortu(
                berria ? "🍕 Pizza berria" : "✏ Pizza editatu",
                30, Y, ZAB, 28, Estiloak.FontH2, Estiloak.Testua);
            this.Controls.Add(lblTit);
            Y += 40;

            this.Controls.Add(Estiloak.LabelSortu("Izena",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtIzena = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtIzena);
            Y += 55;

            this.Controls.Add(Estiloak.LabelSortu("Mota (Klasikoa, Berezi, Euskal...)",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtMota = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtMota);
            Y += 55;

            this.Controls.Add(Estiloak.LabelSortu("Prezioa (€)",
                30, Y, 140, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtPrezioa = Estiloak.TextBoxSortu(30, Y + 22, 140);
            this.Controls.Add(txtPrezioa);
            Y += 55;

            this.Controls.Add(Estiloak.LabelSortu("Ingredienteak",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtIngredienteak = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtIngredienteak);
            Y += 55;

            chkEskuragarri = new CheckBox();
            chkEskuragarri.Text      = "Katalogoan eskuragarri";
            chkEskuragarri.Location  = new Point(30, Y);
            chkEskuragarri.Size      = new Size(220, 22);
            chkEskuragarri.ForeColor = Estiloak.Testua;
            chkEskuragarri.BackColor = Estiloak.Iluna;
            chkEskuragarri.Checked   = true;
            this.Controls.Add(chkEskuragarri);
            Y += 40;

            btnGorde = Estiloak.BotoiNagusiaSortu("✔  Gorde", 30, Y, 140, 38, Estiloak.Berdea);
            btnGorde.Click += btnGorde_Click;
            this.Controls.Add(btnGorde);

            btnUtzi = Estiloak.BotoiBigarrenaSortu("Utzi", 185, Y, 100, 38);
            btnUtzi.Click += new EventHandler(btnUtzi_Click);
            this.Controls.Add(btnUtzi);

            this.AcceptButton = btnGorde;
        }

        private void btnUtzi_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void DatuakBetetzatu(Pizza pizza)
        {
            txtIzena.Text        = pizza.Izena;
            txtMota.Text         = pizza.Mota;
            txtPrezioa.Text      = pizza.Prezioa.ToString("F2",
                System.Globalization.CultureInfo.InvariantCulture);
            txtIngredienteak.Text = pizza.Ingredienteak;
            chkEskuragarri.Checked = pizza.Eskuragarri;
        }

        private void btnGorde_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIzena.Text.Trim()))
            { EuskaraElkarrizketa.Mezua("Izena beharrezkoa da.", "Errorea", errorea: true); return; }

            if (!double.TryParse(txtPrezioa.Text.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double prezioa) || prezioa <= 0)
            { EuskaraElkarrizketa.Mezua("Prezio baliozkoa idatzi (adib: 9.50).", "Errorea", errorea: true); return; }

            int idBerria = 0;
            if (_jatorrizkoa != null)
                idBerria = _jatorrizkoa.Id;
            PizzaBerria = new Pizza(
                idBerria,
                txtIzena.Text.Trim(),
                txtMota.Text.Trim(),
                prezioa,
                txtIngredienteak.Text.Trim(),
                chkEskuragarri.Checked
            );

            this.DialogResult = DialogResult.OK;
        }
    }
}
