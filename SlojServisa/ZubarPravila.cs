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
        public Obavestenje ObrisiZubara(Zubar zubar)
        {
            var ispravan = ProveraDaLiSeSmeObrisatiZubar(zubar);

            if (ispravan.Uspeh)
                _repo.SoftDelete(zubar.IDZubara);

            return ispravan;
        }

        internal Obavestenje ProveraPodatakaZaZubara(Zubar zubar)
        {
            if (string.IsNullOrWhiteSpace(zubar.Ime))
                return new Obavestenje { Uspeh = false, Poruka = "Ime zubara je obavezno." };

            if (!Regex.IsMatch(zubar.Ime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za ime zubara je 50 karaktera i ne može da sadrži brojeve" };

            if (string.IsNullOrWhiteSpace(zubar.Prezime))
                return new Obavestenje { Uspeh = false, Poruka = "Prezime zubara je obavezno." };

            if (!Regex.IsMatch(zubar.Prezime, @"^[A-Za-zČčĆćŠšĐđŽž ]{1,50}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Limit za prezime zubara je 50 karaktera i ne može da sadrži brojeve" };

            if (!Regex.IsMatch(zubar.JMBG, @"^\d{13}$"))
            {
                Console.WriteLine(zubar.JMBG.Length);
                return new Obavestenje { Uspeh = false, Poruka = "JMBG mora da ima 13 brojeva" };
            }
            else
            {
                //ako odgovara paternu
                Console.WriteLine(zubar.JMBG.Length);
                if (_repo.checkIfJMBGExists(zubar.JMBG))
                {
                    return new Obavestenje { Uspeh = false, Poruka = "Osoba sa ovim JMBG:" + zubar.JMBG + " već postoji" };
                }
            }
            if (string.IsNullOrWhiteSpace(zubar.BrojTelefona))
                return new Obavestenje { Uspeh = false, Poruka = "Broj telefona zubara je obavezan." };

            if (!Regex.IsMatch(zubar.BrojTelefona, @"^[\d\+\-\(\)\s]{1,20}$"))
                return new Obavestenje { Uspeh = false, Poruka = "Primeri kako treba da izgleda broj telefona: - +381641234567\r\n- \r\n, - 064-123-4567\r\n- \r\n, - 0631234567890123456\r\n" };

            if (string.IsNullOrWhiteSpace(zubar.Email))
                return new Obavestenje { Uspeh = false, Poruka = "Email zubara je obavezan." };

            if (!Regex.IsMatch(zubar.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return new Obavestenje { Uspeh = false, Poruka = "Email adresa nije validna." };

            if(_repo.checkIfEmailForZubarExists(zubar.Email))
                return new Obavestenje { Uspeh = false, Poruka = "Email za ovog zubara već postoji" };


            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je dodat entitet" };
        }

        internal Obavestenje ProveraDaLiSeSmeObrisatiZubar(Zubar zubar)
        {
            /*if (_repo.GetAppointmentCountFromDentist(zubar.IDZubara) > 0)
            {
                return new Obavestenje { Uspeh = false, Poruka = "Moraju se svi zakazani termini rasporediti drugim zubarima da bi se ovaj zubar obrisao" };
            }*/

            return new Obavestenje { Uspeh = true, Poruka = "Uspešno je obrisan zubar" };
        }
    }
}
