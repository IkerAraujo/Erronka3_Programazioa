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

    // ── Eskaera klasea ────────────────────────────────────────
    // DB-ko "eskaerak" taulari dagokio.
    // Klase honek eskaera baten datu guztiak gordetzen ditu.
    public class Eskaera
    {
        public int           Id              { get; set; }
        public string        BezeroIzena     { get; set; }
        public string        BezeroHelbidea  { get; set; }
        public bool          EtxekoEntrega   { get; set; }
        public List<Pizza>   Pizzak          { get; set; }
        public EgoeraMota    Egoera          { get; set; }
        public DateTime      Data            { get; set; }

        // Kalkulatutako propietatea: pizza guztien prezioen batura
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

        // Pizza zerrendaratu eskaerara
        public void PizzaGehitu(Pizza pizza)
        {
            Pizzak.Add(pizza);
        }

        // Egoera hurrengo fasera pasa (egoera makina)
        // Oharra: DB-a eguneratzeko DatuBasea.EskaeraEgoeraPasatu() deitu behar da!
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
}
