using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.Klase
{//
    public class Pacijenti
{
    public int IDPacijenta { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string JMBG { get; set; }
    public string BrojTelefona { get; set; }
    public string Pol { get; set; }
    public string Alergije { get; set; }
    public bool Trudnoca { get; set; }
    public int BrojZuba { get; set; }
    public int IDZubara { get; set; }
}
}
