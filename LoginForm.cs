using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    public class LoginForm : Form
    {
        private TextBox txtUser;
        private TextBox txtPwd;
        private Button btnLogin;
        private Label lblError;

        public LoginForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — Saioa hasi", 420, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            Eraiki();
        }

        private void Eraiki()
        {
        
            this.Controls.Add(
                Estiloak.LabelSortu(
                    "🍕 EuskoPizza",
                    40, 30, 320, 40,
                    Estiloak.FontTitulu,
                    Estiloak.Gorria));

           
            this.Controls.Add(
                Estiloak.LabelSortu(
                    "Erabiltzaile izena",
                    40, 100, 300, 20,
                    Estiloak.FontH3,
                    Estiloak.TestuArgia));

            txtUser = Estiloak.TextBoxSortu(40, 125, 320);
            this.Controls.Add(txtUser);

            this.Controls.Add(
                Estiloak.LabelSortu(
                    "Pasahitza",
                    40, 170, 300, 20,
                    Estiloak.FontH3,
                    Estiloak.TestuArgia));

            txtPwd = Estiloak.TextBoxSortu(40, 195, 320, pasahitza: true);
            this.Controls.Add(txtPwd);

            lblError = Estiloak.LabelSortu(
                "",
                40, 235, 320, 22,
                Estiloak.FontTxiki,
                Color.FromArgb(255, 110, 110));
            this.Controls.Add(lblError);

            btnLogin = Estiloak.BotoiNagusiaSortu(
                "Sartu →",
                40, 270, 320, 46,
                Estiloak.Gorria);

            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            string erabiltzailea = txtUser.Text.Trim();
            string pasahitza = txtPwd.Text;

            if (erabiltzailea == "" || pasahitza == "")
            {
                lblError.Text = "⚠ Bete erabiltzailea eta pasahitza.";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Egiaztatzen...";

            try
            {
                Erabiltzaile erabiltzaileaObj =
                    DatuBasea.SaioaHasi(erabiltzailea, pasahitza);

                if (erabiltzaileaObj == null)
                {
                    lblError.Text = "✗ Erabiltzaile edo pasahitza okerra.";
                    txtPwd.Clear();
                    txtPwd.Focus();
                    return;
                }

                Form hurrengoa = null;

                if (erabiltzaileaObj is LangileArrunta)
                    hurrengoa = new LangileForm((LangileArrunta)erabiltzaileaObj);
                else if (erabiltzaileaObj is Sukaldaria)
                    hurrengoa = new SukaldeForm((Sukaldaria)erabiltzaileaObj);
                else if (erabiltzaileaObj is Banatzailea)
                    hurrengoa = new BanatzaileForm((Banatzailea)erabiltzaileaObj);
                else if (erabiltzaileaObj is Admin)
                    hurrengoa = new AdminForm((Admin)erabiltzaileaObj);

                if (hurrengoa != null)
                {
                    this.Hide();
                    hurrengoa.FormClosed += (s, ev) => this.Close();
                    hurrengoa.Show();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "⚠ Errorea: " + ex.Message;
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Sartu →";
            }
        }
    }
}