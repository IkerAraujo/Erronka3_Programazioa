namespace Pizzeria
{
    // Pizza baten datuak gordetzen ditu
    public class Pizza
    {
        public int    Id           { get; set; }
        public string Izena        { get; set; }
        public string Mota         { get; set; }   // Klasikoa, Berezi, Euskal...
        public string Ingredienteak{ get; set; }
        public double Prezioa      { get; set; }
        public bool   Eskuragarri  { get; set; }

        public Pizza(int id, string izena, string mota, double prezioa,
                     string ingredienteak, bool eskuragarri = true)
        {
            Id            = id;
            Izena         = izena;
            Mota          = mota;
            Prezioa       = prezioa;
            Ingredienteak = ingredienteak;
            Eskuragarri   = eskuragarri;
        }

        public override string ToString()
        {
            return $"{Izena} ({Mota}) — {Prezioa:F2}€";
        }
    }
}
