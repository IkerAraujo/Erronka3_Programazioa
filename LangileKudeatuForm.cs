using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Admin-eko Langile CRUD pantaila.
    // Langileak ikusi, gehitu eta desaktibatu daitezke.
    public class LangileKudeatuForm : Form
    {
        private DataGridView gridLangileak;
        private Button       btnGehitu;
        private Button       btnEditatu;
        private Button       btnDesaktibatu;
        private Button       btnFreshatu;
        private Label        lblEgoera;

        public LangileKudeatuForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — Langile Kudeaketa", 780, 500);
            this.MaximizeBox = true;
            EraikiForms();
            LangileakKargatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "👥 Langile Kudeaketa",
                Estiloak.Morea, "Langileak ikusi, gehitu edo desaktibatu.");


            gridLangileak = new DataGridView();
            gridLangileak.Location            = new Point(15, 80);
            gridLangileak.Size                = new Size(745, 315);
            gridLangileak.BackgroundColor     = Estiloak.PanelIluna;
            gridLangileak.ForeColor           = Estiloak.Testua;
            gridLangileak.GridColor           = Estiloak.Muga;
            gridLangileak.Font                = Estiloak.FontTxiki;
            gridLangileak.BorderStyle         = BorderStyle.None;
            gridLangileak.RowHeadersVisible   = false;
            gridLangileak.AllowUserToAddRows  = false;
            gridLangileak.SelectionMode       = DataGridViewSelectionMode.FullRowSelect;
            gridLangileak.MultiSelect         = false;
            gridLangileak.ReadOnly            = true;
            gridLangileak.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridLangileak.DefaultCellStyle.BackColor          = Estiloak.PanelArgia;
            gridLangileak.DefaultCellStyle.ForeColor          = Estiloak.Testua;
            gridLangileak.DefaultCellStyle.SelectionBackColor = Estiloak.Morea;
            gridLangileak.DefaultCellStyle.SelectionForeColor = Color.White;
            gridLangileak.ColumnHeadersDefaultCellStyle.BackColor = Estiloak.PanelIluna;
            gridLangileak.ColumnHeadersDefaultCellStyle.ForeColor = Estiloak.TestuArgia;
            gridLangileak.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            gridLangileak.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(45, 45, 58);

            this.Controls.Add(gridLangileak);


            int Ybot = 410;
            btnGehitu = Estiloak.BotoiNagusiaSortu("➕  Langile berria",
                15, Ybot, 160, 40, Estiloak.Berdea);
            btnGehitu.Click += btnGehitu_Click;
            this.Controls.Add(btnGehitu);

            btnEditatu = Estiloak.BotoiNagusiaSortu("✏  Editatu",
                185, Ybot, 130, 40, Estiloak.Urdina);
            btnEditatu.Click += btnEditatu_Click;
            this.Controls.Add(btnEditatu);

            btnDesaktibatu = Estiloak.BotoiNagusiaSortu("🚫  Desaktibatu",
                325, Ybot, 155, 40, Color.FromArgb(180, 50, 50));
            btnDesaktibatu.Click += btnDesaktibatu_Click;
            this.Controls.Add(btnDesaktibatu);

            btnFreshatu = Estiloak.BotoiBigarrenaSortu("↺  Freskatu",
                490, Ybot, 120, 40);
            btnFreshatu.Click += new EventHandler(btnFreshatu_Click);
            this.Controls.Add(btnFreshatu);

            lblEgoera = Estiloak.LabelSortu("", 15, 457, 745, 22,
                Estiloak.FontTxiki, Estiloak.Berdea);
            lblEgoera.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblEgoera);
        }

        private void LangileakKargatu()
        {
            try
            {
                List<Erabiltzaile> langileak = DatuBasea.LangileakLortu();

                gridLangileak.Columns.Clear();
                gridLangileak.Rows.Clear();

                gridLangileak.Columns.Add("id",     "ID");
                gridLangileak.Columns.Add("izena",   "Izena");
                gridLangileak.Columns.Add("rola",    "Rola");
                gridLangileak.Columns.Add("aktibo",  "Egoera");

                gridLangileak.Columns["id"].Width    = 40;
                gridLangileak.Columns["aktibo"].Width = 80;

                for (int i = 0; i < langileak.Count; i++)
                {
                    Erabiltzaile l = langileak[i];

                    string egoeraBal;
                    if (l.Aktibo)
                        egoeraBal = "✔ Aktibo";
                    else
                        egoeraBal = "✗ Inaktibo";

                    int errenkada = gridLangileak.Rows.Add(
                        l.Id,
                        l.Izena,
                        l.RolaLortu(),
                        egoeraBal
                    );
                    if (!l.Aktibo)
                        gridLangileak.Rows[errenkada].DefaultCellStyle.ForeColor =
                            Color.FromArgb(100, 100, 100);
                }

                lblEgoera.Text = langileak.Count + " langile kargatu dira.";
                lblEgoera.ForeColor = Estiloak.TestuArgia;
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void btnFreshatu_Click(object sender, EventArgs e)
        {
            LangileakKargatu();
        }

        private void btnEditatu_Click(object sender, EventArgs e)
        {
            if (gridLangileak.SelectedRows.Count == 0)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Hautatu langile bat editatzeko.";
                return;
            }

            DataGridViewRow errenkada = gridLangileak.SelectedRows[0];
            int    id    = Convert.ToInt32(errenkada.Cells["id"].Value);
            string izena = errenkada.Cells["izena"].Value?.ToString() ?? "";
            string rola  = errenkada.Cells["rola"].Value?.ToString()  ?? "";

            using (LangileEditatuForm editatuForm = new LangileEditatuForm(id, izena, rola))
            {
                if (editatuForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DatuBasea.LangileEguneratu(
                            id,
                            editatuForm.Izena,
                            editatuForm.Abizena,
                            editatuForm.Nan,
                            editatuForm.Gmail,
                            editatuForm.RolId);
                        LangileakKargatu();
                        lblEgoera.ForeColor = Estiloak.Berdea;
                        lblEgoera.Text = "✔  '" + editatuForm.Izena + "' eguneratu da.";
                    }
                    catch (Exception ex)
                    {
                        lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                        lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
                    }
                }
            }
        }

        private void btnGehitu_Click(object sender, EventArgs e)
        {
            using (LangileGehituForm gehituForm = new LangileGehituForm())
            {
                if (gehituForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        DatuBasea.LangilaGehitu(
                            gehituForm.Izena, gehituForm.Abizena,
                            gehituForm.Nan, gehituForm.Gmail,
                            gehituForm.Pasahitza, gehituForm.RolId);
                        LangileakKargatu();
                        lblEgoera.ForeColor = Estiloak.Berdea;
                        lblEgoera.Text = "✔  '" + gehituForm.Izena + "' langilea gehitu da.";
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
            if (gridLangileak.SelectedRows.Count == 0)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Hautatu langile bat zerrendan.";
                return;
            }

            DataGridViewRow errenkada = gridLangileak.SelectedRows[0];
            int id = Convert.ToInt32(errenkada.Cells["id"].Value);

            string izena = "";
            if (errenkada.Cells["izena"].Value != null)
                izena = errenkada.Cells["izena"].Value.ToString();

            bool aktibo = false;
            if (errenkada.Cells["aktibo"].Value != null)
                aktibo = errenkada.Cells["aktibo"].Value.ToString().StartsWith("✔");

            if (!aktibo)
            {
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "Langile hau jadanik inaktibo dago.";
                return;
            }

            if (!EuskaraElkarrizketa.GaldeBaiEz(
                "'" + izena + "' langilea desaktibatu nahi duzu?\n" +
                "(Ez da aplikaziora sartu ahal izango)",
                "Berretsi desaktibatzea")) return;

            try
            {
                DatuBasea.LangileDesaktibatu(id);
                LangileakKargatu();
                lblEgoera.ForeColor = Estiloak.TestuArgia;
                lblEgoera.Text = "'" + izena + "' desaktibatu da.";
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }
    }

    // ── Langile Editatu elkarrizketa ──────────────────────────
    public class LangileEditatuForm : Form
    {
        public string Izena     { get; private set; }
        public string Abizena   { get; private set; }
        public string Nan       { get; private set; }
        public string Gmail     { get; private set; }
        public int    RolId     { get; private set; }

        private readonly int    _id;
        private TextBox  txtIzena, txtAbizena, txtNan, txtGmail;
        private ComboBox cmbRola;
        private Button   btnGorde, btnUtzi;

        public LangileEditatuForm(int id, string izena, string rola)
        {
            _id = id;
            Estiloak.FormEzarri(this, "Langilea editatu", 400, 380);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            EraikiForms(izena, rola);
        }

        private void EraikiForms(string izena, string rola)
        {
            int Y = 20;
            int ZAB = 320;

            this.Controls.Add(Estiloak.LabelSortu("✏ Langilea editatu",
                30, Y, ZAB, 28, Estiloak.FontH2, Estiloak.Testua));
            Y += 40;

            this.Controls.Add(Estiloak.LabelSortu("Izena",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtIzena = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            txtIzena.Text = izena;
            this.Controls.Add(txtIzena);
            Y += 50;

            this.Controls.Add(Estiloak.LabelSortu("Abizena",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtAbizena = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtAbizena);
            Y += 50;

            this.Controls.Add(Estiloak.LabelSortu("NAN",
                30, Y, 140, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtNan = Estiloak.TextBoxSortu(30, Y + 22, 140);
            this.Controls.Add(txtNan);
            Y += 50;

            this.Controls.Add(Estiloak.LabelSortu("Gmail",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtGmail = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtGmail);
            Y += 50;

            this.Controls.Add(Estiloak.LabelSortu("Rola",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            cmbRola = new ComboBox();
            cmbRola.Location      = new Point(30, Y + 22);
            cmbRola.Size          = new Size(ZAB, 32);
            cmbRola.BackColor     = Estiloak.PanelArgia;
            cmbRola.ForeColor     = Estiloak.Testua;
            cmbRola.Font          = Estiloak.FontNormal;
            cmbRola.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRola.Items.Add("Langile Arrunta (2)");
            cmbRola.Items.Add("Sukaldaria (3)");
            cmbRola.Items.Add("Banatzailea (4)");
            cmbRola.Items.Add("Admin (1)");

            // Uneko rola hautatu
            if      (rola == "Sukaldaria")  cmbRola.SelectedIndex = 1;
            else if (rola == "Banatzailea") cmbRola.SelectedIndex = 2;
            else if (rola == "Admin")       cmbRola.SelectedIndex = 3;
            else                            cmbRola.SelectedIndex = 0;

            this.Controls.Add(cmbRola);
            Y += 55;

            btnGorde = Estiloak.BotoiNagusiaSortu("✔  Gorde", 30, Y, 145, 38, Estiloak.Berdea);
            btnGorde.Click += btnGorde_Click;
            this.Controls.Add(btnGorde);

            btnUtzi = Estiloak.BotoiBigarrenaSortu("Utzi", 185, Y, 100, 38);
            btnUtzi.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; };
            this.Controls.Add(btnUtzi);

            this.AcceptButton = btnGorde;
        }

        private void btnGorde_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIzena.Text.Trim()) ||
                string.IsNullOrEmpty(txtNan.Text.Trim())   ||
                string.IsNullOrEmpty(txtGmail.Text.Trim()))
            {
                EuskaraElkarrizketa.Mezua("Bete itzazu eremu guztiak.", "Errorea", errorea: true);
                return;
            }

            Izena   = txtIzena.Text.Trim();
            Abizena = txtAbizena.Text.Trim();
            Nan     = txtNan.Text.Trim();
            Gmail   = txtGmail.Text.Trim();

            int[] rolIdak = { 2, 3, 4, 1 };
            RolId = rolIdak[cmbRola.SelectedIndex];

            this.DialogResult = DialogResult.OK;
        }
    }

    // ── Langile Gehitu elkarrizketa ───────────────────────────
    public class LangileGehituForm : Form
    {
        public string Izena     { get; private set; }
        public string Abizena   { get; private set; }
        public string Nan       { get; private set; }
        public string Gmail     { get; private set; }
        public string Pasahitza { get; private set; }
        public int    RolId     { get; private set; }

        private TextBox    txtIzena;
        private TextBox    txtAbizena;
        private TextBox    txtNan;
        private TextBox    txtGmail;
        private TextBox    txtPasahitza;
        private ComboBox   cmbRola;
        private Button     btnGorde;
        private Button     btnUtzi;

        public LangileGehituForm()
        {
            Estiloak.FormEzarri(this, "Langile berria gehitu", 400, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            EraikiForms();
        }

        private void EraikiForms()
        {
            int Y = 20;
            int ZAB = 320;

            this.Controls.Add(Estiloak.LabelSortu("👤 Langile berria",
                30, Y, ZAB, 28, Estiloak.FontH2, Estiloak.Testua));
            Y += 40;

            // Izena
            this.Controls.Add(Estiloak.LabelSortu("Izena",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtIzena = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtIzena);
            Y += 50;

            // Abizena
            this.Controls.Add(Estiloak.LabelSortu("Abizena",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtAbizena = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            this.Controls.Add(txtAbizena);
            Y += 50;

            // NAN
            this.Controls.Add(Estiloak.LabelSortu("NAN",
                30, Y, 140, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtNan = Estiloak.TextBoxSortu(30, Y + 22, 140);
            this.Controls.Add(txtNan);
            Y += 50;

            // Gmail
            this.Controls.Add(Estiloak.LabelSortu("Gmail",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtGmail = Estiloak.TextBoxSortu(30, Y + 22, ZAB);
            txtGmail.PlaceholderText = "adib: langilea@euskopizza.com";
            this.Controls.Add(txtGmail);
            Y += 50;

            // Pasahitza
            this.Controls.Add(Estiloak.LabelSortu("Pasahitza",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtPasahitza = Estiloak.TextBoxSortu(30, Y + 22, ZAB, pasahitza: true);
            this.Controls.Add(txtPasahitza);
            Y += 50;

            // Rola
            this.Controls.Add(Estiloak.LabelSortu("Rola",
                30, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));

            cmbRola = new ComboBox();
            cmbRola.Location      = new Point(30, Y + 22);
            cmbRola.Size          = new Size(ZAB, 32);
            cmbRola.BackColor     = Estiloak.PanelArgia;
            cmbRola.ForeColor     = Estiloak.Testua;
            cmbRola.Font          = Estiloak.FontNormal;
            cmbRola.DropDownStyle = ComboBoxStyle.DropDownList;

            // rolak taulan: 1=Admin, 2=Langile Arrunta, 3=Sukaldaria, 4=Banatzailea
            cmbRola.Items.Add("Langile Arrunta (2)");
            cmbRola.Items.Add("Sukaldaria (3)");
            cmbRola.Items.Add("Banatzailea (4)");
            cmbRola.Items.Add("Admin (1)");
            cmbRola.SelectedIndex = 0;
            this.Controls.Add(cmbRola);
            Y += 55;

            btnGorde = Estiloak.BotoiNagusiaSortu("✔  Gorde", 30, Y, 145, 38, Estiloak.Berdea);
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

        private void btnGorde_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIzena.Text.Trim())  ||
                string.IsNullOrEmpty(txtNan.Text.Trim())     ||
                string.IsNullOrEmpty(txtGmail.Text.Trim())   ||
                string.IsNullOrEmpty(txtPasahitza.Text))
            {
                EuskaraElkarrizketa.Mezua("Bete itzazu eremu guztiak.", "Errorea", errorea: true);
                return;
            }

            Izena     = txtIzena.Text.Trim();
            Abizena   = txtAbizena.Text.Trim();
            Nan       = txtNan.Text.Trim();
            Gmail     = txtGmail.Text.Trim();
            Pasahitza = txtPasahitza.Text;

            // Rol ID: ComboBox-etik atera
            int[] rolIdak = { 2, 3, 4, 1 };   // Langile Arrunta, Sukaldaria, Banatzailea, Admin
            RolId = rolIdak[cmbRola.SelectedIndex];

            this.DialogResult = DialogResult.OK;
        }
    }
}
