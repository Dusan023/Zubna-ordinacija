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
        public Obavestenje DodajLek(Lek lek)
        {
            var ispravan = ProveraPodatakaZaLek(lek);

            if (ispravan.Uspeh)
                _repo.Insert(lek);

            return ispravan;
        }
        public Obavestenje IzmeniLek(Lek lek)
        {
            var ispravan = ProveraPodatakaZaLek(lek);

            if (ispravan.Uspeh)
                _repo.Update(lek);

            return ispravan;
        }
        public void ObrisiLek(int id) => _repo.Delete(id);

        internal Obavestenje ProveraPodatakaZaLek(Lek lek)
        {
            //****PRAVILA ZA INSERT/UPDATE ZA PACIJENTA****

            //Lista provera(pravila) da se vidi da li je sve ispravno
            //IF(uslov==true) -> pada


            //Na kraju ako nigde nije pao onda se vraća Obavestenje sa uspehom (true)

            //Samo treba da se prilagodi na WPF-u kada da proverava input-e i da prikaze poruku kada nađe grešku 
            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" }; // nema poruke
        }
    }
}
