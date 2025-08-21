using SlojPodataka.Klase;
using SlojPodataka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klase;

namespace SlojServisa
{
    public class PregledPravila
    {
        
            private readonly PregledDB _repo;

            public PregledPravila()
            {
                _repo = new PregledDB();
            }

            public List<Pregled> VratiSvePreglede() => _repo.GetAll();
            public void DodajPregled(Pregled pregled) => _repo.Insert(pregled);
            public void IzmeniPregled(Pregled pregled) => _repo.Update(pregled);
            public void ObrisiPregled(int id) => _repo.Delete(id);
        
    }
}
