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
        public Zubari()
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
            if (dr != null)
            {

                TextboxIdZubara.Text = dr["IDZubara"].ToString();
                TextboxIme.Text = dr["Ime"].ToString();
                TextboxPrezime.Text = dr["Prezime"].ToString();
                TextboxJMBG.Text = dr["JMBG"].ToString();
                TextboxBrojTelefona.Text = dr["BrojTelefona"].ToString();
                TextboxEmail.Text = dr["Email"].ToString();

            }
        }

        private void binDataGrid()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT * FROM [Zubar]";
            command.Connection = connection;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable("Zubar");
            dataAdapter.Fill(dataTable);
            DataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO [Zubar](Ime, Prezime, JMBG, BrojTelefona, Email) VALUES(@Ime, @Prezime, @JMBG, @BrojTelefona, @Email)";
            command.Parameters.AddWithValue("@Ime", TextboxIme.Text);
            command.Parameters.AddWithValue("@Prezime", TextboxPrezime.Text);
            command.Parameters.AddWithValue("@JMBG", TextboxJMBG.Text);
            command.Parameters.AddWithValue("@BrojTelefona", TextboxBrojTelefona.Text);
            command.Parameters.AddWithValue("@Email", TextboxEmail.Text);
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
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE [Zubar] SET  Ime=@Ime, Prezime=@Prezime, JMBG=@JMBG, BrojTelefona=@BrojTelefona, Email=@Email WHERE IDZubara=@IDZubara";
            command.Parameters.AddWithValue("@IDZubara", TextboxIdZubara.Text);
            command.Parameters.AddWithValue("@Ime", TextboxIme.Text);
            command.Parameters.AddWithValue("@Prezime", TextboxPrezime.Text);
            command.Parameters.AddWithValue("@JMBG", TextboxJMBG.Text);
            command.Parameters.AddWithValue("@BrojTelefona", TextboxBrojTelefona.Text);
            command.Parameters.AddWithValue("@Email", TextboxEmail.Text);
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
            TextboxIdZubara.Text = "";
            TextboxIme.Text = "";
            TextboxPrezime.Text = "";
            TextboxJMBG.Text = "";
            TextboxBrojTelefona.Text = "";
            TextboxEmail.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = " alter table Pacijent nocheck constraint all DELETE FROM [Zubar] WHERE IDZubara = @IDZubara alter table Pacijent check constraint all";
            command.Parameters.AddWithValue("@IDZubara", TextboxIdZubara.Text);
            command.Connection = connection;
            int provera = command.ExecuteNonQuery();
            if (provera == 1)
            {
                MessageBox.Show("Podaci su uspešno izbrisani!");
                binDataGrid();
            }
            ponistiUnosTxt();
        }
    }
}
