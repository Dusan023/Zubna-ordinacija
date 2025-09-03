using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.Klase
{
    public class Zubar 
    {
        public int IDZubara { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }

        public bool isDeleted { get; set; }
    }
}
