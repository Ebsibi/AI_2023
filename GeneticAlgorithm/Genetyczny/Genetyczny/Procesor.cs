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
        public List<int> listaIdZadań;
        public int zajętyCzas;

        public Procesor(List<Zadanie>? zadania = null, List<int> IDzadań = null, int zajętyCzas = 0)
        {
            this.listaZadań = zadania;
            this.listaIdZadań = IDzadań;
            this.zajętyCzas = zajętyCzas;
        }
    }
}
