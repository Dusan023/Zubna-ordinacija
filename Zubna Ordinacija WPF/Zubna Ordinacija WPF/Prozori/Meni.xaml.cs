using System;
using System.Collections.Generic;
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
using Zubna_Ordinacija_WPF.Prozori;

namespace Zubna_Ordinacija_WPF.Prozori
{
    /// <summary>
    /// Interaction logic for Meni.xaml
    /// </summary>
    public partial class Meni : Window
    {
        public Meni()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ButtonIzlogujteSe_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindowpage = new MainWindow();
            MainWindowpage.Show();
            this.Close();
        }

        private void ButtonPacijenti_Click(object sender, RoutedEventArgs e)
        {
            Pacijenti pacijentipage = new Pacijenti();
            pacijentipage.Show();
            this.Close();
        }

        private void ButtonTermini_Click(object sender, RoutedEventArgs e)
        {
            Termini terminipage = new Termini();
            terminipage.Show();
            this.Close();
        }

        private void ButtonPregledi_Click(object sender, RoutedEventArgs e)
        {
            Pregledi pregledipage = new Pregledi();
            pregledipage.Show();
            this.Close();
        }

        private void ButtonLekovi_Click(object sender, RoutedEventArgs e)
        {
            Lekovi lekovipage = new Lekovi();
            lekovipage.Show();
            this.Close();
        }

        private void ButtonZubari_Click(object sender, RoutedEventArgs e)
        {
            Zubari zubaripage = new Zubari();
            zubaripage.Show();
            this.Close();
        }
    }
}
