using Microsoft.IdentityModel.Tokens;
using SlojPodataka.Klase;
using SlojServisa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using Klase.Pomocne_klase;
using System.Windows.Threading;
using Klase;
using System.Text.RegularExpressions;
using System.Net;

namespace Zubna_Ordinacija_WPF.Prozori
{
    /// <summary>
    /// Interaction logic for Pacijenti.xaml
    /// </summary>
    public partial class Pacijenti : Window
    {

        private readonly PacijentPravila _pacijentRepo;
        private readonly ZubarPravila _zubarRepo;
        private readonly InputTrudnocaCheck _proveriUnosZaTrudnocu;
        private DispatcherTimer debounceTimer;

        private readonly Obavestenje Obavesti; 
        public Pacijenti()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _pacijentRepo= new PacijentPravila();
            _zubarRepo = new ZubarPravila();
            _proveriUnosZaTrudnocu = new InputTrudnocaCheck();
            Obavesti = new Obavestenje();
            binDataGrid();
        }

        private void ButtonNazad_Click(object sender, RoutedEventArgs e)
        {
            Meni menipage = new Meni();
            menipage.Show();
            this.Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedItem is SlojPodataka.Klase.Pacijenti pacijent)
            {
                TextboxIdPacijenta.Text = pacijent.IDPacijenta.ToString();
                TextboxIme.Text = pacijent.Ime;
                TextboxPrezime.Text = pacijent.Prezime;
                TextboxJMBG.Text = pacijent.JMBG;
                TextboxBrojTelefona.Text = pacijent.BrojTelefona;
                ComboboxPol.Text = pacijent.Pol;
                TextboxAlergije.Text = pacijent.Alergije;
                TextboxTrudnoca.Text = (bool) pacijent.Trudnoca ? "Da" : "Ne"; // ili "1"/"" po potrebi
                TextboxBrojZuba.Text = pacijent.BrojZuba.ToString();
                ComboboxZubar.SelectedValue = pacijent.IDZubara;
            }
        }
        private void binDataGrid()
        {
            DataGrid.ItemsSource = _pacijentRepo.VratiSvePacijente();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            SlojPodataka.Klase.Pacijenti pacijent = new SlojPodataka.Klase.Pacijenti
            {
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Pol = ComboboxPol.Text,
                Alergije = TextboxAlergije.Text,
                Trudnoca = _proveriUnosZaTrudnocu.proveraUnosaZaTrudnocu(TextboxTrudnoca.Text),  //paziti prilikom ispravke
                BrojZuba = int.Parse(TextboxBrojZuba.Text),
                IDZubara = int.Parse(ComboboxZubar.SelectedValue.ToString())
            };

            var poruka = _pacijentRepo.DodajPacijenta(pacijent);
            if(!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            var p = new SlojPodataka.Klase.Pacijenti
            {
                IDPacijenta = Convert.ToInt32(TextboxIdPacijenta.Text),
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Pol = ComboboxPol.Text,
                Alergije = TextboxAlergije.Text,
                Trudnoca = _proveriUnosZaTrudnocu.proveraUnosaZaTrudnocu(TextboxTrudnoca.Text),  //paziti prilikom ispravke
                BrojZuba = int.Parse(TextboxBrojZuba.Text),
                IDZubara = int.Parse(ComboboxZubar.SelectedValue.ToString())
            };

            //MessageBox.Show(TextboxIdPacijenta.Text);

            var poruka = _pacijentRepo.IzmeniPacijenta(p);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ponistiUnosTxt()
        {
            TextboxIdPacijenta.Text = "";
            TextboxIme.Text = "";
            TextboxPrezime.Text = "";
            TextboxJMBG.Text = "";
            TextboxBrojTelefona.Text = "";
            ComboboxPol.Text = "";
            TextboxAlergije.Text = "";
            TextboxTrudnoca.Text = "";
            TextboxBrojZuba.Text = "";
            ComboboxZubar.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(TextboxIdPacijenta.Text);
            _pacijentRepo.ObrisiPacijenta(id);
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ComboboxZubar_Loaded(object sender, RoutedEventArgs e)
        {
            var zubari = _zubarRepo.VratiSveAktivneZubare();
            ComboboxZubar.ItemsSource = zubari;
            ComboboxZubar.DisplayMemberPath = "Ime";     // prikaz
            ComboboxZubar.SelectedValuePath = "IDZubara"; // vrednost
        }

        private void pretragaPacijenataSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            // Ako tajmer nije već kreiran, napravi ga
            if (debounceTimer == null)
            {
                debounceTimer = new DispatcherTimer();
                debounceTimer.Interval = TimeSpan.FromMilliseconds(750); // debounce vreme

                debounceTimer.Tick += (s, es) =>
                {
                    debounceTimer.Stop();

                    string tekst = pretragaPacijenataSearch.Text.Trim();
                    var view = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);

                    if (string.IsNullOrWhiteSpace(tekst))
                    {
                        view.Filter = null;
                        return;
                    }

                    if (Regex.IsMatch(tekst, @"^\d{13}$"))
                    {
                        view.Filter = item =>
                        {
                            var pacijent = item as SlojPodataka.Klase.Pacijenti;
                            return pacijent != null && pacijent.JMBG == tekst;
                        };
                    }
                    else if (Regex.IsMatch(tekst, @"^[\p{L}\s-]+$"))
                    {
                        string[] delovi = tekst.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        view.Filter = item =>
                        {
                            var pacijent = item as SlojPodataka.Klase.Pacijenti;
                            if (pacijent == null) return false;

                            string ime = pacijent.Ime?.ToLowerInvariant() ?? "";
                            string prezime = pacijent.Prezime?.ToLowerInvariant() ?? "";

                            return delovi.All(deo =>
                                ime.Contains(deo.ToLowerInvariant()) || prezime.Contains(deo.ToLowerInvariant()));
                        };
                    }
                    else
                    {
                        MessageBox.Show("Unos mora biti JMBG (13 cifara) ili ime/prezime (slova, razmaci, crtice).",
                                        "Neispravan unos", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                };
            }

            // Restartuj tajmer na svako kucanje
            debounceTimer.Stop();
            debounceTimer.Start();


        }

        private void KartonButton_Click(object sender, RoutedEventArgs e)
        {
            var proslediJMBG = TextboxJMBG.Text?.Trim();

            if (string.IsNullOrWhiteSpace(proslediJMBG))
            {
                MessageBox.Show("Morate uneti JMBG pacijenta.",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pacijentRepo = new PacijentPravila();
            var pacijent = pacijentRepo.vratiPacijentaPoJmbg(proslediJMBG);

            if (pacijent == null)
            {
                MessageBox.Show("Pacijent sa unetim JMBG-om ne postoji u bazi.",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ako je sve ok – otvori karton
            var prozor = new RadnaPovršinaNaPacijenta(proslediJMBG);
            prozor.Show();
        }

    }
}
