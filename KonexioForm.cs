using System.Windows.Forms;

namespace Pizzeria
{
    public class KonexioForm : Form
    {
        public string Zerbitzaria { get; private set; }
        public string DatuBasea { get; private set; }
        public string Erabiltzailea { get; private set; }
        public string Pasahitza { get; private set; }

        public KonexioForm()
        {
            Zerbitzaria = "localhost";
            DatuBasea = "EuskoPizza";
            Erabiltzailea = "root";
            Pasahitza = "1MG32024";
        }

        public void ErroreaErakutsi(string mezua)
        {
            MessageBox.Show(mezua, "Errorea",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}