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

            if (!KonexioaKonfiguratu())
                return;

            Application.Run(new LoginForm());
        }

        private static bool KonexioaKonfiguratu()
        {
            DatuBasea.KonexioaEzarri("localhost", "EuskoPizza", "root", "1MG32025");
            if (DatuBasea.KonexioaEgiaztatu())
                return true;

            using (KonexioForm konForm = new KonexioForm())
            {
                while (true)
                {
                    if (konForm.ShowDialog() != DialogResult.OK)
                        return false;

                    DatuBasea.KonexioaEzarri(
                        konForm.Zerbitzaria,
                        konForm.DatuBasea,
                        konForm.Erabiltzailea,
                        konForm.Pasahitza
                    );

                    if (DatuBasea.KonexioaEgiaztatu())
                        return true;

                    konForm.ErroreaErakutsi(
                        "Ezin da konektatu. Egiaztatu datuak."
                    );
                }
            }
        }
    }
}