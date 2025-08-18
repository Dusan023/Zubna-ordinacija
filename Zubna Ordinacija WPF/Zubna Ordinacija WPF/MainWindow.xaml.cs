using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zubna_Ordinacija_WPF.Prozori;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Zubna_Ordinacija_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Username, Password;
            Username = TextboxKorisnickoIme.Text;
            Password = PasswordfieldLozinka.Password;

            if (Username == "Doktor" && Password == "1234")
            {
                Meni menipage = new Meni();
                menipage.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka!");
            }
        }
    }
}
