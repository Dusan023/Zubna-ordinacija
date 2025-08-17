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
        public Termini()
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
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;

            string match = "", match2 = "";
            int x = 0;
            if (dr != null)
            {
                match = dr["IDZubara"].ToString();
                match2 = dr["IDZubara"].ToString();

            }
            // If the search string is empty set to begining of textBox
            int lastMatch = 0;
            bool found = true;
            while (found)
            {
                if (ComboboxZubar.Items.Count == x)
                {
                    ComboboxZubar.SelectedIndex = lastMatch;
                    found = false;
                }
                else
                {
                    ComboboxZubar.SelectedIndex = x;
                    match = ComboboxZubar.SelectedValue.ToString();
                    if (match.Contains(match2))
                    {
                        lastMatch = x;
                        found = false;
                    }
                    x++;
                }
            }

            string match3 = "", match4 = "";
            int x1 = 0;
            if (dr != null)
            {
                match3 = dr["IDPacijenta"].ToString();
                match4 = dr["IDPacijenta"].ToString();

            }
            // If the search string is empty set to begining of textBox
            int lastMatch1 = 0;
            bool found1 = true;
            while (found1)
            {
                if (ComboboxPacijent.Items.Count == x1)
                {
                    ComboboxPacijent.SelectedIndex = lastMatch1;
                    found1 = false;
                }
                else
                {
                    ComboboxPacijent.SelectedIndex = x1;
                    match3 = ComboboxPacijent.SelectedValue.ToString();
                    if (match3.Contains(match4))
                    {
                        lastMatch1 = x1;
                        found1 = false;
                    }
                    x++;
                }
            }

            if (dr != null)
            {
                TextboxIdTermina.Text = dr["IDTermina"].ToString();
                DatepickerDatum.Text = dr["Datum"].ToString();
                TextboxVreme.Text = dr["Vreme"].ToString();
                TextboxVrstaUsluge.Text = dr["VrstaUsluge"].ToString();
                
            }
        }

        private void binDataGrid()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM [Termin]";
            command.Connection = connection;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Termin");
            dataAdapter.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
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
            string Zubar = ComboboxZubar.Text;
            string[] podaciZubara = Zubar.Split('-');
            string Pacijent = ComboboxPacijent.Text;
            string[] podaciPacijenta = Pacijent.Split('-');
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            DateTime datum = Convert.ToDateTime(DatepickerDatum.Text);
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO [Termin] (Datum, Vreme, VrstaUsluge, IDPacijenta, IDZubara) VALUES(@Datum, @Vreme, @VrstaUsluge, @IDPacijenta, @IDZubara)";
            command.Parameters.AddWithValue("@Datum", datum);
            command.Parameters.AddWithValue("@Vreme", TextboxVreme.Text);
            command.Parameters.AddWithValue("@VrstaUsluge", TextboxVrstaUsluge.Text);
            command.Parameters.AddWithValue("@IDPacijenta", podaciPacijenta[0]);
            command.Parameters.AddWithValue("@IDZubara", podaciZubara[0]);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno upisani");
                binDataGrid();
            }
            ponistiUnosTxt();
        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "DELETE FROM [Termin] WHERE IDTermina = @IDTermina";
            command.Parameters.AddWithValue("@IDTermina", TextboxIdTermina.Text);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno izbrisani!");
                binDataGrid();
            }
            ponistiUnosTxt();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            string Zubar = ComboboxZubar.Text;
            string[] podaciZubara = Zubar.Split('-');
            string Pacijent = ComboboxPacijent.Text;
            string[] podaciPacijenta = Pacijent.Split('-');
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            DateTime datum = Convert.ToDateTime(DatepickerDatum.Text);
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE [Termin] SET Datum=@Datum, Vreme=@Vreme, VrstaUsluge=@VrstaUsluge, IDPacijenta=@IDPacijenta, IDZubara=@IDZubara WHERE IDTermina=@IDTermina";
            command.Parameters.AddWithValue("@IDTermina", TextboxIdTermina.Text);
            command.Parameters.AddWithValue("@Datum", datum);
            command.Parameters.AddWithValue("@Vreme", TextboxVreme.Text);
            command.Parameters.AddWithValue("@VrstaUsluge", TextboxVrstaUsluge.Text);
            command.Parameters.AddWithValue("@IDPacijenta", podaciPacijenta[0]);
            command.Parameters.AddWithValue("@IDZubara", podaciZubara[0]);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno izmenjeni!");
                binDataGrid();
            }
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
