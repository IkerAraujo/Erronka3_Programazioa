using System;
using System.Collections.Generic;

namespace Pizzeria
{
    // Eskaera baten egoera posibleak
    public enum EgoeraMota
    {
        PrestatzekoZain,
        Sukaldatzen,
        BanatzekoZain,
        Banatzen,
        Entregatuta
    }

    // Eskaera baten datuak gordetzen ditu
    public class Eskaera
    {
        public int           Id              { get; set; }
        public string        BezeroIzena     { get; set; }
        public string        BezeroHelbidea  { get; set; }
        public bool          EtxekoEntrega   { get; set; }
        public List<Pizza>   Pizzak          { get; set; }
        public EgoeraMota    Egoera          { get; set; }
        public DateTime      Data            { get; set; }

        public double PrezioTotala
        {
            get
            {
                double totala = 0;
                foreach (Pizza p in Pizzak)
                    totala += p.Prezioa;
                return totala;
            }
        }

        public Eskaera(int id, string bezeroIzena, bool etxekoEntrega,
                       string helbidea = "")
        {
            Id             = id;
            BezeroIzena    = bezeroIzena;
            EtxekoEntrega  = etxekoEntrega;
            BezeroHelbidea = helbidea;
            Pizzak         = new List<Pizza>();
            Egoera         = EgoeraMota.PrestatzekoZain;
            Data           = DateTime.Now;
        }

        public void PizzaGehitu(Pizza pizza)
        {
            Pizzak.Add(pizza);
        }

        // Eskaeraren egoera hurrengo fasera pasatzen du
        public void EgoeraPasatu()
        {
            if      (Egoera == EgoeraMota.PrestatzekoZain)
                Egoera = EgoeraMota.Sukaldatzen;
            else if (Egoera == EgoeraMota.Sukaldatzen)
                Egoera = EgoeraMota.BanatzekoZain;
            else if (Egoera == EgoeraMota.BanatzekoZain && EtxekoEntrega)
                Egoera = EgoeraMota.Banatzen;
            else if (Egoera == EgoeraMota.Banatzen ||
                    (Egoera == EgoeraMota.BanatzekoZain && !EtxekoEntrega))
                Egoera = EgoeraMota.Entregatuta;
        }

        public override string ToString()
        {
            string mota = EtxekoEntrega ? "🛵 Etxez-etxe" : "🏠 Dendan";
            return $"#{Id} · {BezeroIzena} · {mota} · {PrezioTotala:F2}€";
        }
    }

    public class KontaktuMezua
    {
        public int      Id      { get; }
        public string   Izena   { get; }
        public string   Gmail   { get; }
        public string   Gaia    { get; }
        public string   Testua  { get; }
        public DateTime Data    { get; }

        public KontaktuMezua(int id, string izena, string gmail,
                              string gaia, string testua, DateTime data)
        {
            Id     = id;
            Izena  = izena;
            Gmail  = gmail;
            Gaia   = gaia;
            Testua = testua;
            Data   = data;
        }
    }
}
