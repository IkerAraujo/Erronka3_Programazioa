using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace Pizzeria
{
    // PDF faktura eta txostenak sortzeko (iText7 erabiltzen du)
    public static class PdfSortzailea
    {
        private static readonly string[] LOGO_BIDEAK = new string[]
        {
            @"C:\xampp\htdocs\1MG3_GELAN\Erronka3-Webgunea\argazkiak\EuskoPizza(LOGOA).png",
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png"),
        };

        private static readonly Color GORRIA   = new DeviceRgb(220, 60,  60);
        private static readonly Color GRISA    = new DeviceRgb(240, 240, 240);
        private static readonly Color ILUNA    = new DeviceRgb(40,  40,  50);
        private static readonly Color BERDEA   = new DeviceRgb(60,  180, 100);
        private static readonly Color ZURIA    = ColorConstants.WHITE;

        // Egun bateko eskaera guztien txostena sortzen du
        public static string EgunTxostenaSortu(DateTime eguna)
        {
            List<Eskaera> eskaerак = DatuBasea.EskaerakFakturarakoLortu(eguna);

            string fitxategiIzena = $"EuskoPizza_Txostena_{eguna:yyyyMMdd}_{DateTime.Now:HHmmss}.pdf";
            string bidea = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                fitxategiIzena);

            PdfDocument pdf = new PdfDocument(new PdfWriter(bidea));
            Document dok = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
            dok.SetMargins(40, 40, 40, 40);
            try
            {
                GoiburuaSortu(dok, eguna);

                double totalOrokorra = 0;
                foreach (Eskaera ek in eskaerак)
                    totalOrokorra += ek.PrezioTotala;

                Table laburpena = new Table(new float[] { 1, 1, 1 });
                laburpena.SetWidth(UnitValue.CreatePercentValue(100));
                laburpena.SetMarginBottom(20);

                LaburpenGelaSortu(laburpena, "Eskaera kopurua",
                    eskaerак.Count.ToString(), GORRIA);
                LaburpenGelaSortu(laburpena, "Etxez-etxe",
                    EtxekoKopurua(eskaerак).ToString(), GORRIA);
                LaburpenGelaSortu(laburpena, "Diru-sarrera",
                    $"{totalOrokorra:F2}€", BERDEA);

                dok.Add(laburpena);

                if (eskaerак.Count == 0)
                {
                    Paragraph hutsik = new Paragraph("Ez dago eskaera entregaturik egun honetan.")
                        .SetFontSize(11)
                        .SetFontColor(new DeviceRgb(150, 150, 150))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(30);
                    dok.Add(hutsik);
                }
                else
                {
                    EskaerakTaulaSortu(dok, eskaerак);

                    Paragraph total = new Paragraph($"TOTAL OROKORRA:  {totalOrokorra:F2}€")
                        .SetFontSize(13)
                        .SetBold()
                        .SetFontColor(BERDEA)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetMarginTop(10);
                    dok.Add(total);
                }

                OrrioineaSortu(dok, eguna);
            }
            finally
            {
                dok.Close();
            }

            return bidea;
        }

        // Eskaera baten faktura PDF-a sortzen du
        public static string FakturaSortu(Eskaera eskaera)
        {
            string fitxategiIzena = $"EuskoPizza_Faktura_{eskaera.Id:D4}_{DateTime.Now:HHmmss}.pdf";
            string bidea = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                fitxategiIzena);

            PdfDocument pdf = new PdfDocument(new PdfWriter(bidea));
            Document dok = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
            dok.SetMargins(40, 40, 40, 40);
            try
            {
                GoiburuaSortu(dok, eskaera.Data, $"FAKTURA #{eskaera.Id:D4}");

                Table bezeroTaula = new Table(new float[] { 1, 2 });
                bezeroTaula.SetWidth(UnitValue.CreatePercentValue(60));
                bezeroTaula.SetMarginBottom(15);

                BezeroGelaSortu(bezeroTaula, "Bezero izena",  eskaera.BezeroIzena);
                BezeroGelaSortu(bezeroTaula, "Entrega mota",
                    eskaera.EtxekoEntrega ? "Etxez-etxe" : "Dendan");
                if (eskaera.EtxekoEntrega && !string.IsNullOrEmpty(eskaera.BezeroHelbidea))
                    BezeroGelaSortu(bezeroTaula, "Helbidea", eskaera.BezeroHelbidea);
                BezeroGelaSortu(bezeroTaula, "Data", eskaera.Data.ToString("dd/MM/yyyy HH:mm"));

                dok.Add(bezeroTaula);

                PizzaTaulaSortu(dok, eskaera);

                Paragraph total = new Paragraph($"TOTALA:  {eskaera.PrezioTotala:F2}€")
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(GORRIA)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetMarginTop(8);
                dok.Add(total);

                Paragraph esker = new Paragraph("Eskerrik asko EuskoPizza aukeratzearren!")
                    .SetFontSize(10)
                    .SetFontColor(new DeviceRgb(120, 120, 120))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(30);
                dok.Add(esker);

                OrrioineaSortu(dok, eskaera.Data);
            }
            finally
            {
                dok.Close();
            }

            return bidea;
        }

        private static void GoiburuaSortu(Document dok, DateTime data,
                                           string izenburua = "EGUNEKO TXOSTENA")
        {
            Table goiburuTaula = new Table(new float[] { 1, 3 });
            goiburuTaula.SetWidth(UnitValue.CreatePercentValue(100));
            goiburuTaula.SetMarginBottom(15);

            Cell logoCel = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                                     .SetPaddingRight(15);
            string logoBidea = LogoBideaLortu();
            if (logoBidea != null)
            {
                Image logo = new Image(ImageDataFactory.Create(logoBidea));
                logo.SetWidth(80);
                logoCel.Add(logo);
            }
            else
            {
                logoCel.Add(new Paragraph("EP")
                    .SetFontSize(40)
                    .SetBold()
                    .SetFontColor(GORRIA)
                    .SetTextAlignment(TextAlignment.CENTER));
            }
            goiburuTaula.AddCell(logoCel);

            Cell infoCel = new Cell()
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            infoCel.Add(new Paragraph("EuskoPizza")
                .SetFontSize(22)
                .SetBold()
                .SetFontColor(GORRIA));
            infoCel.Add(new Paragraph("Bertako osagaiekin egindako pizzak")
                .SetFontSize(9)
                .SetFontColor(new DeviceRgb(120, 120, 120)));
            infoCel.Add(new Paragraph(izenburua)
                .SetFontSize(12)
                .SetBold()
                .SetFontColor(ILUNA)
                .SetMarginTop(5));
            infoCel.Add(new Paragraph($"Data: {data:dd/MM/yyyy}")
                .SetFontSize(9)
                .SetFontColor(new DeviceRgb(100, 100, 100)));

            goiburuTaula.AddCell(infoCel);
            dok.Add(goiburuTaula);

            LineSeparator lerroa = new LineSeparator(
                new iText.Kernel.Pdf.Canvas.Draw.SolidLine());
            lerroa.SetMarginBottom(15);
            dok.Add(lerroa);
        }

        private static void EskaerakTaulaSortu(Document dok, List<Eskaera> eskaerак)
        {

            dok.Add(new Paragraph("Eskaeren Zerrenda")
                .SetFontSize(12)
                .SetBold()
                .SetFontColor(ILUNA)
                .SetMarginBottom(5));

            // Taula
            Table taula = new Table(new float[] { 0.5f, 2f, 2f, 1.5f, 1.5f, 1.5f });
            taula.SetWidth(UnitValue.CreatePercentValue(100));


            string[] goiburuak = { "#", "Bezeroa", "Pizzak", "Mota", "Ordua", "Totala" };
            foreach (string g in goiburuak)
            {
                taula.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph(g).SetBold().SetFontSize(9).SetFontColor(ZURIA))
                        .SetBackgroundColor(GORRIA)
                        .SetPadding(5)
                );
            }

            // Datu errenkadak
            bool txandakatu = false;
            foreach (Eskaera ek in eskaerак)
            {
                Color bgcolor = txandakatu ? GRISA : ZURIA;

                string pizzaIzenak = "";
                foreach (Pizza p in ek.Pizzak)
                    pizzaIzenak += p.Izena + ", ";
                if (pizzaIzenak.Length > 2)
                    pizzaIzenak = pizzaIzenak.Substring(0, pizzaIzenak.Length - 2);

                string mota = ek.EtxekoEntrega ? "Etxez-etxe" : "Dendan";

                GelaSortu(taula, $"#{ek.Id}",            bgcolor);
                GelaSortu(taula, ek.BezeroIzena,         bgcolor);
                GelaSortu(taula, pizzaIzenak,             bgcolor);
                GelaSortu(taula, mota,                    bgcolor);
                GelaSortu(taula, ek.Data.ToString("HH:mm"), bgcolor);
                GelaSortu(taula, $"{ek.PrezioTotala:F2}€", bgcolor, TextAlignment.RIGHT);

                txandakatu = !txandakatu;
            }

            dok.Add(taula);
        }

        private static void PizzaTaulaSortu(Document dok, Eskaera eskaera)
        {
            dok.Add(new Paragraph("Eskaerako produktuak")
                .SetFontSize(11)
                .SetBold()
                .SetFontColor(ILUNA)
                .SetMarginBottom(5));

            Table taula = new Table(new float[] { 3f, 1.5f, 1.5f, 1.5f });
            taula.SetWidth(UnitValue.CreatePercentValue(100));
            taula.SetMarginBottom(10);

            string[] goiburuak = { "Produktua", "Mota", "Kop.", "Prezioa" };
            foreach (string g in goiburuak)
            {
                taula.AddHeaderCell(
                    new Cell()
                        .Add(new Paragraph(g).SetBold().SetFontSize(9).SetFontColor(ZURIA))
                        .SetBackgroundColor(GORRIA)
                        .SetPadding(5)
                );
            }

            bool txandakatu = false;
            foreach (Pizza p in eskaera.Pizzak)
            {
                Color bg = txandakatu ? GRISA : ZURIA;
                GelaSortu(taula, p.Izena,           bg);
                GelaSortu(taula, p.Mota,            bg);
                GelaSortu(taula, "1",               bg, TextAlignment.CENTER);
                GelaSortu(taula, $"{p.Prezioa:F2}€", bg, TextAlignment.RIGHT);
                txandakatu = !txandakatu;
            }

            dok.Add(taula);
        }

        private static void LaburpenGelaSortu(Table taula, string etiketa,
                                               string balioa, Color kolorea)
        {
            Cell c = new Cell()
                .SetBackgroundColor(new DeviceRgb(30, 30, 40))
                .SetPadding(10)
                .SetTextAlignment(TextAlignment.CENTER);
            c.Add(new Paragraph(etiketa)
                .SetFontSize(8)
                .SetFontColor(new DeviceRgb(160, 160, 180)));
            c.Add(new Paragraph(balioa)
                .SetFontSize(18)
                .SetBold()
                .SetFontColor(kolorea));
            taula.AddCell(c);
        }

        private static void BezeroGelaSortu(Table taula, string etiketa, string balioa)
        {
            taula.AddCell(new Cell()
                .Add(new Paragraph(etiketa).SetBold().SetFontSize(9))
                .SetBackgroundColor(GRISA)
                .SetPadding(4));
            taula.AddCell(new Cell()
                .Add(new Paragraph(balioa).SetFontSize(9))
                .SetPadding(4));
        }

        private static void GelaSortu(Table taula, string testua, Color bgcolor,
                                       TextAlignment lerrokatzea = TextAlignment.LEFT)
        {
            taula.AddCell(new Cell()
                .Add(new Paragraph(testua)
                    .SetFontSize(8)
                    .SetTextAlignment(lerrokatzea))
                .SetBackgroundColor(bgcolor)
                .SetPadding(4));
        }

        private static void OrrioineaSortu(Document dok, DateTime data)
        {
            LineSeparator lerroa = new LineSeparator(
                new iText.Kernel.Pdf.Canvas.Draw.SolidLine());
            lerroa.SetMarginTop(20).SetMarginBottom(5);
            dok.Add(lerroa);

            dok.Add(new Paragraph(
                $"EuskoPizza  ·  Inprimatze data: {DateTime.Now:dd/MM/yyyy HH:mm}  ·  Aplikazioa: C# .NET 8")
                .SetFontSize(7)
                .SetFontColor(new DeviceRgb(150, 150, 150))
                .SetTextAlignment(TextAlignment.CENTER));
        }

        private static string LogoBideaLortu()
        {
            foreach (string bide in LOGO_BIDEAK)
            {
                if (File.Exists(bide)) return bide;
            }
            return null;
        }

        private static int EtxekoKopurua(List<Eskaera> eskaerак)
        {
            int kop = 0;
            foreach (Eskaera e in eskaerак)
                if (e.EtxekoEntrega) kop++;
            return kop;
        }
    }
}
