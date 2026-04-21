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

            // DB konexioa konfiguratu eta egiaztatu hasieran
            if (!KonexioaKonfiguratu())
                return;

            Application.Run(new LoginForm());
        }

        // Konexio konfigurazio pantaila erakutsi.
        // Pasahitza zuzena ez bada, berriz galdetzen du.
        private static bool KonexioaKonfiguratu()
        {
            // Lehenengo saiakera: zerbitzaria
            DatuBasea.KonexioaEzarri("192.168.115.176", "3erronka", "ander", "1MG32025");
            if (DatuBasea.KonexioaEgiaztatu()) return true;

            // Pasahitz huts ez → konfigurazio pantaila erakutsi
            using (KonexioForm konForm = new KonexioForm())
            {
                while (true)
                {
                    if (konForm.ShowDialog() != DialogResult.OK)
                        return false;   // Erabiltzaileak utzi du

                    DatuBasea.KonexioaEzarri(
                        konForm.Zerbitzaria,
                        konForm.DatuBasea,
                        konForm.Erabiltzailea,
                        konForm.Pasahitza);

                    if (DatuBasea.KonexioaEgiaztatu())
                        return true;

                    konForm.ErroreaErakutsi(
                        "⚠ Ezin da konektatu. Egiaztatu pasahitza.");
                }
            }
        }
    }
}
