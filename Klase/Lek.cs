using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.Klase
{
    public class Lek
    {
        public int IDLeka { get; set; }
        public string Naziv { get; set; }
        public string Proizvodjac { get; set; }
        public string Jacina { get; set; }
        public string Doziranje { get; set; }
        public bool isDeleted { get; set; }
    }
}
