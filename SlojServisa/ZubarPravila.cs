using Klase;
using SlojPodataka;
using SlojPodataka.Klase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlojServisa //
{
    public class ZubarPravila
    {
        private readonly ZubarDB _repo;

        public ZubarPravila()
        {
            _repo = new ZubarDB();
        }

        public List<Zubar> VratiSveZubare() => _repo.GetAll();
        public Obavestenje DodajZubara(Zubar zubar)
        {
            var ispravan = ProveraPodatakaZaZubara(zubar);

            if (ispravan.Uspeh)
                _repo.Insert(zubar);

            return ispravan;
        }
        public Obavestenje IzmeniZubara(Zubar zubar)
        {
            var ispravan = ProveraPodatakaZaZubara(zubar);

            if (ispravan.Uspeh)
                _repo.Update(zubar);

            return ispravan;
        }
        public void ObrisiZubara(int id) => _repo.Delete(id);

        internal Obavestenje ProveraPodatakaZaZubara(Zubar zubar)
        {
            //****PRAVILA ZA INSERT/UPDATE ZA PACIJENTA****

            //Lista provera(pravila) da se vidi da li je sve ispravno
            //IF(uslov==true) -> pada


            //Na kraju ako nigde nije pao onda se vraća Obavestenje sa uspehom (true)

            //Samo treba da se prilagodi na WPF-u kada da proverava input-e i da prikaze poruku kada nađe grešku 
            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" }; 
        }
    }
}
