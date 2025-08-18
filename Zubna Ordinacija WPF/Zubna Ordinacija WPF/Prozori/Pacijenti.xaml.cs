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
    /// Interaction logic for Pacijenti.xaml
    /// </summary>
    public partial class Pacijenti : Window
    {
        public Pacijenti()
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

            if (dr != null)
            {

                TextboxIdPacijenta.Text = dr["IDPacijenta"].ToString();
                TextboxIme.Text = dr["Ime"].ToString();
                TextboxPrezime.Text = dr["Prezime"].ToString();
                TextboxJMBG.Text = dr["JMBG"].ToString();
                TextboxBrojTelefona.Text = dr["BrojTelefona"].ToString();
                ComboboxPol.Text = dr["Pol"].ToString();
                TextboxAlergije.Text = dr["Alergije"].ToString();
                TextboxTrudnoca.Text = dr["Trudnoca"].ToString();
                TextboxBrojZuba.Text = dr["BrojZuba"].ToString();

            }
        }
        private void binDataGrid()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM [Pacijent]";
            command.Connection = connection;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Pacijent");
            dataAdapter.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            string Zubar = ComboboxZubar.Text;
            string[] podaciZubara = Zubar.Split('-');
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO [Pacijent](Ime, Prezime, JMBG, BrojTelefona, Pol, Alergije, Trudnoca, BrojZuba, IDZubara) VALUES(@Ime, @Prezime, @JMBG, @BrojTelefona, @Pol, @Alergije, @Trudnoca, @BrojZuba, @IDZubara)";
            command.Parameters.AddWithValue("@Ime", TextboxIme.Text);
            command.Parameters.AddWithValue("@Prezime", TextboxPrezime.Text);
            command.Parameters.AddWithValue("@JMBG", TextboxJMBG.Text);
            command.Parameters.AddWithValue("@BrojTelefona", TextboxBrojTelefona.Text);
            command.Parameters.AddWithValue("@Pol", ComboboxPol.Text);
            command.Parameters.AddWithValue("@Alergije", TextboxAlergije.Text);
            command.Parameters.AddWithValue("@Trudnoca", TextboxTrudnoca.Text);
            command.Parameters.AddWithValue("@BrojZuba", TextboxBrojZuba.Text);
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

        private void ButtonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            string Zubar = ComboboxZubar.Text;
            string[] podaciZubara = Zubar.Split('-');
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE [Pacijent] SET  Ime=@Ime, Prezime=@Prezime, JMBG=@JMBG, BrojTelefona=@BrojTelefona, Pol=@Pol, Alergije=@Alergije, Trudnoca=@Trudnoca, BrojZuba=@BrojZuba, IDZubara=@IDZubara WHERE IDPacijenta=@IDPacijenta";
            command.Parameters.AddWithValue("@IDPacijenta", TextboxIdPacijenta.Text);
            command.Parameters.AddWithValue("@Ime", TextboxIme.Text);
            command.Parameters.AddWithValue("@Prezime", TextboxPrezime.Text);
            command.Parameters.AddWithValue("@JMBG", TextboxJMBG.Text);
            command.Parameters.AddWithValue("@BrojTelefona", TextboxBrojTelefona.Text);
            command.Parameters.AddWithValue("@Pol", ComboboxPol.Text);
            command.Parameters.AddWithValue("@Alergije", TextboxAlergije.Text);
            command.Parameters.AddWithValue("@Trudnoca", TextboxTrudnoca.Text);
            command.Parameters.AddWithValue("@BrojZuba", TextboxBrojZuba.Text);
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
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "DELETE FROM [Pacijent] WHERE IDPacijenta = @IDPacijenta";
            command.Parameters.AddWithValue("@IDPacijenta", TextboxIdPacijenta.Text);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno izbrisani!");
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
    }
}
