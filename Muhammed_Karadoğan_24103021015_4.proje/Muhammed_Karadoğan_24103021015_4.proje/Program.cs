using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

public class Program
{
    public static void Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        IzlemeListesi listem = new IzlemeListesi();

        string secim = "";
        while (secim != "0")
        {
            Console.Clear();
            BaslikYazdir("FİLM AKIŞ PLATFORMU ANA MENÜ");
            Console.WriteLine("1: Film Ekle");
            Console.WriteLine("2: Film Çıkar");
            Console.WriteLine("3: İzleme Listesini Göster");
            Console.WriteLine("4: Listeyi Sırala (ad, puan, süre)");
            Console.WriteLine("5: Listeyi Karıştır (Shuffle)");
            Console.WriteLine("0: Çıkış");
            Console.WriteLine(new string('=', 75));
            Console.Write(">> Seçiminizi yapın: ");

            secim = Console.ReadLine();

            try
            {
                Console.Clear();
                switch (secim)
                {
                    case "1":
                        FilmEkleMenu(listem);
                        GeriDonusIcinBekle();
                        break;
                    case "2":
                        FilmCikarMenu(listem);
                        GeriDonusIcinBekle();
                        break;
                    case "3":
                        listem.ListeyiGoster();
                        GeriDonusIcinBekle();
                        break;
                    case "4":
                        if (listem.FilmSayisi == 0)
                        {
                            Console.WriteLine("\nSıralanacak film yok, listeniz boş.");
                        }
                        else
                        {
                            Console.Write("Sıralama türünü girin (ad, puan, süre): ");
                            listem.ListeyiSirala(Console.ReadLine());
                        }
                        GeriDonusIcinBekle();
                        break;
                    case "5":
                        listem.KarisikOynat();
                        GeriDonusIcinBekle();
                        break;
                    case "0":
                        Console.WriteLine("\nProgramdan çıkılıyor...");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[UYARI] Geçersiz seçim. Lütfen menüdeki sayılardan birini girin.");
                        Console.ResetColor();
                        GeriDonusIcinBekle();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[KRİTİK HATA] Beklenmedik bir hata oluştu: {ex.Message}");
                Console.ResetColor();
                GeriDonusIcinBekle();
            }
        }
    }

    public static void FilmEkleMenu(IzlemeListesi listem)
    {
        BaslikYazdir("Yeni Film Ekleme");
        Console.Write("Film Adı: ");
        string ad = Console.ReadLine();
        Console.Write("Yönetmen: ");
        string yonetmen = Console.ReadLine();

        int süre = ReadInt("Süre (dakika): ");
        int yasSiniri = ReadInt("Yaş Sınırı: ");
        double puan = ReadDouble("Puan (0.0-10.0): ");

        Console.WriteLine("\nFilm Türünü Seçin:");
        Console.WriteLine("A: Aksiyon, K: Komedi, D: Drama, B: Belgesel");
        Console.Write(">> Tür (A/K/D/B): ");
        string turSecim = Console.ReadLine().ToUpper();

        Film yeniFilm = null;
        switch (turSecim)
        {
            case "A":
                Console.Write("Dublör Bilgisi: ");
                yeniFilm = new Aksiyon(ad, yonetmen, süre, yasSiniri, puan, Console.ReadLine());
                break;
            case "K":
                yeniFilm = new Komedi(ad, yonetmen, süre, yasSiniri, puan, ReadInt("Mizah Yoğunluğu (1-5): "));
                break;
            case "D":
                yeniFilm = new Drama(ad, yonetmen, süre, yasSiniri, puan);
                break;
            case "B":
                Console.Write("Anlatıcı Adı: ");
                yeniFilm = new Belgesel(ad, yonetmen, süre, yasSiniri, puan, Console.ReadLine());
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[UYARI] Geçersiz film türü seçimi. Film eklenemedi.");
                Console.ResetColor();
                return;
        }

        if (yeniFilm != null)
        {
            listem.FilmEkle(yeniFilm);
        }
    }

    public static void FilmCikarMenu(IzlemeListesi listem)
    {
        if (listem.FilmSayisi == 0)
        {
            BaslikYazdir("Film Çıkarma");
            Console.WriteLine("Listeniz zaten boş, çıkarılacak film yok.");
            return;
        }

        listem.ListeyiGoster("FİLM ÇIKARILACAK LİSTE");
        Console.Write("\n>> Çıkarmak istediğiniz filmin adını veya numarasını yazın: ");
        listem.FilmCikar(Console.ReadLine());
    }

    public static int ReadInt(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value)) return value;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[UYARI] Geçersiz tam sayı formatı. Lütfen tekrar girin.");
            Console.ResetColor();
        }
    }

    public static double ReadDouble(string prompt)
    {
        double value;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine().Replace(',', '.');
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[UYARI] Geçersiz ondalık sayı formatı. Lütfen tekrar girin (örn: 7.5).");
            Console.ResetColor();
        }
    }

    public static void BaslikYazdir(string baslik)
    {
        int genislik = 75;
        Console.WriteLine(new string('=', genislik));
        string ortalanmisBaslik = String.Format("{0," + ((genislik + baslik.Length) / 2) + "}", baslik);
        Console.WriteLine(ortalanmisBaslik);
        Console.WriteLine(new string('=', genislik));
    }

    public static void GeriDonusIcinBekle()
    {
        Console.WriteLine("\nAna menüye dönmek için bir tuşa basın...");
        Console.ReadKey();
    }
}