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

            DatuBasea.KonexioaEzarri(
                "localhost",
                "pizzeria",
                "root",
                "1MG32025");

            bool konexioaOndo = DatuBasea.KonexioaEgiaztatu();

            Application.Run(new LoginForm());
        }
    }
}