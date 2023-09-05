using Projekt_AI;
using System;
namespace Projekt_AI
{
    class Program
    {
        static List<Procesor> F(List<Zadanie> zadania, int liczbaProcesorów) // ta funkcja ma za zadanie obliczyć koszt zadań sumarycznie
        {
            List<Procesor> maszyna = new List<Procesor>(); // deklaracja kolekcji, gdzie przechowywane będą procesory
            List<Zadanie> kopiaZadania = new List<Zadanie>(); // w tej funkcji będę pracował na kopii listy zadań
            foreach (Zadanie zadanie in zadania) { kopiaZadania.Add(zadanie.Clone()); } // głęboka kopia listy zadań
            List<Zadanie> kolejka = new List<Zadanie>(); // do tej listy trafią te zadania, które muszą poczekać na rozpoczęcie się innego zadania
            int ilośćZadań = zadania.Count();
            for (int i = 0; i < liczbaProcesorów; i++) maszyna.Add(new Procesor(new List<Zadanie>() { }, new List<int>() { }, 0));
            int minimalnyCzas = 0;
            do
            {
                foreach (Zadanie zadanie in kopiaZadania) // dla każdego zadania z listy przeglądam każdy procesor i sprawdzam czy opłaca się lub czy można uruchomić na nim dane zadanie
                {
                    foreach (Procesor procesor in maszyna)
                    {
                        minimalnyCzas = maszyna.Min(procesor => procesor.zajętyCzas);

                        if ((procesor.zajętyCzas != minimalnyCzas) && (zadanie.PoprzednieZadanie.HasValue) && (zadanie.PrzypisaneZadanie == false) && (procesor.listaIdZadań.Contains((int)zadanie.PoprzednieZadanie)))
                        {
                            procesor.listaZadań.Add(zadanie);
                            kolejka.Remove(zadanie);
                            procesor.listaIdZadań.Add(zadanie.IdZadania);
                            zadanie.PrzypisaneZadanie = true;
                            procesor.zajętyCzas += zadanie.CzasWykonaniaZadania;
                            ilośćZadań--;
                            break;
                        }
                        else if (procesor.zajętyCzas != minimalnyCzas)
                            continue;
                        else
                        {
                            if (!(zadanie.PoprzednieZadanie.HasValue) && (zadanie.PrzypisaneZadanie == false))
                            {
                                procesor.listaZadań.Add(zadanie);
                                kolejka.Remove(zadanie);
                                procesor.listaIdZadań.Add(zadanie.IdZadania);
                                zadanie.PrzypisaneZadanie = true;
                                procesor.zajętyCzas += zadanie.CzasWykonaniaZadania;
                                ilośćZadań--;
                                break;
                            }
                            else if ((zadanie.PoprzednieZadanie.HasValue) && (zadanie.PrzypisaneZadanie == false) && (procesor.listaIdZadań.Contains((int)zadanie.PoprzednieZadanie)))
                            {
                                //foreach (Zadanie zadanieZListy in procesor.listaZadań.ToList())
                                //{
                                //    if (zadanieZListy.IdZadania == zadanie.PoprzednieZadanie)
                                //    {
                                procesor.listaZadań.Add(zadanie);
                                kolejka.Remove(zadanie);
                                procesor.listaIdZadań.Add(zadanie.IdZadania);
                                zadanie.PrzypisaneZadanie = true;
                                procesor.zajętyCzas += zadanie.CzasWykonaniaZadania;
                                ilośćZadań--;
                                break;
                                //    }
                                //}
                            }
                            else
                            {
                                if (kolejka.Contains(zadanie))
                                    continue;
                                else
                                    kolejka.Add(zadanie);
                            }
                        }
                    }
                }
                kopiaZadania.Clear();
                foreach (Zadanie zadanie in kolejka)
                {
                    kopiaZadania.Add(zadanie.Clone());
                }
                kolejka.Clear();
                ilośćZadań = kopiaZadania.Count();
                //for (int i = 0; i < liczbaProcesorów; i++)
                //{
                //    Console.WriteLine("Procesor {0}: Czas sumaryczny: {1}", i, maszyna[i].zajętyCzas);
                //    foreach (Zadanie zadanie in maszyna[i].listaZadań)
                //        Console.WriteLine("\t{0} | {1} | {2}", zadanie.IdZadania, zadanie.CzasWykonaniaZadania, zadanie.PoprzednieZadanie);
                //}
            }
            while (ilośćZadań > 0);
            return maszyna;
            //return x * x * x * x - 2 * x * x + x - 5;   // badana funkcja, x^4 - 2x^2 + x - 5
        }

        static void Wyzarzanie(List<Zadanie> Zadania, int liczbaProcesorów)
        {

            double T = 100.0;                           // zmienna obrazująca temperaturę, jedna ze zmiennych wejściowych
            double Tmin = 0.01;                         // zmienna będąca kryterium zatrzymania
            double wspolczynnikChlodzenia = 0.99;       // im bliżej 1, tym wolniej "stygnie"
                                                        //double minimumPrzedzialu = -10;             // wpisane na sztywno, można potraktować jako domyślne
                                                        //double maksimumPrzedzialu = 10;             // jw
            Random random = new Random();
            List<Zadanie> nowaListaZadań = new List<Zadanie>();
            foreach (Zadanie zadanie in Zadania) { nowaListaZadań.Add(zadanie.Clone()); }
            //double x = random.NextDouble() * (maksimumPrzedzialu - minimumPrzedzialu) + minimumPrzedzialu;
            // w tej wersji max x = 10, min x = -10, zgadza się
            nowaListaZadań.Shuffle();

            List<Procesor> maszynaX = new List<Procesor>();
            List<Procesor> maszynaNoweX = new List<Procesor>();
            List<Zadanie> minimumFunkcji = Zadania;
            List<Procesor> najlepszaAktualnieMaszyna = F(Zadania, liczbaProcesorów);
            int wartoscWMinimum = najlepszaAktualnieMaszyna.Max(procesor => procesor.zajętyCzas);
            int wartoscMaszynyX = 0;
            int wartoscMaszynyNoweX = 0;


            while (T > Tmin)
            {
                //double noweX = x + random.NextDouble() * (maksimumPrzedzialu - minimumPrzedzialu) + minimumPrzedzialu;
                //if (noweX < minimumPrzedzialu) x = minimumPrzedzialu;
                //if (noweX > maksimumPrzedzialu) x = maksimumPrzedzialu;
                nowaListaZadań.Shuffle();
                maszynaX = F(Zadania, liczbaProcesorów);
                maszynaNoweX = F(nowaListaZadań, liczbaProcesorów);
                wartoscMaszynyX = maszynaX.Max(procesor => procesor.zajętyCzas);
                wartoscMaszynyNoweX = maszynaNoweX.Max(procesor => procesor.zajętyCzas);

                int roznica = wartoscMaszynyNoweX - wartoscMaszynyX;

                if (roznica < 0 || Math.Exp(-roznica / T) > random.NextDouble())
                {

                    Zadania = nowaListaZadań;
                }

                if (wartoscMaszynyX < wartoscWMinimum)
                {
                    minimumFunkcji = Zadania;
                    wartoscWMinimum = wartoscMaszynyX;
                }

                T *= wspolczynnikChlodzenia;

                //for (int i = 0; i < liczbaProcesorów; i++)
                //{
                //    Console.WriteLine("Procesor {0}: Czas sumaryczny: {1}", i, maszynaX[i].zajętyCzas);
                //    foreach (Zadanie zadanie in maszynaX[i].listaZadań)
                //        Console.WriteLine("\t{0} | {1} | {2}", zadanie.IdZadania, zadanie.CzasWykonaniaZadania, zadanie.PoprzednieZadanie);
                //}

                //Thread.Sleep(5000);
                //Console.Clear();
            }

            for (int i = 0; i < liczbaProcesorów; i++)
            {
                Console.WriteLine("Procesor {0}: Czas sumaryczny: {1}", i, maszynaX[i].zajętyCzas);
                foreach (Zadanie zadanie in maszynaX[i].listaZadań)
                    Console.WriteLine("\t{0} | {1} | {2}", zadanie.IdZadania, zadanie.CzasWykonaniaZadania, zadanie.PoprzednieZadanie);
            }
            //Console.WriteLine("Minimum: {0}, Wartość w tym minimum = {1}", minimumFunkcji, wartoscWMinimum);
        }

        static void Main(string[] args)
        {
            Zadanie przeglądarka = new Zadanie(0, 6);
            Zadanie edytorTekstu = new Zadanie(1, 7);
            Zadanie graKomputerowa1 = new Zadanie(2, 8);
            Zadanie graPrzeglądarkowa1 = new Zadanie(3, 2, 0);
            Zadanie otwarcieDokumentuTekstowego = new Zadanie(4, 2, 1);
            Zadanie odtwarzaczMuzyki = new Zadanie(5, 4);
            Zadanie klientPocztyWPrzeglądarce = new Zadanie(6, 4, 0);
            Zadanie odtworzeniePiosenki1 = new Zadanie(7, 2, 5);
            Zadanie odtworzeniePiosenki2 = new Zadanie(8, 2, 7);
            Zadanie odtworzeniePiosenki3 = new Zadanie(9, 2, 8);
            Zadanie arkuszKalkulacyjny = new Zadanie(10, 7);
            Zadanie otwarcieArkuszaKalkulacyjnego1 = new Zadanie(11, 3, 10);
            Zadanie kalkulator = new Zadanie(12, 1);

            List<Zadanie> zadania = new List<Zadanie>{
            przeglądarka,
            edytorTekstu,
            graKomputerowa1,
            graPrzeglądarkowa1,
            otwarcieDokumentuTekstowego,
            odtwarzaczMuzyki,
            klientPocztyWPrzeglądarce,
            odtworzeniePiosenki1,
            odtworzeniePiosenki2,
            odtworzeniePiosenki3,
            arkuszKalkulacyjny,
            otwarcieArkuszaKalkulacyjnego1,
            kalkulator
        };
            Wyzarzanie(zadania, 4);
        }


    }

    public static class Extensions
    {
        private static Random rand = new Random();

        public static void Shuffle<T>(this IList<T> values)
        {
            int n = values.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = values[k];
                values[k] = values[n];
                values[n] = value;
            }

        }
    }
}