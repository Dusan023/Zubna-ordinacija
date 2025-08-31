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
    public class PacijentPravila
    {
        private readonly PacijentiDB _repo;





        public PacijentPravila()
        {
            _repo = new PacijentiDB();
        }

        public List<Pacijenti> VratiSvePacijente() => _repo.GetAll();
        public Obavestenje DodajPacijenta(Pacijenti pacijent)
        {
            var ispravan = ProveraPodatakaPacijenta(pacijent);

            if (ispravan.Uspeh)
                _repo.Insert(pacijent);


            
            return ispravan;

        }
        public Obavestenje IzmeniPacijenta(Pacijenti pacijent)
        {
            var ispravan = ProveraPodatakaPacijenta(pacijent);

            if (ispravan.Uspeh)
                _repo.Update(pacijent);

            return ispravan;
        }
        public void ObrisiPacijenta(int id) => _repo.Delete(id);

        //----------------------------------------------------------------------------------------

        internal Obavestenje ProveraPodatakaPacijenta(Pacijenti pacijent)
        {
            //****PRAVILA ZA INSERT/UPDATE ZA PACIJENTA****

            if (string.IsNullOrWhiteSpace(pacijent.Ime))
                return new Obavestenje { Uspeh = false, Poruka = "Ime pacijenta je obavezno." };

            if (!Regex.IsMatch(pacijent.Ime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za ime pacijenta je 50 karaktera i ne može da sadrži brojeve" };

            if (string.IsNullOrWhiteSpace(pacijent.Prezime))
                return new Obavestenje { Uspeh = false, Poruka = "Prezime pacijenta je obavezno." };

            if (!Regex.IsMatch(pacijent.Prezime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za prezime pacijenta je 50 karaktera i ne može da sadrži brojeve" };

            if (!Regex.IsMatch(pacijent.JMBG, @"^\d{13}$"))
            {
                Console.WriteLine(pacijent.JMBG.Length);
                return new Obavestenje { Uspeh = false, Poruka = "JMBG mora da ima 13 brojeva" };
            }
            else
            {
                //ako odgovara paternu
                Console.WriteLine(pacijent.JMBG.Length);
                if (_repo.checkIfJMBGExists(pacijent.JMBG))
                {
                    return new Obavestenje { Uspeh = false, Poruka = "Osoba sa ovim JMBG:" + pacijent.JMBG + " već postoji" };
                }
            }

            if (string.IsNullOrWhiteSpace(pacijent.BrojTelefona))
                return new Obavestenje { Uspeh = false, Poruka = "Broj telefona pacijenta je obavezan." };

            if (!Regex.IsMatch(pacijent.BrojTelefona, @"^[\d\+\-\(\)\s]{1,20}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Primeri kako treba da izgleda broj telefona: - +381641234567\r\n- \r\n, - 064-123-4567\r\n- \r\n, - 0631234567890123456\r\n" };

            if (string.IsNullOrWhiteSpace(pacijent.Pol))
                return new Obavestenje { Uspeh = false, Poruka = "Pol pacijenta je obavezan." };

            // RAZMISLI PROVERI TRUDNOCE I PISANJE
            

            if (pacijent.Pol == "M" && pacijent.Trudnoca==true)
                return new Obavestenje { Uspeh = false, Poruka = "Kako?!" };

            if(pacijent.Trudnoca == null)
                return new Obavestenje { Uspeh = false, Poruka = "Odgovor za trudnoću je ne prepoznatljiv, ovu su dostupni odgovori: /n" +
                    "\"da\", \"trudna\", \"jeste\", \"yes\", \"true\", \"1\" ili \"ne\", \"nije\", \"no\", \"false\", \"0\", \"/\""
                };

            if (pacijent.BrojZuba <= 0)
                return new Obavestenje { Uspeh = false, Poruka = "-Zuba?!" };

            //Moguće je zbog geneckih poremećaja da osoba ima preko 50 zuba

            if (pacijent.IDZubara <= 0)
                return new Obavestenje { Uspeh = false, Poruka = "Zubar mora biti izabran." };

            
            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat pacijent" }; // nema poruke
        }
    }
}
