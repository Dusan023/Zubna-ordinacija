using Klase;
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

namespace Zubna_Ordinacija_WPF.Prozori
{
    /// <summary>
    /// Interaction logic for Zubari.xaml
    /// </summary>
    public partial class Zubari : Window
    {
        private readonly ZubarPravila _zubarPravila;

        public Zubari()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _zubarPravila = new ZubarPravila();
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
            
            if (DataGrid.SelectedItem is Zubar zubar)
            {
                TextboxIdZubara.Text = zubar.IDZubara.ToString();
                TextboxIme.Text = zubar.Ime;
                TextboxPrezime.Text = zubar.Prezime;
                TextboxJMBG.Text = zubar.JMBG;
                TextboxBrojTelefona.Text = zubar.BrojTelefona;
                TextboxEmail.Text = zubar.Email;
            }
        }

        private void binDataGrid()
        {
            DataGrid.ItemsSource = _zubarPravila.VratiSveZubare();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            Zubar noviZubar = new Zubar
            {
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Email = TextboxEmail.Text
            };

            _zubarPravila.DodajZubara(noviZubar);
            MessageBox.Show("Zubar uspešno dodat!");
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            Zubar izmena = new Zubar
            {
                IDZubara = Convert.ToInt32(TextboxIdZubara.Text),
                Ime = TextboxIme.Text,
                Prezime = TextboxPrezime.Text,
                JMBG = TextboxJMBG.Text,
                BrojTelefona = TextboxBrojTelefona.Text,
                Email = TextboxEmail.Text
            };

            _zubarPravila.IzmeniZubara(izmena);
            MessageBox.Show("Podaci su uspešno izmenjeni!");
            binDataGrid();
            ponistiUnosTxt();
        }
        private void ponistiUnosTxt()
        {
            TextboxIdZubara.Text = "";
            TextboxIme.Text = "";
            TextboxPrezime.Text = "";
            TextboxJMBG.Text = "";
            TextboxBrojTelefona.Text = "";
            TextboxEmail.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(TextboxIdZubara.Text);
            _zubarPravila.ObrisiZubara(id);
            MessageBox.Show("Zubar uspešno obrisan!");
            binDataGrid();
            ponistiUnosTxt();
        }
    }
}
