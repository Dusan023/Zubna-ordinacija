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
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {

                TextboxIdLeka.Text = dr["IDLeka"].ToString();
                TextboxNaziv.Text = dr["Naziv"].ToString();
                TextboxProizvodjac.Text = dr["Proizvodjac"].ToString();
                TextboxJacina.Text = dr["Jacina"].ToString();
                TextboxDoziranje.Text = dr["Doziranje"].ToString();

            }
        }
        private void binDataGrid()
        {
            LekPravila proveri = new LekPravila();
            DataGrid.ItemsSource = proveri.VratiSveLekove();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString =
            ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "INSERT INTO [Lek](Naziv, Proizvodjac, Jacina, Doziranje) VALUES(@Naziv, @Proizvodjac, @Jacina, @Doziranje)";
            command.Parameters.AddWithValue("@Naziv", TextboxNaziv.Text);
            command.Parameters.AddWithValue("@Proizvodjac", TextboxProizvodjac.Text);
            command.Parameters.AddWithValue("@Jacina", TextboxJacina.Text);
            command.Parameters.AddWithValue("@Doziranje", TextboxDoziranje.Text);
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
            command.CommandText = "UPDATE [Lek] SET  Naziv=@Naziv, Proizvodjac=@Proizvodjac, Jacina=@Jacina, Doziranje=@Doziranje WHERE IDLeka=@IDLeka";
            command.Parameters.AddWithValue("@IDLeka", TextboxIdLeka.Text);
            command.Parameters.AddWithValue("@Naziv", TextboxNaziv.Text);
            command.Parameters.AddWithValue("@Proizvodjac", TextboxProizvodjac.Text);
            command.Parameters.AddWithValue("@Jacina", TextboxJacina.Text);
            command.Parameters.AddWithValue("@Doziranje", TextboxDoziranje.Text);
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
            TextboxIdLeka.Text = "";
            TextboxNaziv.Text = "";
            TextboxProizvodjac.Text = "";
            TextboxJacina.Text = "";
            TextboxDoziranje.Text = "";

        }

        private void ButtonObrisi_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connZubnaOrdinacija"].ConnectionString;
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = "DELETE FROM [Lek] WHERE IDLeka = @IDLeka";
            command.Parameters.AddWithValue("@IDLeka", TextboxIdLeka.Text);
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
