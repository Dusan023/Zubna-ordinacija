using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase
{
    public class Termin
    {
        public int IDTermina { get; set; }       // ID termina
        public DateTime Datum { get; set; }      // Datum termina
        public string VrstaUsluge { get; set; }  // Vrsta usluge
        public int IDPacijenta { get; set; }     // ID pacijenta
        public int IDZubara { get; set; }        // ID zubara
        public TimeSpan Vreme { get; set; }      // Vreme termina
    }
}
