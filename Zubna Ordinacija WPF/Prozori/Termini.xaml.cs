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

        public Termini()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _terminRepo = new TerminPravila();
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
            var noviTermin = new Termin
            {
                Datum = DateTime.Parse(DatepickerDatum.Text),
                Vreme = TimeSpan.Parse(TextboxVreme.Text),
                VrstaUsluge = TextboxVrstaUsluge.Text,
                IDPacijenta = Convert.ToInt32(ComboboxPacijent.SelectedValue.ToString()),
                IDZubara = Convert.ToInt32(ComboboxZubar.SelectedValue.ToString())
            };

            _terminRepo.DodajTermin(noviTermin);

            MessageBox.Show("Podaci su uspešno upisani!");
            binDataGrid();
            ponistiUnosTxt();
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
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand commandCbx = new SqlCommand();
            commandCbx.CommandText = "SELECT * FROM [Zubar] ORDER BY IDZubara";
            commandCbx.Connection = connection;
            SqlDataAdapter dataAdapterCbx = new SqlDataAdapter(commandCbx);
            DataTable dataTableCbx = new DataTable("Zubar");
            dataAdapterCbx.Fill(dataTableCbx);
            string IDZubara, Ime;
            for (int i = 0; i < dataTableCbx.Rows.Count; i++)
            {
                IDZubara = dataTableCbx.Rows[i]["IDZubara"].ToString();
                Ime = dataTableCbx.Rows[i]["Ime"].ToString();
                ComboboxZubar.Items.Add(IDZubara + "-" + Ime);
            }
        }

        private void ComboboxPacijent_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand commandCbx = new SqlCommand();
            commandCbx.CommandText = "SELECT * FROM [Pacijent] ORDER BY IDPacijenta";
            commandCbx.Connection = connection;
            SqlDataAdapter dataAdapterCbx = new SqlDataAdapter(commandCbx);
            DataTable dataTableCbx = new DataTable("Pacijent");
            dataAdapterCbx.Fill(dataTableCbx);
            string IDPacijenta, Ime;
            for (int i = 0; i < dataTableCbx.Rows.Count; i++)
            {
                IDPacijenta = dataTableCbx.Rows[i]["IDPacijenta"].ToString();
                Ime = dataTableCbx.Rows[i]["Ime"].ToString();
                ComboboxPacijent.Items.Add(IDPacijenta + "-" + Ime);
            }
        }
    }
}
