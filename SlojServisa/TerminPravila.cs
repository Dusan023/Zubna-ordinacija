using Klase;
using SlojPodataka;
using SlojPodataka.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
        public Obavestenje DodajTermin(Termin termin)
        {
            var ispravan = ProveraPodatakaZaTermin(termin);

            if (ispravan.Uspeh)
                _repo.Insert(termin);

            return ispravan;
        }

        public Obavestenje IzmeniTermin(Termin termin)
        {
            var ispravan = ProveraPodatakaZaTermin(termin);

            if (ispravan.Uspeh)
                _repo.Update(termin);

            return ispravan;
        }

        public void ObrisiTermin(int id)
        {
            //xx
        }

        public List<Termin> DajSveTerminePacijenta(int id) => _repo.GetTerminiFromPacijent(id);

        internal Obavestenje ProveraPodatakaZaTermin(Termin termin)
        {
            if (termin.Datum < DateTime.Today)
                return new Obavestenje { Uspeh = false, Poruka = "Ne može pregled u prošlom vremenu" };

            if (termin.Datum > DateTime.Today.AddYears(1))
                return new Obavestenje { Uspeh = false, Poruka = "Ne može pregled da se zakaze vise od godinu dana" };

            if (!Regex.IsMatch(termin.Vreme.ToString(), @"^(0[8-9]|1[0-9]|20):[0-5][0-9]$"))
                return new Obavestenje { Uspeh = false, Poruka = "Vreme mora biti u formatu HH:mm između 08:00 i 20:59." };

            if (string.IsNullOrWhiteSpace(termin.VrstaUsluge))
                return new Obavestenje { Uspeh = false, Poruka = "Morate da upisite sta ste radili!" };

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }
    }
}
