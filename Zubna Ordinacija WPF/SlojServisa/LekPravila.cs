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
    public class LekPravila
    {
        private readonly LekDB _repo;

        public LekPravila()
        {
            _repo = new LekDB();
        }

        public List<Lek> VratiSveLekove() => _repo.GetAll();
        public void DodajLek(Lek lek) => _repo.Insert(lek);
        public void IzmeniLek(Lek lek) => _repo.Update(lek);
        public void ObrisiLek(int id) => _repo.Delete(id);
    }
}
