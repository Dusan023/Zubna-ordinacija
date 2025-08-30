using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klase.Pomocne_klase
{
    public class InputTrudnocaCheck
    {
        private readonly string[] pozitivniOdgovori = { "da", "trudna", "jeste", "yes", "true", "1" };
        private readonly string[] negativniOdgovori = { "ne", "nije", "no", "false", "0", "/" };

        public bool? proveraUnosaZaTrudnocu(string unos)
        {
            if (string.IsNullOrWhiteSpace(unos))
                return false;

            string normalized = unos.Trim().ToLower();

            if (pozitivniOdgovori.Contains(normalized))
                return true;

            if (negativniOdgovori.Contains(normalized))
                return false;

            // Ako nije eksplicitno ni  jedno ni drugo — vratiti poruku da unese ispravan oblik za trudnocu ili da nije trudna
            return null;
        }

    }
}
