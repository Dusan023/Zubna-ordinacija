using SlojPodataka;
using SlojPodataka.Klase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojServisa
{
    public class PacijentPravila
    {
        private readonly PacijentiDB _repo;

        public PacijentPravila()
        {
            _repo = new PacijentiDB();
        }

        public List<Pacijenti> VratiSvePacijente() => _repo.GetAll();
        public void DodajPacijenta(Pacijenti pacijent) => _repo.Insert(pacijent);
        public void IzmeniPacijenta(Pacijenti pacijent) => _repo.Update(pacijent);
        public void ObrisiPacijenta(int id) => _repo.Delete(id);
    }
}
