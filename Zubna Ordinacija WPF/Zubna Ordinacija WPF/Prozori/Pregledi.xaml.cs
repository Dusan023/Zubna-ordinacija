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
        public Pregledi()
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
                match = dr["idLeka"].ToString();
                match2 = dr["idLeka"].ToString();

            }
            // If the search string is empty set to begining of textBox
            int lastMatch = 0;
            bool found = true;
            while (found)
            {
                if (ComboboxLek.Items.Count == x)
                {
                    ComboboxLek.SelectedIndex = lastMatch;
                    found = false;
                }
                else
                {
                    ComboboxLek.SelectedIndex = x;
                    match = ComboboxLek.SelectedValue.ToString();
                    if (match.Contains(match2))
                    {
                        lastMatch = x;
                        found = false;
                    }
                    x++;
                }
            }
            if (dr != null)
            {

                TextboxIdPregleda.Text = dr["IDPregleda"].ToString();
                DatepickerDatumSledecePosete.Text = dr["DatumSledecePosete"].ToString();
                TextboxIdTermina.Text = dr["IDTermina"].ToString();

            }
        }
        private void binDataGrid()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM [Pregled]";
            command.Connection = connection;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Pregled");
            dataAdapter.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            string Lek = ComboboxLek.Text;
            string[] podaciLeka = Lek.Split('-');
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            DateTime datum = Convert.ToDateTime(DatepickerDatumSledecePosete.Text);
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO [Pregled](DatumSledecePosete, IDTermina, IDLeka) VALUES(@DatumSledecePosete, @IDTermina, @IDLeka)";
            command.Parameters.AddWithValue("@DatumSledecePosete", datum);
            command.Parameters.AddWithValue("@IDTermina", TextboxIdTermina.Text);
            command.Parameters.AddWithValue("@IDLeka", podaciLeka[0]);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno upisani");
                binDataGrid();
            }
            ponistiUnosTxt();
        }

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            string Lek = ComboboxLek.Text;
            string[] podaciLeka = Lek.Split('-');
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            DateTime datum = Convert.ToDateTime(DatepickerDatumSledecePosete.Text);
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE [Pregled] SET  DatumSledecePosete=@DatumSledecePosete, IDTermina=@IDTermina, IDLeka=@IDLeka WHERE IDPregleda=@IDPregleda";
            command.Parameters.AddWithValue("@IDPregleda", TextboxIdPregleda.Text);
            command.Parameters.AddWithValue("@DatumSledecePosete", datum);
            command.Parameters.AddWithValue("@IDTermina", TextboxIdTermina.Text);
            command.Parameters.AddWithValue("@IDLeka", podaciLeka[0]);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno izmenjeni!");
                binDataGrid();
            }
            ponistiUnosTxt();
        }

        private void ponistiUnosTxt()
        {
            TextboxIdPregleda.Text = "";
            DatepickerDatumSledecePosete.Text = "";
            TextboxIdTermina.Text = "";
            ComboboxLek.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "DELETE FROM [Pregled] WHERE IDPregleda = @IDPregleda";
            command.Parameters.AddWithValue("@IDPregleda", TextboxIdPregleda.Text);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno izbrisani!");
                binDataGrid();
            }
            ponistiUnosTxt();
        }

        private void ComboboxLek_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand commandCbx = new SqlCommand();
            commandCbx.CommandText = "SELECT * FROM [Lek] ORDER BY IDLeka";
            commandCbx.Connection = connection;
            SqlDataAdapter dataAdapterCbx = new SqlDataAdapter(commandCbx);
            DataTable dataTableCbx = new DataTable("Lek");
            dataAdapterCbx.Fill(dataTableCbx);
            string IDLeka, Naziv;
            for (int i = 0; i < dataTableCbx.Rows.Count; i++)
            {
                IDLeka = dataTableCbx.Rows[i]["IDLeka"].ToString();
                Naziv = dataTableCbx.Rows[i]["Naziv"].ToString();
                ComboboxLek.Items.Add(IDLeka + "-" + Naziv);
            }
        }
    }
}
