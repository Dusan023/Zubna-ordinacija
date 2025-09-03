using Klase.Pomocne_klase;
using Klase;
using SlojServisa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zubna_Ordinacija_WPF.Prozori
{
    /// <summary>
    /// Interaction logic for RadnaPovršinaNaPacijenta.xaml
    /// </summary>
    public partial class RadnaPovršinaNaPacijenta : Window
    {

        private readonly PacijentPravila _pacijentRepo;
        private readonly ZubarPravila _zubarRepo;
        private readonly InputTrudnocaCheck _proveriUnosZaTrudnocu;
        private readonly TerminPravila _terminiRepo;
        private readonly PregledPravila _preglediRepo;

        private readonly Obavestenje Obavesti;
        public RadnaPovršinaNaPacijenta(string JMBG)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _pacijentRepo = new PacijentPravila();
            _zubarRepo = new ZubarPravila();
            _terminiRepo = new TerminPravila();
            _preglediRepo = new PregledPravila();
            Obavesti = new Obavestenje();
            binDataGrid(JMBG);
        }

        private void binDataGrid(string nadjiPoJmbg)
        {
            //string x = "1234567890123";
            var pacijent = _pacijentRepo.vratiPacijentaPoJmbg(nadjiPoJmbg);
            ImeLabel.Content = "Ime: " + pacijent.Ime;
            PrezimeLabel.Content = "Prezime: " + pacijent.Prezime;
            JmbgLabel.Content = "JMBG: " + pacijent.JMBG;
            TelefonLabel.Content = "Telefon: " + pacijent.BrojTelefona;
            PolLabel.Content = "Pol: " + pacijent.Pol;
            AlergijaLabel.Content = "Alergija: " + pacijent.Alergije;
            TrudnocaLabel.Content = "Trudnoća: " + pacijent.Trudnoca;
            BrojZubaLabel.Content = "Broj zuba: " + pacijent.BrojZuba;

           

            //Svi termini svih pacijenata;
            var termini= _terminiRepo.DajSveTerminePacijenta(pacijent.IDPacijenta);
            dgTermini.ItemsSource = termini;
 
            List<Pregled> pregledi = new List<Pregled>();

            // Svi termini svih pacijenata
            foreach (var termin in termini)
            {
                var preglediTermina = _preglediRepo.DajSvePregledePacijenta(termin.IDTermina);
                pregledi.AddRange(preglediTermina);
            }
            dgPregledi.ItemsSource = pregledi;

            if (termini.Count == 0)
            {
                MessageBox.Show("Trenutno nema ništa uneto od termina i pregleda za ovog pacijenta", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void dgPregledi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pregled = dgPregledi.SelectedItem as Pregled;
            if (pregled == null) return;

            txtIzvestaj.SelectedText = pregled.Izvestaj;
        }
    }
}
