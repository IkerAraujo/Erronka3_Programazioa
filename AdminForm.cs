using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Pizzeria
{
    // Admin-eko pantaila nagusia.
    // Ezkerrean nabigazio barra, eskuinean hautaturiko pantaila.
    public class AdminForm : Form
    {
        private readonly Admin _erabiltzailea;

        // Txertatutako formak
        private LangileForm    _langileForm;
        private SukaldeForm    _sukaldeForm;
        private BanatzaileForm _banatzaileForm;

        // UI elementuak
        private Panel  pNavBar;        // Ezkerreko nabigazio barra
        private Panel  pEdukia;        // Eskuineko eduki area
        private Label  lblEskaeraKop;
        private System.Windows.Forms.Timer _timer;

        // Nabigazio botoiak
        private Button btnNav1;
        private Button btnNav2;
        private Button btnNav3;
        private Button btnNav4;
        private Button btnNav5;
        private Button btnNav6;
        private Button btnNav7;
        private Button btnAktibo;  // Une honetan aktibatuta dagoen botoia

        // Edukia gordetzeko panelak
        private Panel pEdukiLangile;
        private Panel pEdukiSukalde;
        private Panel pEdukiBanatzaile;
        private Panel pEdukiPizza;
        private Panel pEdukiLangileKud;
        private Panel pEdukiPdf;
        private Panel pEdukiKontaktu;

        public AdminForm(Admin erabiltzailea)
        {
            _erabiltzailea = erabiltzailea;
            Estiloak.FormEzarri(this,
                "EuskoPizza — Admin · " + erabiltzailea.Izena, 1100, 700);
            this.MaximizeBox = true;
            EraikiForms();
            TimerHasieratu();
        }

        private void EraikiForms()
        {
            // Goiburua (top banda)
            Panel banda = new Panel();
            banda.Dock      = DockStyle.Top;
            banda.Height    = 50;
            banda.BackColor = Color.FromArgb(80, 40, 120);

            Label lblTit = new Label();
            lblTit.Text      = "⚙  Admin — " + _erabiltzailea.Izena;
            lblTit.Font      = Estiloak.FontH2;
            lblTit.ForeColor = Color.White;
            lblTit.Location  = new Point(15, 13);
            lblTit.AutoSize  = true;
            lblTit.BackColor = Color.Transparent;
            banda.Controls.Add(lblTit);

            lblEskaeraKop = new Label();
            lblEskaeraKop.Font      = Estiloak.FontH3;
            lblEskaeraKop.ForeColor = Color.FromArgb(200, 220, 255);
            lblEskaeraKop.Location  = new Point(600, 15);
            lblEskaeraKop.Size      = new Size(220, 22);
            lblEskaeraKop.BackColor = Color.Transparent;
            banda.Controls.Add(lblEskaeraKop);

            Button btnRefresh = Estiloak.BotoiBigarrenaSortu("↺  Freskatu", 835, 9, 130, 32);
            btnRefresh.BackColor = Color.FromArgb(100, 60, 150);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(130, 90, 180);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            banda.Controls.Add(btnRefresh);

            Button btnItxiSaioa = Estiloak.BotoiBigarrenaSortu("🔓  Itxi saioa", 975, 9, 130, 32);
            btnItxiSaioa.BackColor = Color.FromArgb(100, 40, 40);
            btnItxiSaioa.ForeColor = Color.FromArgb(255, 150, 150);
            btnItxiSaioa.FlatAppearance.BorderColor = Color.FromArgb(150, 60, 60);
            btnItxiSaioa.Click += (s, e) =>
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
            };
            banda.Controls.Add(btnItxiSaioa);

            // Ezkerreko nabigazio barra 
            pNavBar = new Panel();
            pNavBar.Dock      = DockStyle.Left;
            pNavBar.Width     = 175;
            pNavBar.BackColor = Color.FromArgb(28, 28, 38);

            // Logo/izena nabigazio barran
            Label lblLogo = new Label();
            lblLogo.Text      = "🍕";
            lblLogo.Font      = new Font("Segoe UI Emoji", 24);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location  = new Point(0, 15);
            lblLogo.Size      = new Size(175, 45);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            lblLogo.BackColor = Color.Transparent;
            pNavBar.Controls.Add(lblLogo);

            Label lblAppIzena = new Label();
            lblAppIzena.Text      = "EuskoPizza";
            lblAppIzena.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            lblAppIzena.ForeColor = Color.FromArgb(180, 150, 230);
            lblAppIzena.Location  = new Point(0, 60);
            lblAppIzena.Size      = new Size(175, 22);
            lblAppIzena.TextAlign = ContentAlignment.MiddleCenter;
            lblAppIzena.BackColor = Color.Transparent;
            pNavBar.Controls.Add(lblAppIzena);

            // Lerro banatzailea
            Panel lerroaBanatzaile = new Panel();
            lerroaBanatzaile.Location  = new Point(10, 90);
            lerroaBanatzaile.Size      = new Size(155, 1);
            lerroaBanatzaile.BackColor = Color.FromArgb(60, 60, 80);
            pNavBar.Controls.Add(lerroaBanatzaile);

            // Nabigazio botoiak
            int navY = 105;
            btnNav1 = NavBotoiaSortu("🧑  Eskaera berriak", navY);  navY += 50;
            btnNav2 = NavBotoiaSortu("👨‍🍳  Sukaldea",        navY);  navY += 50;
            btnNav3 = NavBotoiaSortu("🛵  Banatzailea",     navY);  navY += 50;
            btnNav4 = NavBotoiaSortu("🍕  Pizza kudeaketa", navY);  navY += 50;
            btnNav5 = NavBotoiaSortu("👥  Langileak",       navY);  navY += 50;
            btnNav6 = NavBotoiaSortu("📄  PDF txostena",    navY);  navY += 50;
            btnNav7 = NavBotoiaSortu("📬  Mezuak",          navY);

            btnNav1.Click += new EventHandler(btnNav1_Click);
            btnNav2.Click += new EventHandler(btnNav2_Click);
            btnNav3.Click += new EventHandler(btnNav3_Click);
            btnNav4.Click += new EventHandler(btnNav4_Click);
            btnNav5.Click += new EventHandler(btnNav5_Click);
            btnNav6.Click += new EventHandler(btnNav6_Click);
            btnNav7.Click += new EventHandler(btnNav7_Click);

            pNavBar.Controls.Add(btnNav1);
            pNavBar.Controls.Add(btnNav2);
            pNavBar.Controls.Add(btnNav3);
            pNavBar.Controls.Add(btnNav4);
            pNavBar.Controls.Add(btnNav5);
            pNavBar.Controls.Add(btnNav6);
            pNavBar.Controls.Add(btnNav7);

            // Eskuineko eduki area + sidebar elkarrekin panel batean 
            Panel mainPanel = new Panel();
            mainPanel.Dock      = DockStyle.Fill;
            mainPanel.BackColor = Estiloak.Iluna;

            pEdukia = new Panel();
            pEdukia.Dock      = DockStyle.Fill;
            pEdukia.BackColor = Estiloak.Iluna;

            // WinForms-ek Fill atzera puskatzen du Left gehitzen denean
            mainPanel.Controls.Add(pEdukia);
            mainPanel.Controls.Add(pNavBar);

            // GARRANTZITSUA: mainPanel (Fill) lehenik, banda (Top) geroago
            this.Controls.Add(mainPanel);
            this.Controls.Add(banda);

            // Eduki panelak sortu eta txertatu 
            EdukiPanelakSortu();

            // Lehenengo pantaila erakutsi (Eskaera berriak)
            NavAktibatu(btnNav1, pEdukiLangile);
        }

        //  Nabigazio botoi baten estiloa sortu
        private Button NavBotoiaSortu(string testua, int y)
        {
            Button btn = new Button();
            btn.Text      = testua;
            btn.Location  = new Point(0, y);
            btn.Size      = new Size(175, 44);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize  = 0;
            btn.BackColor = Color.FromArgb(28, 28, 38);
            btn.ForeColor = Color.FromArgb(180, 180, 200);
            btn.Font      = new Font("Segoe UI", 9);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding   = new Padding(15, 0, 0, 0);
            btn.Cursor    = Cursors.Hand;
            return btn;
        }

        // Eduki panelak sortu
        private void EdukiPanelakSortu()
        {


            // Panel 1 — Langile forma
            pEdukiLangile = new Panel();
            pEdukiLangile.Dock = DockStyle.Fill;
            _langileForm = new LangileForm(
                new LangileArrunta(_erabiltzailea.Id, _erabiltzailea.Izena, "x", "x"));
            TxertatuForma(_langileForm, pEdukiLangile);
            pEdukia.Controls.Add(pEdukiLangile);

            // Panel 2 — Sukalde forma
            pEdukiSukalde = new Panel();
            pEdukiSukalde.Dock = DockStyle.Fill;
            _sukaldeForm = new SukaldeForm(
                new Sukaldaria(_erabiltzailea.Id, _erabiltzailea.Izena, "x", "x"));
            TxertatuForma(_sukaldeForm, pEdukiSukalde);
            pEdukia.Controls.Add(pEdukiSukalde);

            // Panel 3 — Banatzaile forma
            pEdukiBanatzaile = new Panel();
            pEdukiBanatzaile.Dock = DockStyle.Fill;
            _banatzaileForm = new BanatzaileForm(
                new Banatzailea(_erabiltzailea.Id, _erabiltzailea.Izena, "x", "x"));
            TxertatuForma(_banatzaileForm, pEdukiBanatzaile);
            pEdukia.Controls.Add(pEdukiBanatzaile);

            // Panel 4 — Pizza CRUD
            pEdukiPizza = new Panel();
            pEdukiPizza.Dock = DockStyle.Fill;
            PizzaKudeatuForm pizzaForm = new PizzaKudeatuForm();
            TxertatuForma(pizzaForm, pEdukiPizza);
            pEdukia.Controls.Add(pEdukiPizza);

            // Panel 5 — Langile CRUD
            pEdukiLangileKud = new Panel();
            pEdukiLangileKud.Dock = DockStyle.Fill;
            LangileKudeatuForm langileKudForm = new LangileKudeatuForm();
            TxertatuForma(langileKudForm, pEdukiLangileKud);
            pEdukia.Controls.Add(pEdukiLangileKud);

            // Panel 6 — PDF txostena
            pEdukiPdf = new Panel();
            pEdukiPdf.Dock = DockStyle.Fill;
            pEdukiPdf.Controls.Add(PdfPanelSortu());
            pEdukia.Controls.Add(pEdukiPdf);

            // Panel 7 — Kontaktu mezuak
            pEdukiKontaktu = new Panel();
            pEdukiKontaktu.Dock = DockStyle.Fill;
            KontaktuMezuakForm kontaktuForm = new KontaktuMezuakForm();
            TxertatuForma(kontaktuForm, pEdukiKontaktu);
            pEdukia.Controls.Add(pEdukiKontaktu);
        }

        //  Forma bat panel batean txertatu 
        private static void TxertatuForma(Form forma, Panel panel)
        {
            forma.AutoScroll      = false;
            forma.TopLevel        = false;
            forma.FormBorderStyle = FormBorderStyle.None;
            forma.Dock            = DockStyle.Fill;
            panel.Controls.Add(forma);
            forma.Show();


            for (int i = 0; i < forma.Controls.Count; i++)
            {
                if (forma.Controls[i].Dock == DockStyle.Top)
                {
                    forma.Controls[i].BackColor = Estiloak.Iluna;
                    for (int j = 0; j < forma.Controls[i].Controls.Count; j++)
                    {
                        forma.Controls[i].Controls[j].Visible = false;
                    }
                    break;
                }
            }
        }

        //  Nabigazio botoia aktibatu eta panela erakutsi 
        private void NavAktibatu(Button botoia, Panel panela)
        {
            // Botoi guztiak berrezarri
            DesaktibatzeNavBotoiak();

            // Botoi aktibo estiloa
            botoia.BackColor = Color.FromArgb(60, 40, 100);
            botoia.ForeColor = Color.White;
            botoia.Font      = new Font("Segoe UI", 9, FontStyle.Bold);

            // Panel guztiak ezkutatu
            pEdukiLangile.Visible    = false;
            pEdukiSukalde.Visible    = false;
            pEdukiBanatzaile.Visible = false;
            pEdukiPizza.Visible      = false;
            pEdukiLangileKud.Visible = false;
            pEdukiPdf.Visible        = false;

            // Hautaturiko panela erakutsi
            panela.Visible = true;
            panela.BringToFront();

            btnAktibo = botoia;
        }

        private void DesaktibatzeNavBotoiak()
        {
            Button[] botoiak = new Button[] {
                btnNav1, btnNav2, btnNav3, btnNav4, btnNav5, btnNav6
            };
            for (int i = 0; i < botoiak.Length; i++)
            {
                botoiak[i].BackColor = Color.FromArgb(28, 28, 38);
                botoiak[i].ForeColor = Color.FromArgb(180, 180, 200);
                botoiak[i].Font      = new Font("Segoe UI", 9);
            }
        }

        // Nabigazio botoi klikak
        private void btnNav1_Click(object sender, EventArgs e)
        {
            NavAktibatu(btnNav1, pEdukiLangile);
        }

        private void btnNav2_Click(object sender, EventArgs e)
        {
            _sukaldeForm.EskaerаkFreshatu();
            NavAktibatu(btnNav2, pEdukiSukalde);
        }

        private void btnNav3_Click(object sender, EventArgs e)
        {
            _banatzaileForm.EskaerаkFreshatu();
            NavAktibatu(btnNav3, pEdukiBanatzaile);
        }

        private void btnNav4_Click(object sender, EventArgs e)
        {
            NavAktibatu(btnNav4, pEdukiPizza);
        }

        private void btnNav5_Click(object sender, EventArgs e)
        {
            NavAktibatu(btnNav5, pEdukiLangileKud);
        }

        private void btnNav6_Click(object sender, EventArgs e)
        {
            NavAktibatu(btnNav6, pEdukiPdf);
        }

        private void btnNav7_Click(object sender, EventArgs e)
        {
            NavAktibatu(btnNav7, pEdukiKontaktu);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FreskatuDena();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            FreskatuDena();
        }

        // PDF panela
        private Panel PdfPanelSortu()
        {
            Panel p = new Panel();
            p.Dock      = DockStyle.Fill;
            p.BackColor = Estiloak.Iluna;

            Panel goiburua = new Panel();
            goiburua.Dock      = DockStyle.Top;
            goiburua.Height    = 65;
            goiburua.BackColor = Color.FromArgb(30, 120, 80);

            Label lblTit = new Label();
            lblTit.Text      = "📄  PDF Txostena Sortu";
            lblTit.Font      = Estiloak.FontH2;
            lblTit.ForeColor = Color.White;
            lblTit.Location  = new Point(20, 20);
            lblTit.AutoSize  = true;
            lblTit.BackColor = Color.Transparent;
            goiburua.Controls.Add(lblTit);
            p.Controls.Add(goiburua);

            Label lblAzalpen = Estiloak.LabelSortu(
                "Hautatu data bat eta eguneko eskaera entregatuen txostena sortu.",
                30, 90, 600, 22, Estiloak.FontNormal, Estiloak.TestuArgia);
            p.Controls.Add(lblAzalpen);

            Label lblData = Estiloak.LabelSortu("Data:",
                30, 130, 80, 22, Estiloak.FontH3, Estiloak.TestuArgia);
            p.Controls.Add(lblData);

            DateTimePicker dtp = new DateTimePicker();
            dtp.Location = new Point(115, 128);
            dtp.Size     = new Size(180, 30);
            dtp.Format   = DateTimePickerFormat.Short;
            dtp.Value    = DateTime.Today;
            dtp.Font     = Estiloak.FontNormal;
            p.Controls.Add(dtp);

            Button btnSortu = Estiloak.BotoiNagusiaSortu(
                "📋  Txostena sortu", 30, 175, 220, 44, Estiloak.Urdina);
            p.Controls.Add(btnSortu);

            Label lblEg = Estiloak.LabelSortu("",
                30, 232, 700, 22, Estiloak.FontTxiki, Estiloak.Berdea);
            p.Controls.Add(lblEg);

            Label lblOharra = Estiloak.LabelSortu(
                "PDFak Mahaigainean (Desktop) gordetzen dira.",
                30, 270, 500, 20, Estiloak.FontTxiki, Estiloak.TestuArgia);
            p.Controls.Add(lblOharra);

            btnSortu.Tag = new object[] { dtp, lblEg };
            btnSortu.Click += new EventHandler(btnSortu_Click);

            return p;
        }

        private void btnSortu_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            object[] datuak = (object[])btn.Tag;
            DateTimePicker dtp = (DateTimePicker)datuak[0];
            Label lblEg = (Label)datuak[1];

            lblEg.ForeColor = Estiloak.TestuArgia;
            lblEg.Text = "Sortzen...";
            try
            {
                string bidea = PdfSortzailea.EgunTxostenaSortu(dtp.Value);
                lblEg.ForeColor = Estiloak.Berdea;
                lblEg.Text = "✔  PDF sortuta: " + Path.GetFileName(bidea);

                if (EuskaraElkarrizketa.GaldeBaiEz(
                    "PDF prest!\n" + bidea + "\n\nIreki nahi duzu?",
                    "PDF sortuta"))
                {
                    System.Diagnostics.ProcessStartInfo psi =
                        new System.Diagnostics.ProcessStartInfo(bidea);
                    psi.UseShellExecute = true;
                    System.Diagnostics.Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                lblEg.ForeColor = Color.FromArgb(255, 100, 100);
                string mezuOsoa = ex.Message;
                if (ex.InnerException != null)
                    mezuOsoa += "\nBarrukoa: " + ex.InnerException.Message;
                EuskaraElkarrizketa.Mezua(mezuOsoa, "PDF Errorea", errorea: true);
                lblEg.Text = "⚠ Errorea: " + ex.Message;
            }
        }

        // Tenporizadorea: 10 segunduro behin freskatzen du
        private void TimerHasieratu()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 10000;
            _timer.Tick += new EventHandler(timer_Tick);
            _timer.Start();
        }

        private void FreskatuDena()
        {
            try
            {
                _sukaldeForm.EskaerаkFreshatu();
                _banatzaileForm.EskaerаkFreshatu();

                int kop =
                    DatuBasea.EskaerakEgoeraren(EgoeraMota.PrestatzekoZain).Count +
                    DatuBasea.EskaerakEgoeraren(EgoeraMota.Sukaldatzen).Count +
                    DatuBasea.EskaerakEgoeraren(EgoeraMota.BanatzekoZain).Count +
                    DatuBasea.EskaerakEgoeraren(EgoeraMota.Banatzen).Count;

                if (kop > 0)
                    lblEskaeraKop.Text = "● " + kop + " eskaera aktibo";
                else
                    lblEskaeraKop.Text = "";
            }
            catch { }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            base.OnFormClosed(e);
        }
    }
}
