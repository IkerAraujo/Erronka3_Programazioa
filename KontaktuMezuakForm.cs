using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    public class KontaktuMezuakForm : Form
    {
        private DataGridView gridMezuak;
        private RichTextBox  rtbTestua;
        private Button       btnFreshatu;
        private Label        lblEgoera;

        public KontaktuMezuakForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — Kontaktu Mezuak", 800, 560);
            this.MaximizeBox = true;
            EraikiForms();
            MezuakKargatu();
        }

        private void EraikiForms()
        {
            Estiloak.TituluBandaSortu(this, "📬 Kontaktu Mezuak",
                Color.FromArgb(30, 100, 160), "Webgunetik bidalitako mezuak.");


            gridMezuak = new DataGridView();
            gridMezuak.Location            = new Point(15, 80);
            gridMezuak.Size                = new Size(760, 240);
            gridMezuak.BackgroundColor     = Estiloak.PanelIluna;
            gridMezuak.ForeColor           = Estiloak.Testua;
            gridMezuak.GridColor           = Estiloak.Muga;
            gridMezuak.Font                = Estiloak.FontTxiki;
            gridMezuak.BorderStyle         = BorderStyle.None;
            gridMezuak.RowHeadersVisible   = false;
            gridMezuak.AllowUserToAddRows  = false;
            gridMezuak.SelectionMode       = DataGridViewSelectionMode.FullRowSelect;
            gridMezuak.MultiSelect         = false;
            gridMezuak.ReadOnly            = true;
            gridMezuak.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridMezuak.DefaultCellStyle.BackColor          = Estiloak.PanelArgia;
            gridMezuak.DefaultCellStyle.ForeColor          = Estiloak.Testua;
            gridMezuak.DefaultCellStyle.SelectionBackColor = Color.FromArgb(30, 100, 160);
            gridMezuak.DefaultCellStyle.SelectionForeColor = Color.White;
            gridMezuak.ColumnHeadersDefaultCellStyle.BackColor = Estiloak.PanelIluna;
            gridMezuak.ColumnHeadersDefaultCellStyle.ForeColor = Estiloak.TestuArgia;
            gridMezuak.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);
            gridMezuak.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(45, 45, 58);

            gridMezuak.SelectionChanged += new EventHandler(grid_SelectionChanged);
            this.Controls.Add(gridMezuak);


            this.Controls.Add(Estiloak.LabelSortu("Mezuaren edukia",
                15, 330, 300, 20, Estiloak.FontTxiki, Estiloak.TestuArgia));
            rtbTestua = Estiloak.InfoPanelSortu(15, 352, 760, 120);
            rtbTestua.Text = "Hautatu mezu bat informazioa ikusteko...";
            this.Controls.Add(rtbTestua);


            btnFreshatu = Estiloak.BotoiBigarrenaSortu("↺  Freskatu", 15, 488, 130, 36);
            btnFreshatu.Click += (s, e) => MezuakKargatu();
            this.Controls.Add(btnFreshatu);

            lblEgoera = Estiloak.LabelSortu("", 160, 492, 620, 22,
                Estiloak.FontTxiki, Estiloak.TestuArgia);
            lblEgoera.TextAlign = ContentAlignment.MiddleLeft;
            this.Controls.Add(lblEgoera);
        }

        private void MezuakKargatu()
        {
            try
            {
                List<KontaktuMezua> mezuak = DatuBasea.KontaktuMezuakLortu();

                gridMezuak.Columns.Clear();
                gridMezuak.Rows.Clear();

                gridMezuak.Columns.Add("id",    "ID");
                gridMezuak.Columns.Add("data",  "Data");
                gridMezuak.Columns.Add("izena", "Izena");
                gridMezuak.Columns.Add("gmail", "Gmail");
                gridMezuak.Columns.Add("gaia",  "Gaia");

                gridMezuak.Columns["id"].Width   = 45;
                gridMezuak.Columns["data"].Width = 130;

                for (int i = 0; i < mezuak.Count; i++)
                {
                    KontaktuMezua m = mezuak[i];
                    gridMezuak.Rows.Add(
                        m.Id,
                        m.Data.ToString("yyyy-MM-dd HH:mm"),
                        m.Izena,
                        m.Gmail,
                        m.Gaia
                    );
                }

                lblEgoera.Text = mezuak.Count + " mezu.";
                rtbTestua.Text = "Hautatu mezu bat informazioa ikusteko...";
            }
            catch (Exception ex)
            {
                lblEgoera.ForeColor = Color.FromArgb(255, 100, 100);
                lblEgoera.Text = "⚠ DB errorea: " + ex.Message;
            }
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if (gridMezuak.SelectedRows.Count == 0) return;

            DataGridViewRow row = gridMezuak.SelectedRows[0];
            string izena = row.Cells["izena"].Value?.ToString() ?? "";
            string gmail = row.Cells["gmail"].Value?.ToString() ?? "";
            string gaia  = row.Cells["gaia"].Value?.ToString()  ?? "";
            string data  = row.Cells["data"].Value?.ToString()  ?? "";

            // Testua lortu — gridean ez dago, beraz DB-tik berriz lortu
            int id = Convert.ToInt32(row.Cells["id"].Value);
            try
            {
                List<KontaktuMezua> mezuak = DatuBasea.KontaktuMezuakLortu();
                KontaktuMezua aurkitua = null;
                for (int i = 0; i < mezuak.Count; i++)
                    if (mezuak[i].Id == id) { aurkitua = mezuak[i]; break; }

                if (aurkitua != null)
                    rtbTestua.Text = $"Nork: {izena} <{gmail}>\nData: {data}\nGaia: {gaia}\n\n{aurkitua.Testua}";
            }
            catch { }
        }
    }
}
