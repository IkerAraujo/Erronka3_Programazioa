using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{


    public class KonexioForm : Form
    {
        public string Zerbitzaria  { get { return txtServer.Text.Trim(); } }
        public string DatuBasea    { get { return txtDB.Text.Trim(); } }
        public string Erabiltzailea { get { return txtUser.Text.Trim(); } }
        public string Pasahitza    { get { return txtPwd.Text; } }

        private TextBox txtServer;
        private TextBox txtDB;
        private TextBox txtUser;
        private TextBox txtPwd;
        private Label   lblErrorea;
        private Button  btnKonektatu;

        public KonexioForm()
        {
            Estiloak.FormEzarri(this, "EuskoPizza — MySQL Konexioa", 420, 420);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            EraikiForms();
        }

        private void EraikiForms()
        {

            Panel banda = new Panel();
            banda.Dock      = DockStyle.Top;
            banda.Height    = 70;
            banda.BackColor = Color.FromArgb(40, 80, 160);

            Label lblTit = new Label();
            lblTit.Text      = "🔌  MySQL Konexio Ezarpenak";
            lblTit.Font      = Estiloak.FontH2;
            lblTit.ForeColor = Color.White;
            lblTit.Location  = new Point(20, 10);
            lblTit.AutoSize  = true;
            lblTit.BackColor = Color.Transparent;
            banda.Controls.Add(lblTit);

            Label lblAzp = new Label();
            lblAzp.Text      = "Ezin da automatikoki konektatu. Sartu zure datuak.";
            lblAzp.Font      = Estiloak.FontTxiki;
            lblAzp.ForeColor = Color.FromArgb(180, 210, 255);
            lblAzp.Location  = new Point(20, 38);
            lblAzp.AutoSize  = true;
            lblAzp.BackColor = Color.Transparent;
            banda.Controls.Add(lblAzp);
            this.Controls.Add(banda);

            int Y = 90;
            int ZAB = 320;


            this.Controls.Add(Estiloak.LabelSortu("Zerbitzaria (Server)",
                40, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtServer = Estiloak.TextBoxSortu(40, Y + 22, ZAB);
            txtServer.Text = "192.168.115.176";
            this.Controls.Add(txtServer);
            Y += 55;


            this.Controls.Add(Estiloak.LabelSortu("Datu-basea (Database)",
                40, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtDB = Estiloak.TextBoxSortu(40, Y + 22, ZAB);
            txtDB.Text = "3erronka";
            this.Controls.Add(txtDB);
            Y += 55;


            this.Controls.Add(Estiloak.LabelSortu("Erabiltzailea (User)",
                40, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtUser = Estiloak.TextBoxSortu(40, Y + 22, ZAB);
            txtUser.Text = "ander";
            this.Controls.Add(txtUser);
            Y += 55;


            this.Controls.Add(Estiloak.LabelSortu("Pasahitza (Password)",
                40, Y, ZAB, 20, Estiloak.FontH3, Estiloak.TestuArgia));
            txtPwd = Estiloak.TextBoxSortu(40, Y + 22, ZAB, pasahitza: true);
            txtPwd.PlaceholderText = "MySQL Workbench instalatzean ezarri zenuen pasahitza";
            this.Controls.Add(txtPwd);
            Y += 55;


            lblErrorea = Estiloak.LabelSortu("",
                40, Y, ZAB, 22, Estiloak.FontTxiki,
                Color.FromArgb(255, 120, 100));
            this.Controls.Add(lblErrorea);
            Y += 30;


            btnKonektatu = Estiloak.BotoiNagusiaSortu(
                "Konektatu →", 40, Y, ZAB, 44,
                Color.FromArgb(40, 80, 160));
            btnKonektatu.Font   = new Font("Segoe UI", 11, FontStyle.Bold);
            btnKonektatu.Click += new EventHandler(btnKonektatu_Click);
            this.Controls.Add(btnKonektatu);

            this.AcceptButton = btnKonektatu;


            this.Shown += new EventHandler(Form_Shown);
        }

        private void btnKonektatu_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            txtPwd.Focus();
        }

        public void ErroreaErakutsi(string mezua)
        {
            lblErrorea.Text = mezua;
            txtPwd.Clear();
            txtPwd.Focus();
        }
    }
}
