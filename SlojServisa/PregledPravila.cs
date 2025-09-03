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
        public Obavestenje DodajPregled(Pregled pregled)
        {
            var ispravan = PravilaZaDodavanjePreglede(pregled);

            if (ispravan.Uspeh)
                _repo.Insert(pregled);

            return ispravan;
        }

        public Obavestenje IzmeniPregled(Pregled pregled)
        {
            var ispravan = PravilaZaDodavanjePreglede(pregled);

            if (ispravan.Uspeh)
                _repo.Update(pregled);

            return ispravan;
        }
        public void ObrisiPregled(int id) => _repo.Delete(id);

        public List<Pregled> DajSvePregledePacijenta(int id) => _repo.GetTerminiFromPacijent(id);

        internal Obavestenje PravilaZaDodavanjePreglede(Pregled pregled)
        {
            if (string.IsNullOrWhiteSpace(pregled.Izvestaj.ToString()))
                return new Obavestenje
                {
                    Uspeh = false,
                    Poruka = "Datum sledeće pregleda ne može biti u prošlosti."
                };

            if (pregled.IDTermina <= 0)
                return new Obavestenje
                {
                    Uspeh = false,
                    Poruka = "Greska prilikom čitanja ID termina."
                };
            else
            {
                var proveriTermin = _repo.GetTerminWithPregled(pregled.IDTermina);

                if (proveriTermin == null)
                {
                    return new Obavestenje
                    {
                        Uspeh = false,
                        Poruka = "Datum sledeće pregleda ne može biti u prošlosti."
                    };
                }
            }

            if (pregled.IDLeka <= 0)
                return new Obavestenje
                {
                    Uspeh = false,
                    Poruka = "Greska prilikom čitanja ID leka."
                };




            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }

    }
}