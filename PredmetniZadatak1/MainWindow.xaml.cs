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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PredmetniZadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public static class Podaci
    {
        private static string tipKorisnika;
        private static readonly string admin = "admin";
        private static readonly string posetilac = "posetilac";

        public static string Admin => admin;
        public static string Posetilac => posetilac;
        public static string TipKorisnika { get => tipKorisnika; set => tipKorisnika = value; }
    }
    public partial class MainWindow : Window
    {
        public string lozinkaAdmin = "admin";
        public string lozinkaPosetilac = "posetilac";
        public MainWindow()
        {
            InitializeComponent();

            textBoxKorisnik.Text = "Unesite korisnika";
            textBoxKorisnik.Foreground = Brushes.Red;
            passwordBoxLozinka.Password = "Unesite lozinku";
            passwordBoxLozinka.Foreground = Brushes.Red;
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonPrijava_Click(object sender, RoutedEventArgs e)
        {
            string korisnik = textBoxKorisnik.Text;
            string lozinka = passwordBoxLozinka.Password;

            if (validate())
            {
                if (buttonPrijava.Content.Equals("Prijava"))
                {
                    if (korisnik == Podaci.Admin && lozinka == lozinkaAdmin)
                    {
                        Podaci.TipKorisnika = Podaci.Admin;
                        Prikaz prikaz = new Prikaz();
                        prikaz.ShowDialog();
                    }
                    else if (korisnik == Podaci.Posetilac && lozinka == lozinkaPosetilac)
                    {
                        Podaci.TipKorisnika = Podaci.Posetilac;
                        Prikaz prikaz = new Prikaz();
                        prikaz.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Pogrešno korisničko ime ili lozinka.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void buttonIzlaz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool validate()
        {
            bool result = true;

            if (textBoxKorisnik.Text.Trim().Equals("") || textBoxKorisnik.Text.Trim().Equals("Unesite korisnika"))
            {
                result = false;
                textBoxKorisnik.Foreground = Brushes.Red;
                textBoxGreskaKorisnik.Text = "Obavezno popuniti!";
                textBoxGreskaKorisnik.Foreground = Brushes.Red;
                buttonPrijava.Background = Brushes.Red;
            }
            else
            {
                textBoxKorisnik.Foreground = Brushes.Black;
                textBoxGreskaKorisnik.Text = "";
                buttonPrijava.Background = Brushes.SaddleBrown;
            }

            if (passwordBoxLozinka.Password.Trim().Equals("") || passwordBoxLozinka.Password.Trim().Equals("Unesite lozinku"))
            {
                result = false;
                passwordBoxLozinka.Foreground = Brushes.Red;
                textBoxGreskaLozinka.Text = "Obavezno popuniti!";
                textBoxGreskaLozinka.Foreground = Brushes.Red;
                buttonPrijava.Background = Brushes.Red;
            }
            else
            {
                passwordBoxLozinka.Foreground = Brushes.Black;
                textBoxGreskaLozinka.Text = "";
                buttonPrijava.Background = Brushes.SaddleBrown;
            }
            return result;
        }

        private void textBoxKorisnik_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxKorisnik.Text.Trim().Equals("Unesite korisnika"))
            {
                textBoxKorisnik.Text = "";
                textBoxKorisnik.Foreground = Brushes.Black;
            }
        }
        private void passwordBoxLozinka_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBoxLozinka.Password.Trim().Equals("Unesite lozinku"))
            {
                passwordBoxLozinka.Password = "";
                passwordBoxLozinka.Foreground = Brushes.Black;
            }
        }
        private void textBoxKorisnik_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxKorisnik.Text.Trim().Equals(string.Empty))
            {
                textBoxKorisnik.Text = "Unesite korisnika";
                textBoxKorisnik.Foreground = Brushes.Red;
            }
        }
        private void passwordBoxLozinka_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBoxLozinka.Password.Trim().Equals(string.Empty))
            {
                passwordBoxLozinka.Password = "Unesite lozinku";
                passwordBoxLozinka.Foreground = Brushes.Red;
            }
        }
        
    }
}
