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
    public class ZubarPravila
    {
        private readonly ZubarDB _repo;

        public ZubarPravila()
        {
            _repo = new ZubarDB();
        }

        public List<Zubar> VratiSveZubare() => _repo.GetAll();
        public void DodajZubara(Zubar zubar) => _repo.Insert(zubar);
        public void IzmeniZubara(Zubar zubar) => _repo.Update(zubar);
        public void ObrisiZubara(int id) => _repo.Delete(id);
    }
}
