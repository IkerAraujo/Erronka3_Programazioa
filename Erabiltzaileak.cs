namespace Pizzeria
{
    // ── Erabiltzaile klase abstraktua ─────────────────────────
    // DB-ko "erabiltzaileak" taulari dagokio (rol_id IS NOT NULL = langilea).
    // Polimorfismoa: RolaLortu() metodo abstraktua subklase bakoitzak bere modura
    // implementatzen du.
    public abstract class Erabiltzaile
    {
        public int    Id                  { get; set; }
        public string Izena               { get; set; }
        public string Erabiltzaile_Izena  { get; set; }   // Login-eko username
        public string Pasahitza           { get; set; }
        public bool   Aktibo              { get; set; }

        protected Erabiltzaile(int id, string izena,
                                string erabiltzaileIzena, string pasahitza)
        {
            Id                 = id;
            Izena              = izena;
            Erabiltzaile_Izena = erabiltzaileIzena;
            Pasahitza          = pasahitza;
            Aktibo             = true;
        }

        // Subklase bakoitzak bere rola itzuli behar du
        public abstract string RolaLortu();

        public bool PasahitzaEgiaztatu(string pasahitza)
        {
            return Pasahitza == pasahitza;
        }

        public override string ToString() => $"{Izena} ({RolaLortu()})";
    }

    // ── Subklaseak (herentzia) ────────────────────────────────

    public class LangileArrunta : Erabiltzaile
    {
        public LangileArrunta(int id, string izena,
                               string erabiltzaileIzena, string pasahitza)
            : base(id, izena, erabiltzaileIzena, pasahitza) { }

        public override string RolaLortu() => "Langile Arrunta";
    }

    public class Sukaldaria : Erabiltzaile
    {
        public Sukaldaria(int id, string izena,
                          string erabiltzaileIzena, string pasahitza)
            : base(id, izena, erabiltzaileIzena, pasahitza) { }

        public override string RolaLortu() => "Sukaldaria";
    }

    public class Banatzailea : Erabiltzaile
    {
        public Banatzailea(int id, string izena,
                           string erabiltzaileIzena, string pasahitza)
            : base(id, izena, erabiltzaileIzena, pasahitza) { }

        public override string RolaLortu() => "Banatzailea";
    }

    public class Admin : Erabiltzaile
    {
        public Admin(int id, string izena,
                     string erabiltzaileIzena, string pasahitza)
            : base(id, izena, erabiltzaileIzena, pasahitza) { }

        public override string RolaLortu() => "Admin";
    }
}
