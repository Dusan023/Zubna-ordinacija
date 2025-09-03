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
    /// Interaction logic for Pregledi.xaml
    /// </summary>
    public partial class Pregledi : Window
    {
        private readonly PregledPravila _pregledRepo;
        public Pregledi()
        {
            InitializeComponent();
            _pregledRepo = new PregledPravila();
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
            var pregled = DataGrid.SelectedItem as Pregled;
            if (pregled == null) return;

            // Postavi ComboBox na lek koji odgovara IDLeka
            ComboboxLek.SelectedValue = pregled.IDLeka.ToString();

            // Popuni polja
            TextboxIdPregleda.Text = pregled.IDPregleda.ToString();
            IzvestajTextBox.Text = pregled.Izvestaj;
            TextboxIdTermina.Text = pregled.IDTermina.ToString();

            if (!_pregledRepo.ProveriAktivnostZubara(pregled.IDLeka))
            {
                MessageBox.Show(
                "Selektovan lek je trenutno van upotrebe (neaktivan) neaktivan",
                "Informacija",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            }
        }
        private void binDataGrid()
        {
            DataGrid.ItemsSource = _pregledRepo.VratiSvePreglede();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            var pregled = new Pregled
            {
                Izvestaj = IzvestajTextBox.Text,
                IDTermina = int.Parse(TextboxIdTermina.Text),
                IDLeka = Convert.ToInt32(ComboboxLek.SelectedValue)
            };

            var selectedValue = ComboboxLek.SelectedValue;
            //MessageBox.Show($"SelectedValue: {selectedValue}, Type: {selectedValue?.GetType()}");

            var poruka = _pregledRepo.DodajPregled(pregled);
            if (!poruka.Uspeh)
            {
                MessageBox.Show(poruka.Poruka);
                return;
            }
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            var pregled = new Pregled
            {
                IDPregleda = int.Parse(TextboxIdPregleda.Text),
                Izvestaj = IzvestajTextBox.Text,
                IDTermina = int.Parse(TextboxIdTermina.Text),
                IDLeka = Convert.ToInt32(ComboboxLek.SelectedValue)
            };

            var poruka = _pregledRepo.IzmeniPregled(pregled);
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
            TextboxIdPregleda.Text = "";
            IzvestajTextBox.Text = "";
            TextboxIdTermina.Text = "";
            ComboboxLek.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(TextboxIdPregleda.Text);
            _pregledRepo.ObrisiPregled(id);
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ComboboxLek_Loaded(object sender, RoutedEventArgs e)
        {
            LekPravila pravila = new LekPravila();
            var lekovi = pravila.VratiSveAktivneLekove(); // tvoj servisni sloj

            ComboboxLek.ItemsSource = lekovi;
            ComboboxLek.DisplayMemberPath = "Naziv";     // ono što vidi korisnik
            ComboboxLek.SelectedValuePath = "IDLeka";
        }
    }
}