using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Login pantaila
    public class LoginForm : Form
    {
        private TextBox txtErabiltzaileIzena;
        private TextBox txtPasahitza;
        private Button  btnSartu;
        private Label   lblErrorea;

        public LoginForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — Saioa hasi", 420, 470);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            EraikiForms();
        }

        private void EraikiForms()
        {
            Panel goiburua = new Panel();
            goiburua.Dock      = DockStyle.Top;
            goiburua.Height    = 130;
            goiburua.BackColor = Estiloak.Gorria;

            Label lblPizza = new Label();
            lblPizza.Text      = "🍕";
            lblPizza.Font      = new Font("Segoe UI Emoji", 36);
            lblPizza.ForeColor = Color.White;
            lblPizza.Location  = new Point(155, 10);
            lblPizza.AutoSize  = true;
            lblPizza.BackColor = Color.Transparent;

            Label lblIzena = new Label();
            lblIzena.Text      = "EuskoPizza";
            lblIzena.Font      = new Font("Segoe UI", 22, FontStyle.Bold);
            lblIzena.ForeColor = Color.White;
            lblIzena.Location  = new Point(105, 78);
            lblIzena.AutoSize  = true;
            lblIzena.BackColor = Color.Transparent;

            goiburua.Controls.Add(lblPizza);
            goiburua.Controls.Add(lblIzena);
            this.Controls.Add(goiburua);

            Label l1 = Estiloak.LabelSortu("Erabiltzaile izena",
                50, 160, 300, 22, Estiloak.FontH3, Estiloak.TestuArgia);
            txtErabiltzaileIzena = Estiloak.TextBoxSortu(50, 185, 310);
            txtErabiltzaileIzena.PlaceholderText = "adib: ane, jon, admin...";

            Label l2 = Estiloak.LabelSortu("Pasahitza",
                50, 235, 300, 22, Estiloak.FontH3, Estiloak.TestuArgia);
            txtPasahitza = Estiloak.TextBoxSortu(50, 260, 310, pasahitza: true);

            lblErrorea = Estiloak.LabelSortu("",
                50, 308, 310, 22, Estiloak.FontTxiki,
                Color.FromArgb(255, 100, 100));

            btnSartu = Estiloak.BotoiNagusiaSortu("Sartu →",
                50, 340, 310, 46, Estiloak.Gorria);
            btnSartu.Font   = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSartu.Click += new EventHandler(btnSartu_Click);

            Label lblLag = Estiloak.LabelSortu(
                "Erabiltzaileak: ane · jon · mikel · admin",
                50, 400, 310, 20, Estiloak.FontTxiki, Estiloak.TestuArgia);
            lblLag.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.AddRange(new Control[] {
                l1, txtErabiltzaileIzena,
                l2, txtPasahitza,
                lblErrorea, btnSartu, lblLag
            });

            this.AcceptButton = btnSartu;
        }

        private void btnSartu_Click(object sender, EventArgs e)
        {
            lblErrorea.Text = "";
            string eu = txtErabiltzaileIzena.Text.Trim();
            string pw = txtPasahitza.Text;

            if (eu == "" || pw == "")
            {
                lblErrorea.Text = "⚠ Bete itzazu bi eremuak.";
                return;
            }

            btnSartu.Enabled = false;
            btnSartu.Text    = "Egiaztatzea...";

            try
            {
                Erabiltzaile erabiltzailea = DatuBasea.SaioaHasi(eu, pw);

                if (erabiltzailea == null)
                {
                    lblErrorea.Text = "✗ Erabiltzaile izena edo pasahitza okerra.";
                    txtPasahitza.Clear();
                    txtPasahitza.Focus();
                    return;
                }

                Form hurrengoLeihoa = null;

                if (erabiltzailea is LangileArrunta)
                {
                    LangileArrunta l = (LangileArrunta)erabiltzailea;
                    hurrengoLeihoa = new LangileForm(l);
                }
                else if (erabiltzailea is Sukaldaria)
                {
                    Sukaldaria s = (Sukaldaria)erabiltzailea;
                    hurrengoLeihoa = new SukaldeForm(s);
                }
                else if (erabiltzailea is Banatzailea)
                {
                    Banatzailea b = (Banatzailea)erabiltzailea;
                    hurrengoLeihoa = new BanatzaileForm(b);
                }
                else if (erabiltzailea is Admin)
                {
                    Admin a = (Admin)erabiltzailea;
                    hurrengoLeihoa = new AdminForm(a);
                }

                if (hurrengoLeihoa != null)
                {
                    this.Hide();
                    hurrengoLeihoa.FormClosed += new FormClosedEventHandler(Itxi);
                    hurrengoLeihoa.Show();
                }
            }
            catch (Exception ex)
            {
                lblErrorea.Text = "⚠ Errorea: " + ex.Message;
            }
            finally
            {
                btnSartu.Enabled = true;
                btnSartu.Text    = "Sartu →";
            }
        }

        public void EremuakGarbitu()
        {
            txtErabiltzaileIzena.Clear();
            txtPasahitza.Clear();
            lblErrorea.Text = "";
            txtErabiltzaileIzena.Focus();
        }

        private void Itxi(object sender, FormClosedEventArgs e)
        {
            if (!this.Visible)
                this.Close();
        }
    }
}
