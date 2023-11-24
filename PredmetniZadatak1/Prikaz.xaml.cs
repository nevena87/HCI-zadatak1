using Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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

namespace PredmetniZadatak1
{
    /// <summary>
    /// Interaction logic for Prikaz.xaml
    /// </summary>
    public partial class Prikaz : Window
    {
        public static BindingList<Nemanjici> listaNemanjici { get; set; }
        private DataIO serializer = new DataIO();
        public Prikaz()
        {
            listaNemanjici = serializer.DeSerializeObject<BindingList<Nemanjici>>("Nemanjici.xml");
            if (listaNemanjici == null) //U slucaju da nista nije ucitano
            {
                listaNemanjici = new BindingList<Nemanjici>(); //nova lista pa cemo u nju dodavati
            }

            DataContext = this; //okidac Data Bindinga

            InitializeComponent();
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (Podaci.TipKorisnika == Podaci.Admin)
            {
                Dodaj dodaj = new Dodaj();
                dodaj.ShowDialog();
            }
        }

        private void buttonObrisi_Click(object sender, RoutedEventArgs e)
        {
            List<Nemanjici> listaZaBrisanje = new List<Nemanjici>();
            foreach (Nemanjici item in dataGridNemanjici.Items)
            {
                if (item.IsSelected)
                {
                    listaZaBrisanje.Add(item);
                }
            }

            foreach (Nemanjici item in listaZaBrisanje)
            {
                listaNemanjici.Remove(item);
                if (File.Exists(item.File))
                {
                    File.Delete(item.File);
                }
            }
            dataGridNemanjici.Items.Refresh();
        }
        private void buttonDodaj_Loaded(object sender, RoutedEventArgs e)
        {
            if (Podaci.TipKorisnika.Equals(Podaci.Posetilac))
            {
                buttonDodaj.Visibility = Visibility.Collapsed;
            }
        }

        private void buttonObrisi_Loaded(object sender, RoutedEventArgs e)
        {
            if (Podaci.TipKorisnika.Equals(Podaci.Posetilac))
            {
                buttonObrisi.Visibility = Visibility.Collapsed;
            }
        }
        private void buttonZatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void save(object sender, CancelEventArgs e)
        {
            //cuvanje u fajl objekta klase BindingList - nasa lista Nemanjica
            serializer.SerializeObject<BindingList<Nemanjici>>(listaNemanjici, "Nemanjici.xml");
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (Podaci.TipKorisnika.Equals(Podaci.Admin))
            {
                Izmena izmena = new Izmena(dataGridNemanjici.SelectedIndex);
                izmena.ShowDialog();
            }
            else if (Podaci.TipKorisnika.Equals(Podaci.Posetilac))
            {
                Procitaj procitaj = new Procitaj(dataGridNemanjici.SelectedIndex);
                procitaj.ShowDialog();
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
