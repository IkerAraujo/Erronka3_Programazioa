namespace Pizzeria
{
    public static class DatuBasea
    {
        private static string zerbitzaria;
        private static string datuBasea;
        private static string erabiltzailea;
        private static string pasahitza;

        public static void KonexioaEzarri(
            string zerbitzariaBerria,
            string datuBaseaBerria,
            string erabiltzaileaBerria,
            string pasahitzaBerria)
        {
            zerbitzaria = zerbitzariaBerria;
            datuBasea = datuBaseaBerria;
            erabiltzailea = erabiltzaileaBerria;
            pasahitza = pasahitzaBerria;
        }

        public static bool KonexioaEgiaztatu()
        {
            return true;
        }
    }
}
