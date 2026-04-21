using System.Drawing;
using System.Windows.Forms;

namespace Pizzeria
{
    // Aplikazio osoko kolore eta estilo sistema
    public static class Estiloak
    {
        public static readonly Color Iluna       = Color.FromArgb(28, 28, 35);
        public static readonly Color PanelIluna  = Color.FromArgb(38, 38, 48);
        public static readonly Color PanelArgia  = Color.FromArgb(50, 50, 63);
        public static readonly Color Gorria      = Color.FromArgb(220, 60,  60);
        public static readonly Color Laranja     = Color.FromArgb(230, 130, 30);
        public static readonly Color Berdea      = Color.FromArgb(60,  180, 100);
        public static readonly Color Urdina      = Color.FromArgb(60,  140, 220);
        public static readonly Color Morea       = Color.FromArgb(150, 80,  220);
        public static readonly Color Testua      = Color.FromArgb(230, 230, 235);
        public static readonly Color TestuArgia  = Color.FromArgb(160, 160, 175);
        public static readonly Color Muga        = Color.FromArgb(65,  65,  80);

        public static readonly Font FontTitulu  = new Font("Segoe UI", 18, FontStyle.Bold);
        public static readonly Font FontH2      = new Font("Segoe UI", 12, FontStyle.Bold);
        public static readonly Font FontH3      = new Font("Segoe UI", 10, FontStyle.Bold);
        public static readonly Font FontNormal  = new Font("Segoe UI", 10);
        public static readonly Font FontTxiki   = new Font("Segoe UI",  9);
        public static readonly Font FontMono    = new Font("Consolas",  9);

        public static void FormEzarri(Form form, string titulua, int zabalera, int altuera)
        {
            form.Text            = titulua;
            form.Size            = new Size(zabalera, altuera);
            form.BackColor       = Iluna;
            form.ForeColor       = Testua;
            form.Font            = FontNormal;
            form.StartPosition   = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox     = false;
        }

        public static Panel PanelSortu(int x, int y, int z, int alt, Color? kolorea = null)
        {
            Panel p = new Panel();
            p.Location  = new Point(x, y);
            p.Size      = new Size(z, alt);
            p.BackColor = kolorea ?? PanelIluna;
            return p;
        }

        public static Label LabelSortu(string testua, int x, int y, int z = 300, int alt = 25,
                                        Font font = null, Color? kolorea = null)
        {
            Label l = new Label();
            l.Text      = testua;
            l.Location  = new Point(x, y);
            l.Size      = new Size(z, alt);
            l.ForeColor = kolorea ?? Testua;
            l.Font      = font ?? FontNormal;
            l.BackColor = Color.Transparent;
            return l;
        }

        public static TextBox TextBoxSortu(int x, int y, int z = 200, bool pasahitza = false)
        {
            TextBox t = new TextBox();
            t.Location        = new Point(x, y);
            t.Size            = new Size(z, 32);
            t.BackColor       = PanelArgia;
            t.ForeColor       = Testua;
            t.BorderStyle     = BorderStyle.FixedSingle;
            t.Font            = FontNormal;
            if (pasahitza) t.PasswordChar = '●';
            return t;
        }

        public static Button BotoiNagusiaSortu(string testua, int x, int y,
                                                int z = 160, int alt = 40,
                                                Color? kolorea = null)
        {
            Button b = new Button();
            b.Text      = testua;
            b.Location  = new Point(x, y);
            b.Size      = new Size(z, alt);
            b.BackColor = kolorea ?? Berdea;
            b.ForeColor = Color.White;
            b.Font      = FontH3;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize  = 0;
            b.FlatAppearance.MouseOverBackColor = AltxatuKolorea(kolorea ?? Berdea);
            b.Cursor    = Cursors.Hand;
            return b;
        }

        public static Button BotoiBigarrenaSortu(string testua, int x, int y,
                                                   int z = 140, int alt = 36)
        {
            Button b = new Button();
            b.Text      = testua;
            b.Location  = new Point(x, y);
            b.Size      = new Size(z, alt);
            b.BackColor = PanelArgia;
            b.ForeColor = Testua;
            b.Font      = FontTxiki;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = Muga;
            b.FlatAppearance.BorderSize  = 1;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 88);
            b.Cursor    = Cursors.Hand;
            return b;
        }

        public static ListBox ListBoxSortu(int x, int y, int z, int alt)
        {
            ListBox lb = new ListBox();
            lb.Location         = new Point(x, y);
            lb.Size             = new Size(z, alt);
            lb.BackColor        = PanelArgia;
            lb.ForeColor        = Testua;
            lb.Font             = FontMono;
            lb.BorderStyle      = BorderStyle.FixedSingle;
            lb.SelectionMode    = SelectionMode.One;
            lb.ItemHeight       = 22;
            return lb;
        }

        public static RichTextBox InfoPanelSortu(int x, int y, int z, int alt)
        {
            RichTextBox r = new RichTextBox();
            r.Location    = new Point(x, y);
            r.Size        = new Size(z, alt);
            r.BackColor   = PanelArgia;
            r.ForeColor   = Testua;
            r.Font        = FontMono;
            r.BorderStyle = BorderStyle.FixedSingle;
            r.ReadOnly    = true;
            r.ScrollBars  = RichTextBoxScrollBars.None;
            return r;
        }

        public static Panel TituluBandaSortu(Form form, string testua, Color kolorea, string azpitestua = "")
        {
            Panel banda = new Panel();
            banda.Dock      = DockStyle.Top;
            banda.Height    = azpitestua == "" ? 60 : 75;
            banda.BackColor = kolorea;

            Label lbl = new Label();
            lbl.Text      = testua;
            lbl.Font      = FontTitulu;
            lbl.ForeColor = Color.White;
            lbl.Location  = new Point(20, azpitestua == "" ? 13 : 8);
            lbl.AutoSize  = true;
            banda.Controls.Add(lbl);

            if (azpitestua != "")
            {
                Label lbl2 = new Label();
                lbl2.Text      = azpitestua;
                lbl2.Font      = FontTxiki;
                lbl2.ForeColor = Color.FromArgb(200, 255, 255, 255);
                lbl2.Location  = new Point(22, 45);
                lbl2.AutoSize  = true;
                lbl2.BackColor = Color.Transparent;
                banda.Controls.Add(lbl2);
            }

            form.Controls.Add(banda);
            return banda;
        }

        public static Label EgoeraBadgeSortu(string testua, Color kolorea, int x, int y)
        {
            Label l = new Label();
            l.Text      = $"  {testua}  ";
            l.Font      = FontTxiki;
            l.ForeColor = Color.White;
            l.BackColor = kolorea;
            l.Location  = new Point(x, y);
            l.AutoSize  = true;
            l.Padding   = new Padding(4, 2, 4, 2);
            return l;
        }

        public static Panel LerroaSortu(int x, int y, int zabalera)
        {
            Panel p = new Panel();
            p.Location  = new Point(x, y);
            p.Size      = new Size(zabalera, 1);
            p.BackColor = Muga;
            return p;
        }

        private static Color AltxatuKolorea(Color c)
        {
            return Color.FromArgb(
                System.Math.Min(c.R + 30, 255),
                System.Math.Min(c.G + 30, 255),
                System.Math.Min(c.B + 30, 255));
        }
    }
}
