using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // MessageBox ordezko pertsonalizatua, euskarazko botoiekin
    public static class EuskaraElkarrizketa
    {
        // "Bai" / "Ez" galdera-elkarrizketa. true itzultzen du "Bai" sakatuz gero.
        public static bool GaldeBaiEz(string mezua, string izenburua)
        {
            bool emaitza = false;

            Form f = new Form();
            f.Text            = izenburua;
            f.Size            = new Size(420, 180);
            f.StartPosition   = FormStartPosition.CenterParent;
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox     = false;
            f.MinimizeBox     = false;
            f.BackColor       = Color.FromArgb(30, 30, 40);

            Label lbl = new Label();
            lbl.Text      = mezua;
            lbl.ForeColor = Color.FromArgb(220, 220, 230);
            lbl.Font      = new Font("Segoe UI", 10);
            lbl.Location  = new Point(20, 20);
            lbl.Size      = new Size(375, 70);
            f.Controls.Add(lbl);

            Button btnBai = new Button();
            btnBai.Text      = "Bai";
            btnBai.Size      = new Size(100, 36);
            btnBai.Location  = new Point(195, 100);
            btnBai.BackColor = Color.FromArgb(60, 160, 80);
            btnBai.ForeColor = Color.White;
            btnBai.FlatStyle = FlatStyle.Flat;
            btnBai.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            btnBai.Click    += (s, e) => { emaitza = true; f.Close(); };
            f.Controls.Add(btnBai);

            Button btnEz = new Button();
            btnEz.Text      = "Ez";
            btnEz.Size      = new Size(100, 36);
            btnEz.Location  = new Point(305, 100);
            btnEz.BackColor = Color.FromArgb(180, 60, 60);
            btnEz.ForeColor = Color.White;
            btnEz.FlatStyle = FlatStyle.Flat;
            btnEz.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEz.Click    += (s, e) => { emaitza = false; f.Close(); };
            f.Controls.Add(btnEz);

            f.AcceptButton = btnBai;
            f.CancelButton = btnEz;

            f.ShowDialog();
            return emaitza;
        }

        // Mezu sinplea "Ados" botoiarekin
        public static void Mezua(string mezua, string izenburua, bool errorea = false)
        {
            Form f = new Form();
            f.Text            = izenburua;
            f.Size            = new Size(420, 160);
            f.StartPosition   = FormStartPosition.CenterParent;
            f.FormBorderStyle = FormBorderStyle.FixedDialog;
            f.MaximizeBox     = false;
            f.MinimizeBox     = false;
            f.BackColor       = Color.FromArgb(30, 30, 40);

            Label lbl = new Label();
            lbl.Text      = mezua;
            lbl.ForeColor = errorea
                ? Color.FromArgb(255, 120, 100)
                : Color.FromArgb(220, 220, 230);
            lbl.Font      = new Font("Segoe UI", 10);
            lbl.Location  = new Point(20, 20);
            lbl.Size      = new Size(375, 55);
            f.Controls.Add(lbl);

            Button btnAdos = new Button();
            btnAdos.Text      = "Ados";
            btnAdos.Size      = new Size(100, 36);
            btnAdos.Location  = new Point(300, 82);
            btnAdos.BackColor = Color.FromArgb(40, 80, 160);
            btnAdos.ForeColor = Color.White;
            btnAdos.FlatStyle = FlatStyle.Flat;
            btnAdos.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdos.Click    += (s, e) => f.Close();
            f.Controls.Add(btnAdos);

            f.AcceptButton = btnAdos;
            f.CancelButton = btnAdos;

            f.ShowDialog();
        }
    }
}
