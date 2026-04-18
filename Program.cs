using System;
using System.Windows.Forms;

namespace Pizzeria
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (KonexioForm konForm = new KonexioForm())
            {
                DialogResult result = konForm.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return;
                }

                DatuBasea.KonexioaEzarri(
                    konForm.Zerbitzaria,
                    konForm.DatuBasea,
                    konForm.Erabiltzailea,
                    konForm.Pasahitza
                );

                bool konexioaOndo = DatuBasea.KonexioaEgiaztatu();

                if (!konexioaOndo)
                {
                    konForm.ErroreaErakutsi("Ezin da konektatu datu‑basera");
                    return;
                }
            }

            Application.Run(new LoginForm());
        }
    }
}