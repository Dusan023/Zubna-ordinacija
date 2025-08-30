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
        public Pacijenti()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _pacijentRepo= new PacijentPravila();
            _zubarRepo = new ZubarPravila();
            _proveriUnosZaTrudnocu = new InputTrudnocaCheck();
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
                Trudnoca = string.IsNullOrEmpty(TextboxTrudnoca.Text) ? false : true,  //paziti prilikom ispravke
                BrojZuba = int.Parse(TextboxBrojZuba.Text),
                IDZubara = int.Parse(ComboboxZubar.SelectedValue.ToString())
            };

            MessageBox.Show(TextboxIdPacijenta.Text);

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
            var zubari = _zubarRepo.VratiSveZubare();
            ComboboxZubar.ItemsSource = zubari;
            ComboboxZubar.DisplayMemberPath = "Ime";     // prikaz
            ComboboxZubar.SelectedValuePath = "IDZubara"; // vrednost
        }
    }
}
