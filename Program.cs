using System;
using System.Text.Json.Nodes;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;

namespace poznamky2
{
    enum Moznosti
    {
        Zobrazit = 1,
        NapsatPoznamku = 2,
        SmazatPoznamku = 3,
        UpravitPoznamku = 4,
        VytvoritKategorii = 5,
        SmazatKategorii = 6,
        UpravitKategorii = 7,
        Konec = 8,
    }

    class Kategorie()
    {

        public string nazev;
        public void podoba_kategorie()
        {
            Console.WriteLine(nazev.ToUpper());
            Console.WriteLine("           ");
            Console.WriteLine("------------------------------------------------------------------------------------------");

        }
        public Kategorie(string u_nazev) :this()
        {
            nazev = u_nazev;
            
        }

        public static Kategorie ziskat_kategorii(string vypsat)
        {
            int count = 0;
            foreach (Kategorie k in Program.ListKategorie)
            {
                Console.WriteLine(count + ") " + k.nazev);
                count = count + 1;

            }
            Console.WriteLine(vypsat);
            string kkategorie = Console.ReadLine();
            int index = int.Parse(kkategorie);
            Kategorie objekt_kategorie = Program.ListKategorie[index];
            return objekt_kategorie;
        }

    }
    class Poznamka()
    {
        public string nazev;
        public string obsah;
        public Kategorie kategorie;
        public DateTime datumvytvoreno;
        public DateTime datumvyprsino;
        public void podoba_poznamky()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(nazev.ToUpper());
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("           ");
            Console.WriteLine(obsah);
            Console.WriteLine("           ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Datum vytvoření poznámky: "+datumvytvoreno);
            Console.ForegroundColor= ConsoleColor.Red;
            Console.WriteLine("Datum vypršení poznamky: "+datumvyprsino);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("------------------------------------------------------------------------------------------");

        }
        public Poznamka(string u_nazev, string u_obsah, DateTime u_datumvytvoreno, DateTime u_datumvyprsino,Kategorie u_kategorie) :this()
        {
            nazev =u_nazev;
            kategorie = u_kategorie;
            obsah = u_obsah;
            datumvyprsino = u_datumvyprsino;
            datumvytvoreno = u_datumvytvoreno;
        }

        public static Poznamka ziskat_pounamku(string vypsat)
        {
            int count = 0;
            foreach (Poznamka k in Program.ListPoznamek)
            {
                Console.WriteLine(count + ") " + k.nazev);
                count = count + 1;

            }
            Console.WriteLine(vypsat);
            string poznamka = Console.ReadLine();
            int index = int.Parse(poznamka);
            Poznamka objekt_poznamka = Program.ListPoznamek[index];
            return objekt_poznamka;
        }

    }
    internal class Program
    {
        public static List<Poznamka> ListPoznamek = new List<Poznamka>();
        public static List<Kategorie> ListKategorie = new List<Kategorie>();
        static string ulozeni_kategorii = "kategorie.txt";
        static string ulozeni_poznamek = "poznamky.txt";
        static void NacistKategorie()
        {
            if (File.Exists(ulozeni_kategorii))
            {
                string json = File.ReadAllText(ulozeni_kategorii);
                if (json != "")
                {
                    List<Kategorie> nacteny_list_k = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Kategorie>>(json);
                    ListKategorie = nacteny_list_k;
                    
                }

            }
            else
            {
                File.Create(ulozeni_kategorii).Dispose();
            }
        }
        static void NacistPoznamku()
        {
            if (File.Exists(ulozeni_poznamek))
            {
                string json = File.ReadAllText(ulozeni_poznamek);
                if (json != "")
                {
                    List<Poznamka> nacteny_list_p = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Poznamka>>(json);
                    ListPoznamek = nacteny_list_p;

                }

            }
            else
            {
                File.Create(ulozeni_poznamek).Dispose();
            }
        }
        static void UlozitData()
        {
            string ulozit_kategorie = Newtonsoft.Json.JsonConvert.SerializeObject(ListKategorie);
            File.WriteAllText(ulozeni_kategorii, ulozit_kategorie);
            string ulozit_poznamky = Newtonsoft.Json.JsonConvert.SerializeObject(ListPoznamek);
            File.WriteAllText(ulozeni_poznamek, ulozit_poznamky);
        }
        static void SmazatVyprsenePoznamky()
        {
            int smazano = ListPoznamek.RemoveAll(poznamka => DateTime.Compare(poznamka.datumvyprsino,DateTime.Now)<0);
            Console.WriteLine("Smazáno vypršených poznámek: "+ smazano);

        }

        static void Main(string[] args)
        {
            NacistKategorie();
            NacistPoznamku();
            SmazatVyprsenePoznamky ();
            int option;
            do
            {
                option = Getimput();
                switch(option)
                {
                    case (int)Moznosti.Zobrazit: Zobrazit(); break;
                    case (int)Moznosti.NapsatPoznamku: NapsatPoznamku(); break;
                    case (int)Moznosti.UpravitPoznamku: UpravitPoznamku(); break;
                    case (int)Moznosti.SmazatPoznamku: SmazatPoznamku(); break;
                    case (int)Moznosti.VytvoritKategorii: VytvoritKategorii(); break;
                    case (int)Moznosti.UpravitKategorii: UpravitKategorii(); break;
                    case (int)Moznosti.SmazatKategorii: SmazatKategorii(); break;
                }
                Console.Clear();
                 
            }
            while (option != (int)Moznosti.Konec) ;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Konec Programu ");
            Console.ForegroundColor= ConsoleColor.White;
            UlozitData();
        }


        static void Zobrazit()
        {
            Kategorie objekt_kategorie = Kategorie.ziskat_kategorii("Zadej číslo kategorie kterou chceš zobrazit:");
            foreach (Poznamka poznamka in ListPoznamek)
            {
              if(poznamka.kategorie.nazev == objekt_kategorie.nazev)
                {
                    poznamka.podoba_poznamky();
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Stiskni entr pokud chceš pokračovat :) ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
        }
        static void NapsatPoznamku()
        {
            Console.WriteLine("Zadej název poznámky: ");
            string Nazev = Console.ReadLine();
            Console.WriteLine("Zadej obsah poznámky: ");
            string Obsah = Console.ReadLine();
            Kategorie objekt_kategorie = Kategorie.ziskat_kategorii("Zadej název kategorie do které chceš přidat poznámku:");
            Console.WriteLine("Zadej datum vypršení: ");
            DateTime Datumvyprseni;
            if (DateTime.TryParse(Console.ReadLine(), out Datumvyprseni))
            {
                Console.ReadLine();
                Poznamka poznamka = new Poznamka(Nazev, Obsah, DateTime.Now,Datumvyprseni, objekt_kategorie);
                ListPoznamek.Add(poznamka);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Uspěšně přidáno :) ");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("Špatně zadané datum.");
                return;
            }
            

        }
        static void SmazatPoznamku()
        {
            int count = 0;
            foreach (Poznamka poznamka in ListPoznamek)
            {
                Console.WriteLine(count +") "+ poznamka.nazev);
                count = count + 1;
            }
            Console.WriteLine("Zadej název poznámky kterou chceš smazat: ");
            string smazat = Console.ReadLine();
            int Smazat = int.Parse(smazat);
            ListPoznamek.RemoveAt(Smazat);
        }
        static void UpravitPoznamku()
        {
            Poznamka objekt_poznamka = Poznamka.ziskat_pounamku("Jakou poznámku chceš upravit:");
            Console.WriteLine("Zadej nový název:");
            string novy_nazev = Console.ReadLine();
            objekt_poznamka.nazev = novy_nazev;
            Console.WriteLine("Zadej nový obsah:");
            string novy_obsah = Console.ReadLine();
            objekt_poznamka.obsah = novy_obsah;
            Kategorie nova_kategorie = Kategorie.ziskat_kategorii("Zadej novou kategorii:");
            objekt_poznamka.kategorie = nova_kategorie;

        }
        static void VytvoritKategorii()
        {
            Console.WriteLine("Zadej název kategorie: ");
            string Nazev = Console.ReadLine();
            Kategorie kategorie = new Kategorie(Nazev);
            ListKategorie.Add(kategorie);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Uspěšně přidáno :) ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void SmazatKategorii()
        {
            Kategorie objekt_kategorie = Kategorie.ziskat_kategorii("Jakou kategorii chceš smazat: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Opravdu chceš smazat tuhle kategorii? ");
            Console.ForegroundColor = ConsoleColor.White;
            string ano_ne = Console.ReadLine().ToUpper();
            if (ano_ne == "ANO")
            {
                ListKategorie.Remove(objekt_kategorie);
            }
            else
            {
                Getimput();
            }
        }
        static void UpravitKategorii()
        {
            Kategorie objekt_kategorie = Kategorie.ziskat_kategorii("Zadej název kategorie kterou chceš upravit:");
            Console.WriteLine("Zadej nový název:");
            string novy_nazev = Console.ReadLine();
            objekt_kategorie.nazev = novy_nazev;

        }
        static void Konec()
        {

        }
        static int Getimput()
        {
            while (true) 
            {
                Console.WriteLine("1)Zobrazit");
                Console.WriteLine("2)Napsat poznámku");
                Console.WriteLine("3)Smazat poznámku");
                Console.WriteLine("4)Upravit poznámku");
                Console.WriteLine("5)Vytvořit kategorii");
                Console.WriteLine("6)Smazat kategorii");
                Console.WriteLine("7)Upravit kategorii");
                Console.WriteLine("8)Konec");
                Console.WriteLine("Napiš číslo funkce kterou chceš využít: ");
                string radek = Console.ReadLine();
                int cisloFunkce;
                if (!int.TryParse(radek, out cisloFunkce))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Zadej znovu:");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                if (cisloFunkce > 8 || cisloFunkce < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Zadej znovu:");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                return cisloFunkce;
            }
        }
    }
}
