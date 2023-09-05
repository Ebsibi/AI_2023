using GAF;
using GAF.Extensions;
using GAF.Operators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_AI;
using System;
namespace Projekt_AI
{
    class Genetyczny
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

        static void AlgorytmGenetyczny(List<Zadanie> Zadania, int liczbaProcesorów)
        {
            const int Populacja = 100;
            List<Zadanie> zadania = new List<Zadanie>();
            var population = new Population();
            // dodanie pracy do populacji
            List<Zadanie> PopulacjaZadań = new List<Zadanie>() { };
            foreach (Zadanie zadanie in Zadania)
            {
                //if (!(zadanie.PoprzednieZadanie.HasValue))
                    PopulacjaZadań.Add(zadanie);
            }
                        
            //tworzenie chromosomów
            for (var p = 0; p < Populacja; p++)
            {
                var chromosome = new Chromosome();
                foreach (var c in PopulacjaZadań)
                    chromosome.Genes.Add(new Gene(c));
                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            //Tworzenie operatorów elit
            var elite = new Elite(5);

            //Tworzenie operatora mieszania
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            //Tworzenie operatora mutacji
            var mutate = new SwapMutate(0.02);

            //Tworzenie algorytmu genetycznego
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            //Dodaj operatory
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            //odpal algorytm
            ga.Run(Terminate);

            var fittest = ga.Population.GetTop(1)[0];

            List<Zadanie> zadania2 = new List<Zadanie>() { };

            foreach (var gene in fittest.Genes)
                zadania2.Add((Zadanie)gene.ObjectValue);

            List<Procesor> maszynaX = new List<Procesor>();
            maszynaX = F(zadania2, liczbaProcesorów);

            for (int i = 0; i < liczbaProcesorów; i++)
            {
                Console.WriteLine("Procesor {0}: Czas sumaryczny: {1}", i, maszynaX[i].zajętyCzas);
                foreach (Zadanie zadanie in maszynaX[i].listaZadań)
                    Console.WriteLine("\t{0} | {1} | {2}", zadanie.IdZadania, zadanie.CzasWykonaniaZadania, zadanie.PoprzednieZadanie);
            }
            //Console.WriteLine("Minimum: {0}, Wartość w tym minimum = {1}", minimumFunkcji, wartoscWMinimum);
        }

        // Wyznaczanie najlepszego
        static double CalculateFitness(Chromosome chromosome)
        {
            var minTime = CalculateMinTime(chromosome);
            var fitness = 10 / minTime;
            return fitness > 1.0 ? 1.0 : fitness;

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
            AlgorytmGenetyczny(zadania, 4);
        }

        

        //Generowanie nowego harmonogramu na podstawie genów i zwrócenie czasu wykonywania
        static double CalculateMinTime(Chromosome chromosome)
        {
            List<Zadanie> zadania = new List<Zadanie>();
            foreach (var gene in chromosome.Genes)
                zadania.Add((Zadanie)gene.ObjectValue);
            List<Procesor> maszynaX = new List<Procesor>();
            maszynaX = F(zadania, 4);
            return maszynaX.Max(procesor => procesor.zajętyCzas);
        }

        // Masowe wymieranie populacji
        static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 100;
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