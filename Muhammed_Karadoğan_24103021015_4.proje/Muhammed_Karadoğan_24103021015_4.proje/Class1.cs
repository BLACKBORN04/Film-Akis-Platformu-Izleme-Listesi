using System;
using System.Collections.Generic;
using System.Linq;

public class Film
{
    public string Ad { get; set; }
    public string Yonetmen { get; set; }
    public int SüreDakika { get; set; }
    public int YasSiniri { get; set; }
    public double Puan { get; set; }
    public string Tur { get; protected set; }
    public virtual double Fiyat { get; }

    public Film(string ad, string yonetmen, int süre, int yasSiniri, double puan)
    {
        this.Ad = ad; this.Yonetmen = yonetmen; this.SüreDakika = süre;
        this.YasSiniri = yasSiniri; this.Puan = puan;
    }

    public virtual bool TurKuraliniDogrula(out string hataMesaji)
    {
        hataMesaji = "";
        if (Puan < 0 || Puan > 10)
        {
            hataMesaji = $"Hata: Puan 0-10 aralığında olmalıdır, ancak siz '{Puan}' girdiniz.";
            return false;
        }
        if (SüreDakika <= 0)
        {
            hataMesaji = $"Hata: Süre pozitif bir değer olmalıdır, ancak siz '{SüreDakika}' girdiniz.";
            return false;
        }
        return true;
    }

    public override string ToString()
    {
        return $"{Ad} ({Tur}) | Yönetmen: {Yonetmen} | Süre: {SüreDakika}dk | Yaş: +{YasSiniri} | Puan: {Puan:F1} | Fiyat: {Fiyat:C}";
    }
}

public class Aksiyon : Film
{
    public string DublorBilgisi { get; set; }
    public override double Fiyat => 2.0;

    public Aksiyon(string ad, string yonetmen, int süre, int yasSiniri, double puan, string dublor)
        : base(ad, yonetmen, süre, yasSiniri, puan) { Tur = "Aksiyon"; DublorBilgisi = dublor; }

    public override bool TurKuraliniDogrula(out string hataMesaji)
    {
        if (!base.TurKuraliniDogrula(out hataMesaji)) return false;
        if (SüreDakika < 90)
        {
            hataMesaji = $"Hata: Aksiyon filmleri en az 90 dakika olmalıdır, ancak siz {SüreDakika} dakika girdiniz.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(DublorBilgisi))
        {
            hataMesaji = "Hata: Aksiyon filmleri için dublör bilgisi girilmesi zorunludur.";
            return false;
        }
        return true;
    }
}

public class Komedi : Film
{
    public int MizahYogunlugu { get; set; }
    public override double Fiyat => 1.2;

    public Komedi(string ad, string yonetmen, int süre, int yasSiniri, double puan, int mizahYogunlugu)
        : base(ad, yonetmen, süre, yasSiniri, puan) { Tur = "Komedi"; MizahYogunlugu = mizahYogunlugu; }

    public override bool TurKuraliniDogrula(out string hataMesaji)
    {
        if (!base.TurKuraliniDogrula(out hataMesaji)) return false;
        if (SüreDakika > 120)
        {
            hataMesaji = $"Hata: Komedi filmleri en fazla 120 dakika olabilir, ancak siz {SüreDakika} dakika girdiniz.";
            return false;
        }
        if (MizahYogunlugu < 1 || MizahYogunlugu > 5)
        {
            hataMesaji = $"Hata: Mizah yoğunluğu 1-5 arasında olmalıdır, ancak siz {MizahYogunlugu} girdiniz.";
            return false;
        }
        return true;
    }
}

public class Drama : Film
{
    public override double Fiyat => 1.8;

    public Drama(string ad, string yonetmen, int süre, int yasSiniri, double puan)
        : base(ad, yonetmen, süre, yasSiniri, puan) { Tur = "Drama"; }

    public override bool TurKuraliniDogrula(out string hataMesaji)
    {
        if (!base.TurKuraliniDogrula(out hataMesaji)) return false;
        if (SüreDakika < 90)
        {
            hataMesaji = $"Hata: Drama filmleri en az 90 dakika olmalıdır, ancak siz {SüreDakika} dakika girdiniz.";
            return false;
        }
        if (YasSiniri < 13)
        {
            hataMesaji = $"Hata: Drama filmleri için yaş sınırı en az 13 olmalıdır, ancak siz {YasSiniri} girdiniz.";
            return false;
        }
        return true;
    }
}

public class Belgesel : Film
{
    public string Anlatici { get; set; }
    public override double Fiyat => 1.5;

    public Belgesel(string ad, string yonetmen, int süre, int yasSiniri, double puan, string anlatici)
        : base(ad, yonetmen, süre, yasSiniri, puan) { Tur = "Belgesel"; Anlatici = anlatici; }

    public override bool TurKuraliniDogrula(out string hataMesaji)
    {
        if (!base.TurKuraliniDogrula(out hataMesaji)) return false;
        if (SüreDakika < 45 || SüreDakika > 180)
        {
            hataMesaji = $"Hata: Belgesel süresi 45-180 dakika aralığında olmalıdır, ancak siz {SüreDakika} dakika girdiniz.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(Anlatici))
        {
            hataMesaji = "Hata: Belgesel filmleri için anlatıcı bilgisi girilmesi zorunludur.";
            return false;
        }
        return true;
    }
}

public class IzlemeListesi
{
    private List<Film> _izlemeListesi = new List<Film>();
    private double _toplamMaliyet = 0;

    public int FilmSayisi => _izlemeListesi.Count;

    public void FilmEkle(Film film)
    {
        if (film.TurKuraliniDogrula(out string hata))
        {
            _izlemeListesi.Add(film);
            _toplamMaliyet += film.Fiyat;
            Console.WriteLine($"\n[BAŞARILI] '{film.Ad}' ({film.Tur}) izleme listesine eklendi. (Maliyet: {film.Fiyat:C})");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[EKLEME BAŞARISIZ] '{film.Ad}' filmi eklenemedi.");
            Console.WriteLine($"   Sebep: {hata}");
            Console.ResetColor();
        }
    }

    public void FilmCikar(string kullaniciGirisi)
    {
        if (_izlemeListesi.Count == 0) return;

        Film filmToRemove = null;

        if (int.TryParse(kullaniciGirisi, out int filmIndex))
        {
            if (filmIndex >= 1 && filmIndex <= _izlemeListesi.Count)
            {
                filmToRemove = _izlemeListesi[filmIndex - 1];
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[UYARI] Geçersiz numara. Lütfen 1 ile {_izlemeListesi.Count} arasında bir numara girin.");
                Console.ResetColor();
                return;
            }
        }
        else
        {
            filmToRemove = _izlemeListesi.FirstOrDefault(f => f.Ad.Equals(kullaniciGirisi, StringComparison.OrdinalIgnoreCase));
        }

        if (filmToRemove != null)
        {
            _izlemeListesi.Remove(filmToRemove);
            _toplamMaliyet -= filmToRemove.Fiyat;
            Console.WriteLine($"\n[BAŞARILI] '{filmToRemove.Ad}' izleme listesinden çıkarıldı.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[UYARI] '{kullaniciGirisi}' adında veya numarasında bir film bulunamadı.");
            Console.ResetColor();
        }
    }

    public void ListeyiSirala(string siralamaTuru)
    {
        if (_izlemeListesi.Count == 0) { return; }

        string siralamaMesaji = "";
        IEnumerable<Film> siraliListe = _izlemeListesi;
        switch (siralamaTuru.ToLower())
        {
            case "ad": siraliListe = _izlemeListesi.OrderBy(f => f.Ad); siralamaMesaji = "Ada Göre Sıralandı"; break;
            case "puan": siraliListe = _izlemeListesi.OrderByDescending(f => f.Puan); siralamaMesaji = "Puana Göre Sıralandı"; break;
            case "süre": siraliListe = _izlemeListesi.OrderBy(f => f.SüreDakika); siralamaMesaji = "Süreye Göre Sıralandı"; break;
            default: Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("\n[UYARI] Geçersiz sıralama türü. (ad, puan, süre)"); Console.ResetColor(); return;
        }
        _izlemeListesi = siraliListe.ToList();
        ListeyiGoster(siralamaMesaji);
    }

    public void KarisikOynat()
    {
        if (_izlemeListesi.Count < 2)
        {
            Program.BaslikYazdir("Listeyi Karıştır");
            Console.WriteLine("Karıştırılacak yeterli film yok (en az 2 film olmalı).");
            return;
        }

        Random rng = new Random();
        int n = _izlemeListesi.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Film value = _izlemeListesi[k];
            _izlemeListesi[k] = _izlemeListesi[n];
            _izlemeListesi[n] = value;
        }

        Program.BaslikYazdir("Liste Karıştırıldı!");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> SIRADAKİ FİLM ÖNERİNİZ:");
        Console.ResetColor();
        Console.WriteLine($"   {_izlemeListesi[0].ToString()}\n");

        Console.WriteLine("Listenin yeni sıralamasını aşağıda görebilirsiniz:");
        ListeyiGoster("Karıştırılmış Yeni Liste");
    }

    public void ListeyiGoster(string baslik = "GÜNCEL İZLEME LİSTESİ")
    {
        Program.BaslikYazdir(baslik);
        if (_izlemeListesi.Count == 0) { Console.WriteLine("İzleme listeniz boş."); return; }

        int index = 1;
        foreach (var film in _izlemeListesi)
        {
            Console.WriteLine($"{index++}. {film.ToString()}");
        }
        Console.WriteLine(new string('-', 75));
        Console.WriteLine($"TOPLAM FİLM: {_izlemeListesi.Count} | TOPLAM MALİYET: {_toplamMaliyet:C}");
        Console.WriteLine(new string('=', 75));
    }
}