using Klase;
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

namespace Zubna_Ordinacija_WPF.Prozori
{
    /// <summary>
    /// Interaction logic for Termini.xaml
    /// </summary>
    public partial class Termini : Window
    {
        private readonly TerminPravila _terminRepo;
        private readonly ZubarPravila _zubarRepo;
        private readonly PacijentPravila _pacijentRepo;

        public Termini()
        {
            InitializeComponent();
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _terminRepo = new TerminPravila();
            _zubarRepo = new ZubarPravila();
            _pacijentRepo = new PacijentPravila();
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
            if (DataGrid.SelectedItem is Termin termin)
            {
                TextboxIdTermina.Text = termin.IDTermina.ToString();
                DatepickerDatum.Text = termin.Datum.ToString("yyyy-MM-dd");
                TextboxVreme.Text = termin.Vreme.ToString(@"hh\:mm");
                TextboxVrstaUsluge.Text = termin.VrstaUsluge;

                ComboboxPacijent.SelectedValue = termin.IDPacijenta;
                ComboboxZubar.SelectedValue = termin.IDZubara;

                if(!_terminRepo.ProveriAktivnostZubara(termin.IDZubara))
                {
                    MessageBox.Show(
                    "Trenutno je zubar neaktivan",
                    "Informacija",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                }
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _terminRepo.VratiSveTermine();

        }

        private void ponistiUnosTxt()
        {
            TextboxIdTermina.Text = "";
            DatepickerDatum.Text = "";
            TextboxVreme.Text = "";
            TextboxVrstaUsluge.Text = "";
            ComboboxPacijent.Text = "";
            ComboboxZubar.Text = "";

        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Pacijent: {ComboboxPacijent.SelectedValue}, Zubar: {ComboboxZubar.SelectedValue}");
            if(ValidirajUnosTermina() == false)
            {
                MessageBox.Show("Niste uneli sve podatke za termin!");
                return;
            }
            var noviTermin = new Termin
            {
                Datum = DateTime.Parse(DatepickerDatum.Text).Date,
                Vreme = TimeSpan.Parse(TextboxVreme.Text),
                VrstaUsluge = TextboxVrstaUsluge.Text,
                IDPacijenta = Convert.ToInt32(ComboboxPacijent.SelectedValue.ToString()),
                IDZubara = Convert.ToInt32(ComboboxZubar.SelectedValue.ToString())
            };

            var poruka = _terminRepo.DodajTermin(noviTermin);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private bool ValidirajUnosTermina()
        {
            // Datum obavezan
            if (string.IsNullOrWhiteSpace(DatepickerDatum.Text)) return false;
            if (!DateTime.TryParse(DatepickerDatum.Text, out _)) return false;

            // Vreme obavezno
            if (string.IsNullOrWhiteSpace(TextboxVreme.Text)) return false;
            if (!TimeSpan.TryParse(TextboxVreme.Text, out _)) return false;

            // Vrsta usluge
            if (string.IsNullOrWhiteSpace(TextboxVrstaUsluge.Text)) return false;

            // Pacijent i zubar
            if (ComboboxPacijent.SelectedValue == null) return false;
            if (ComboboxZubar.SelectedValue == null) return false;

            return true;
        }


        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TextboxIdTermina.Text, out int id))
            {
                _terminRepo.ObrisiTermin(id);
                MessageBox.Show("Podaci su uspešno obrisani!");
                binDataGrid();
                ponistiUnosTxt();
            }
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            var termin = new Termin
            {
                IDTermina = int.Parse(TextboxIdTermina.Text),
                Datum = DateTime.Parse(DatepickerDatum.Text),
                Vreme = TimeSpan.Parse(TextboxVreme.Text),
                VrstaUsluge = TextboxVrstaUsluge.Text,
                IDPacijenta = Convert.ToInt32(ComboboxPacijent.SelectedValue),
                IDZubara = Convert.ToInt32(ComboboxZubar.SelectedValue)
            };

            _terminRepo.IzmeniTermin(termin);

            MessageBox.Show("Podaci su uspešno izmenjeni!");
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ComboboxZubar_Loaded(object sender, RoutedEventArgs e)
        {
            var AktivniZubari = _zubarRepo.VratiSveAktivneZubare();
            //var filter = zubari.FindAll(x => x.isDeleted == false);
            ComboboxZubar.ItemsSource = AktivniZubari;
            ComboboxZubar.DisplayMemberPath = "Ime";     // prikaz
            ComboboxZubar.SelectedValuePath = "IDZubara"; // vrednost
        }

        private void ComboboxPacijent_Loaded(object sender, RoutedEventArgs e)
        {
            var pacijenti = _pacijentRepo.VratiSvePacijente();
            ComboboxPacijent.ItemsSource = pacijenti;
            ComboboxPacijent.DisplayMemberPath = "Ime";
            ComboboxPacijent.SelectedValuePath = "IDPacijenta";

        }


        private void FiltrirajTermineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var izabrano = FiltrirajTermineComboBox.SelectedItem as ComboBoxItem;
            if (izabrano == null) return;

            string vremenskiPeriod = izabrano.Content.ToString();
            var view = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);

            DateTime danas = DateTime.Today;
            DateTime krajnjiDatum;

            switch (vremenskiPeriod)
            {
                case "Danas":
                    krajnjiDatum = danas;
                    break;

                case "7 dana":
                    krajnjiDatum = danas.AddDays(7);
                    break;

                case "30 dana":
                    krajnjiDatum = danas.AddDays(30);
                    break;

                default:
                    view.Filter = null; // prikazuje sve ako izbor nije validan
                    return;
            }

            view.Filter = item =>
            {
                var termin = item as Termin;
                return termin != null &&
                       termin.Datum.Date >= danas &&
                       termin.Datum.Date <= krajnjiDatum;
            };

        }
    }
}