using Klase;
using SlojPodataka;
using SlojPodataka.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojServisa
{
    public class TerminPravila
    {
        private readonly TerminDB _repo;

        public TerminPravila()
        {
            _repo = new TerminDB();
        }

        public List<Termin> VratiSveTermine() => _repo.GetAll();
        public void DodajTermin(Termin termin) => _repo.Insert(termin);
        public void IzmeniTermin(Termin termin) => _repo.Update(termin);
        public void ObrisiTermin(int id) => _repo.Delete(id);
    }
}
