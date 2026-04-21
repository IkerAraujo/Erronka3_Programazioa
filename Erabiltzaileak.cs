namespace Pizzeria
{
    // Klase abstraktua, langile guztiek heredatzen dute
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

        public abstract string RolaLortu();

        public bool PasahitzaEgiaztatu(string pasahitza)
        {
            return Pasahitza == pasahitza;
        }

        public override string ToString() => $"{Izena} ({RolaLortu()})";
    }

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
