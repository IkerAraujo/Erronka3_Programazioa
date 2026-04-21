using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Pizzeria
{
    // MySQL datu-basearekin konektatzen den klase estatikoa
    public static class DatuBasea
    {
        private static string _server   = "192.168.115.176";
        private static string _database = "3erronka";
        private static string _user     = "admin";
        private static string _password = "1MG32025";

        public static void KonexioaEzarri(string server, string database,
                                           string user,   string password)
        {
            _server   = server;
            _database = database;
            _user     = user;
            _password = password;
        }

        private static string KonexioKatea()
        {
            return "Server=" + _server + ";Port=3306;Database=" + _database + ";" +
                   "Uid=" + _user + ";Pwd=" + _password + ";" +
                   "CharSet=utf8mb4;SslMode=None;AllowPublicKeyRetrieval=True;";
        }

        public static MySqlConnection KonexioaSortu()
        {
            return new MySqlConnection(KonexioKatea());
        }

        // true itzultzen du konexioa ondo badoa
        public static bool KonexioaEgiaztatu()
        {
            try
            {
                MySqlConnection kon = KonexioaSortu();
                kon.Open();
                kon.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // --- LOGIN ---

        // Erabiltzailea egiaztatu eta bere rol-objektua itzultzen du, edo null
        public static Erabiltzaile SaioaHasi(string erabiltzaileIzena, string pasahitza)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT e.id, e.izena, e.pasahitza, r.izena AS rola " +
                         "FROM langileak e " +
                         "JOIN rolak r ON r.id = e.rol_id " +
                         "WHERE LOWER(e.izena) = LOWER(@izena) " +
                         "AND e.aktibo = 1 LIMIT 1";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@izena", erabiltzaileIzena.Trim());

            MySqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                dr.Close();
                kon.Close();
                return null;
            }

            string dbPasahitza = dr.GetString("pasahitza");

            if (dbPasahitza != pasahitza)
            {
                dr.Close();
                kon.Close();
                return null;
            }

            int    id    = dr.GetInt32("id");
            string izena = dr.GetString("izena");
            string rola  = dr.GetString("rola");

            dr.Close();
            kon.Close();

            if (rola == "Admin")
                return new Admin(id, izena, erabiltzaileIzena, pasahitza);
            else if (rola == "Sukaldaria")
                return new Sukaldaria(id, izena, erabiltzaileIzena, pasahitza);
            else if (rola == "Banatzailea")
                return new Banatzailea(id, izena, erabiltzaileIzena, pasahitza);
            else
                return new LangileArrunta(id, izena, erabiltzaileIzena, pasahitza);
        }

        // --- PIZZAK ---

        public static List<Pizza> PizzakLortu()
        {
            List<Pizza> pizzak = new List<Pizza>();

            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT id, izena, mota, prezioa, ingredienteak " +
                         "FROM pizzak WHERE eskuragarri = 1 ORDER BY mota, izena";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Pizza p = new Pizza(
                    dr.GetInt32("id"),
                    dr.GetString("izena"),
                    dr.GetString("mota"),
                    (double)dr.GetDecimal("prezioa"),
                    dr.GetString("ingredienteak")
                );
                pizzak.Add(p);
            }

            dr.Close();
            kon.Close();
            return pizzak;
        }

        // Admin-entzat: desaktibatutakoak ere itzultzen ditu
        public static List<Pizza> PizzakDenakLortu()
        {
            List<Pizza> pizzak = new List<Pizza>();

            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT id, izena, mota, prezioa, ingredienteak, eskuragarri " +
                         "FROM pizzak ORDER BY mota, izena";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Pizza p = new Pizza(
                    dr.GetInt32("id"),
                    dr.GetString("izena"),
                    dr.GetString("mota"),
                    (double)dr.GetDecimal("prezioa"),
                    dr.GetString("ingredienteak"),
                    dr.GetBoolean("eskuragarri")
                );
                pizzak.Add(p);
            }

            dr.Close();
            kon.Close();
            return pizzak;
        }

        public static void PizzaGehitu(Pizza pizza)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "INSERT INTO pizzak (izena, mota, ingredienteak, prezioa, eskuragarri) " +
                         "VALUES (@izena, @mota, @ingredienteak, @prezioa, 1)";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@izena",         pizza.Izena);
            cmd.Parameters.AddWithValue("@mota",          pizza.Mota);
            cmd.Parameters.AddWithValue("@ingredienteak", pizza.Ingredienteak);
            cmd.Parameters.AddWithValue("@prezioa",       pizza.Prezioa);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        public static void PizzaEguneratu(Pizza pizza)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE pizzak SET izena=@izena, mota=@mota, " +
                         "ingredienteak=@ingredienteak, prezioa=@prezioa, " +
                         "eskuragarri=@eskuragarri WHERE id=@id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@id",            pizza.Id);
            cmd.Parameters.AddWithValue("@izena",         pizza.Izena);
            cmd.Parameters.AddWithValue("@mota",          pizza.Mota);
            cmd.Parameters.AddWithValue("@ingredienteak", pizza.Ingredienteak);
            cmd.Parameters.AddWithValue("@prezioa",       pizza.Prezioa);
            cmd.Parameters.AddWithValue("@eskuragarri",   pizza.Eskuragarri ? 1 : 0);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        public static void PizzaDesaktibatu(int id)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE pizzak SET eskuragarri = 0 WHERE id = @id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        // --- ESKAERAK ---

        public static int EskaeraGorde(Eskaera eskaera)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();
            MySqlTransaction transakzioa = kon.BeginTransaction();

            try
            {
                // oharra formatua: "BezeroIzena" edo "BezeroIzena|Helbidea"
                string oharra = eskaera.BezeroIzena;
                if (eskaera.EtxekoEntrega)
                    oharra = oharra + "|" + eskaera.BezeroHelbidea;

                string sql1 = "INSERT INTO eskaerak (egoera, oharra, sortze_data, eguneratze_data) " +
                              "VALUES ('Prestatzeko zain', @oharra, NOW(), NOW())";

                MySqlCommand cmd1 = new MySqlCommand(sql1, kon, transakzioa);
                cmd1.Parameters.AddWithValue("@oharra", oharra);
                cmd1.ExecuteNonQuery();

                int eskaeraId = (int)cmd1.LastInsertedId;

                for (int i = 0; i < eskaera.Pizzak.Count; i++)
                {
                    Pizza p = eskaera.Pizzak[i];

                    string sql2 = "INSERT INTO eskaera_elementuak " +
                                  "(eskaera_id, pizza_id, kantitatea, prezioa) " +
                                  "VALUES (@eskaera_id, @pizza_id, 1, @prezioa)";

                    MySqlCommand cmd2 = new MySqlCommand(sql2, kon, transakzioa);
                    cmd2.Parameters.AddWithValue("@eskaera_id", eskaeraId);
                    cmd2.Parameters.AddWithValue("@pizza_id",   p.Id);
                    cmd2.Parameters.AddWithValue("@prezioa",    p.Prezioa);
                    cmd2.ExecuteNonQuery();
                }

                if (eskaera.EtxekoEntrega)
                {
                    string sql3 = "INSERT INTO banaketak (eskaera_id, helbidea) " +
                                  "VALUES (@eskaera_id, @helbidea)";

                    MySqlCommand cmd3 = new MySqlCommand(sql3, kon, transakzioa);
                    cmd3.Parameters.AddWithValue("@eskaera_id", eskaeraId);
                    cmd3.Parameters.AddWithValue("@helbidea",   eskaera.BezeroHelbidea);
                    cmd3.ExecuteNonQuery();
                }

                transakzioa.Commit();
                kon.Close();
                return eskaeraId;
            }
            catch (Exception ex)
            {
                transakzioa.Rollback();
                kon.Close();
                throw ex;
            }
        }

        public static List<Eskaera> EskaerakEgoeraren(EgoeraMota egoera)
        {
            List<Eskaera> eskaerак = new List<Eskaera>();
            string egoeraTx = EgoeraTestura(egoera);

            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT e.id, e.oharra, e.sortze_data, " +
                         "CASE WHEN b.id IS NOT NULL THEN 1 ELSE 0 END AS etxeko " +
                         "FROM eskaerak e " +
                         "LEFT JOIN banaketak b ON b.eskaera_id = e.id " +
                         "WHERE e.egoera = @egoera ORDER BY e.id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@egoera", egoeraTx);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int    id     = dr.GetInt32("id");
                string oharra = dr.IsDBNull(dr.GetOrdinal("oharra")) ? "" : dr.GetString("oharra");
                DateTime data = dr.GetDateTime("sortze_data");
                bool   etxeko = dr.GetBoolean("etxeko");

                string[] zatiak    = oharra.Split('|');
                string bezeroIzena = zatiak.Length > 0 ? zatiak[0] : "";
                string helbidea    = zatiak.Length > 1 ? zatiak[1] : "";

                Eskaera ek = new Eskaera(id, bezeroIzena, etxeko, helbidea);
                ek.Egoera  = egoera;
                ek.Data    = data;
                eskaerак.Add(ek);
            }

            dr.Close();
            kon.Close();

            for (int i = 0; i < eskaerак.Count; i++)
                PizzakEskaeranKargatu(eskaerак[i]);

            return eskaerак;
        }

        private static void PizzakEskaeranKargatu(Eskaera eskaera)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT p.id, p.izena, p.mota, ee.prezioa, p.ingredienteak " +
                         "FROM eskaera_elementuak ee " +
                         "JOIN pizzak p ON p.id = ee.pizza_id " +
                         "WHERE ee.eskaera_id = @id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@id", eskaera.Id);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Pizza p = new Pizza(
                    dr.GetInt32("id"),
                    dr.GetString("izena"),
                    dr.GetString("mota"),
                    (double)dr.GetDecimal("prezioa"),
                    dr.GetString("ingredienteak")
                );
                eskaera.Pizzak.Add(p);
            }

            dr.Close();
            kon.Close();
        }

        public static void EskaeraEzabatu(int eskaeraId)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();
            MySqlTransaction transakzioa = kon.BeginTransaction();

            try
            {
                MySqlCommand cmd1 = new MySqlCommand(
                    "DELETE FROM eskaera_elementuak WHERE eskaera_id = @id", kon, transakzioa);
                cmd1.Parameters.AddWithValue("@id", eskaeraId);
                cmd1.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand(
                    "DELETE FROM eskaerak WHERE id = @id", kon, transakzioa);
                cmd2.Parameters.AddWithValue("@id", eskaeraId);
                cmd2.ExecuteNonQuery();

                transakzioa.Commit();
            }
            catch
            {
                transakzioa.Rollback();
                throw;
            }
            finally
            {
                kon.Close();
            }
        }

        public static void EskaeraEgoeraPasatu(int eskaeraId, string egoeraBerria)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE eskaerak SET egoera = @egoera, " +
                         "eguneratze_data = NOW() WHERE id = @id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@egoera", egoeraBerria);
            cmd.Parameters.AddWithValue("@id",     eskaeraId);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        // Banatzaileak eskaera hartzen duenean deitzen da
        public static void BanaketaHasi(int eskaeraId, int banatzaileId)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE banaketak SET banatzaile_id = @bid, hasiera = NOW() " +
                         "WHERE eskaera_id = @eid";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@bid", banatzaileId);
            cmd.Parameters.AddWithValue("@eid", eskaeraId);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        public static void BanaketaAmaitu(int eskaeraId)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE banaketak SET amaiera = NOW() WHERE eskaera_id = @eid";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@eid", eskaeraId);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        // --- LANGILEAK ---

        public static List<Erabiltzaile> LangileakLortu()
        {
            List<Erabiltzaile> langileak = new List<Erabiltzaile>();

            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT e.id, e.izena, e.aktibo, r.izena AS rola " +
                         "FROM langileak e " +
                         "JOIN rolak r ON r.id = e.rol_id " +
                         "ORDER BY r.id, e.izena";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int    id     = dr.GetInt32("id");
                string izena  = dr.GetString("izena");
                string rola   = dr.GetString("rola");
                bool   aktibo = dr.GetBoolean("aktibo");

                Erabiltzaile e;

                if (rola == "Admin")
                    e = new Admin(id, izena, izena.ToLower(), "");
                else if (rola == "Sukaldaria")
                    e = new Sukaldaria(id, izena, izena.ToLower(), "");
                else if (rola == "Banatzailea")
                    e = new Banatzailea(id, izena, izena.ToLower(), "");
                else
                    e = new LangileArrunta(id, izena, izena.ToLower(), "");

                e.Aktibo = aktibo;
                langileak.Add(e);
            }

            dr.Close();
            kon.Close();
            return langileak;
        }

        public static void LangilaGehitu(string izena, string abizena,
                                          string nan,   string gmail,
                                          string pasahitza, int rolId)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "INSERT INTO langileak " +
                         "(izena, abizena, nan, gmail, pasahitza, rol_id) " +
                         "VALUES (@izena, @abizena, @nan, @gmail, @pasahitza, @rolId)";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@izena",     izena);
            cmd.Parameters.AddWithValue("@abizena",   abizena);
            cmd.Parameters.AddWithValue("@nan",       nan);
            cmd.Parameters.AddWithValue("@gmail",     gmail);
            cmd.Parameters.AddWithValue("@pasahitza", pasahitza);
            cmd.Parameters.AddWithValue("@rolId",     rolId);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        public static void LangileDesaktibatu(int id)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE langileak SET aktibo = 0 WHERE id = @id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        public static void LangileEguneratu(int id, string izena, string abizena,
                                             string nan, string gmail, int rolId)
        {
            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "UPDATE langileak SET izena=@izena, abizena=@abizena, " +
                         "nan=@nan, gmail=@gmail, rol_id=@rolId WHERE id=@id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@izena",   izena);
            cmd.Parameters.AddWithValue("@abizena", abizena);
            cmd.Parameters.AddWithValue("@nan",     nan);
            cmd.Parameters.AddWithValue("@gmail",   gmail);
            cmd.Parameters.AddWithValue("@rolId",   rolId);
            cmd.Parameters.AddWithValue("@id",      id);
            cmd.ExecuteNonQuery();

            kon.Close();
        }

        public static List<KontaktuMezua> KontaktuMezuakLortu()
        {
            List<KontaktuMezua> mezuak = new List<KontaktuMezua>();

            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT id, izena, gmail, gaia, testua, data " +
                         "FROM kontaktu_mezuak ORDER BY data DESC";

            MySqlCommand    cmd = new MySqlCommand(sql, kon);
            MySqlDataReader dr  = cmd.ExecuteReader();

            while (dr.Read())
            {
                mezuak.Add(new KontaktuMezua(
                    dr.GetInt32("id"),
                    dr.GetString("izena"),
                    dr.GetString("gmail"),
                    dr.GetString("gaia"),
                    dr.GetString("testua"),
                    dr.GetDateTime("data")
                ));
            }

            dr.Close();
            kon.Close();
            return mezuak;
        }

        // --- PDF ---

        public static List<Eskaera> EskaerakFakturarakoLortu(DateTime eguna)
        {
            List<Eskaera> eskaerак = new List<Eskaera>();

            MySqlConnection kon = KonexioaSortu();
            kon.Open();

            string sql = "SELECT e.id, e.oharra, e.sortze_data, " +
                         "CASE WHEN b.id IS NOT NULL THEN 1 ELSE 0 END AS etxeko " +
                         "FROM eskaerak e " +
                         "LEFT JOIN banaketak b ON b.eskaera_id = e.id " +
                         "WHERE e.egoera = 'Entregatuta' " +
                         "AND DATE(e.sortze_data) = @eguna ORDER BY e.id";

            MySqlCommand cmd = new MySqlCommand(sql, kon);
            cmd.Parameters.AddWithValue("@eguna", eguna.Date.ToString("yyyy-MM-dd"));
            MySqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int    id     = dr.GetInt32("id");
                string oharra = dr.IsDBNull(dr.GetOrdinal("oharra")) ? "" : dr.GetString("oharra");
                DateTime data = dr.GetDateTime("sortze_data");
                bool   etxeko = dr.GetBoolean("etxeko");

                string[] zatiak    = oharra.Split('|');
                string bezeroIzena = zatiak.Length > 0 ? zatiak[0] : "";
                string helbidea    = zatiak.Length > 1 ? zatiak[1] : "";

                Eskaera ek = new Eskaera(id, bezeroIzena, etxeko, helbidea);
                ek.Egoera  = EgoeraMota.Entregatuta;
                ek.Data    = data;
                eskaerак.Add(ek);
            }

            dr.Close();
            kon.Close();

            for (int i = 0; i < eskaerак.Count; i++)
                PizzakEskaeranKargatu(eskaerак[i]);

            return eskaerак;
        }

        // EgoeraMota enuma DB-ko testu bihurtzen du
        public static string EgoeraTestura(EgoeraMota egoera)
        {
            if      (egoera == EgoeraMota.PrestatzekoZain) return "Prestatzeko zain";
            else if (egoera == EgoeraMota.Sukaldatzen)     return "Sukaldatzen";
            else if (egoera == EgoeraMota.BanatzekoZain)   return "Banatzeko zain";
            else if (egoera == EgoeraMota.Banatzen)         return "Banatzen";
            else if (egoera == EgoeraMota.Entregatuta)      return "Entregatuta";
            else                                             return "Prestatzeko zain";
        }
    }
}
