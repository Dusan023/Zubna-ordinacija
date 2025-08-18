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
using SlojPodataka.Klase;
using SlojServisa;

namespace Zubna_Ordinacija_WPF.Prozori
{
    /// <summary>
    /// Interaction logic for Lekovi.xaml
    /// </summary>
    public partial class Lekovi : Window
    {
        public Lekovi()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            if (DataGrid.SelectedItem is Lek lek)
            {
                TextboxIdLeka.Text = lek.IDLeka.ToString();
                TextboxNaziv.Text = lek.Naziv;
                TextboxProizvodjac.Text = lek.Proizvodjac;
                TextboxJacina.Text = lek.Jacina;
                TextboxDoziranje.Text = lek.Doziranje;
            }
        }
        private void binDataGrid()
        {
            LekPravila proveri = new LekPravila();
            DataGrid.ItemsSource = proveri.VratiSveLekove();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            var lek = new Lek
            {
                Naziv = TextboxNaziv.Text,
                Proizvodjac = TextboxProizvodjac.Text,
                Jacina = TextboxJacina.Text,
                Doziranje = TextboxDoziranje.Text
            };
            LekPravila uradi = new LekPravila();

            uradi.DodajLek(lek);
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            var lek = new Lek
            {
                IDLeka = int.Parse(TextboxIdLeka.Text),
                Naziv = TextboxNaziv.Text,
                Proizvodjac = TextboxProizvodjac.Text,
                Jacina = TextboxJacina.Text,
                Doziranje = TextboxDoziranje.Text
            };

            LekPravila izmeni = new LekPravila();
            izmeni.IzmeniLek(lek);
            binDataGrid();
            ponistiUnosTxt();
        }

        private void ponistiUnosTxt()
        {
            TextboxIdLeka.Text = "";
            TextboxNaziv.Text = "";
            TextboxProizvodjac.Text = "";
            TextboxJacina.Text = "";
            TextboxDoziranje.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(TextboxIdLeka.Text);
            LekPravila izbrisi = new LekPravila();
            izbrisi.ObrisiLek(id);
            binDataGrid();
            ponistiUnosTxt();
        }
    }
}
