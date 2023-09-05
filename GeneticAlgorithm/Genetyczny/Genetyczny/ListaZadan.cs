using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_AI
{
    internal class Zadanie : ICloneable
    {
        public int IdZadania;                 // jednoznaczny indentyfikator zadania
        public int CzasWykonaniaZadania;      // czas wykonania zadania wyrażony w sekundach
        public int? PoprzednieZadanie;         // ID zadania wcześniejszego, takiego, które musi wykonać się przed aktualnym zadaniem
                                               // de facto zadanie poprzednie i aktualne mogłyby być jednym zadaniem, w sensie sumarycznego czasu
        public bool PrzypisaneZadanie;         // flaga potrzebna do tego, aby jakieś zadanie przypadkiem nie trafiło dwa razy do przetwarzania

        public Zadanie(int IdZadania, int CzasWykonaniaZadania, int? PoprzednieZadanie = null, bool PrzypisaneZadanie = false)
        {
            this.IdZadania = IdZadania;
            this.CzasWykonaniaZadania = CzasWykonaniaZadania;
            this.PoprzednieZadanie = PoprzednieZadanie;
            this.PrzypisaneZadanie = PrzypisaneZadanie;
        }

        // W zadaniu będę kopiował bazową listę, ponieważ później chcę ułożyć zadania, które w niej występują w sposób losowy
        // Potrzebuję głębokiej kopii, dlatego implementuję interfejs ICloneable
        // Muszę zadeklarować w ten sposób, ponieważ implementując po ICLoneable muszę zwracać Object,
        // natomiast nie mam jawnego rzutowania z Zadanie na Object
        Object ICloneable.Clone()
        {
            return new Zadanie(this.IdZadania, this.CzasWykonaniaZadania, this.PoprzednieZadanie, this.PrzypisaneZadanie);
        }

        public Zadanie Clone()
        {
            return new Zadanie(this.IdZadania, this.CzasWykonaniaZadania, this.PoprzednieZadanie, this.PrzypisaneZadanie);
        }
    }
}
