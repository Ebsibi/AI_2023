using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_AI
{
    internal class Procesor
    {
        public List<Zadanie>? listaZadań;
        public int zajętyCzas;

        public Procesor(List<Zadanie>? zadania = null, int zajętyCzas = 0)
        {
            this.listaZadań = zadania;
            this.zajętyCzas = zajętyCzas;
        }
    }   
}
