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
using Classes;
using System.IO;

namespace PredmetniZadatak1
{
    /// <summary>
    /// Interaction logic for Procitaj.xaml
    /// </summary>
    public partial class Procitaj : Window
    {
        public Procitaj(int index)
        {
            InitializeComponent();
            Nemanjici Nemanjici = Prikaz.listaNemanjici[index];

            textBoxIme.Text = Nemanjici.Ime;
            textBoxGodinaRodjenja.Text = "Godina rodjenja vladara: " + Convert.ToString(Nemanjici.GodinaRodjenja) + ".";
            textBoxDatum.Text = "Datum generisanja: " + Nemanjici.Datum.ToString("dd.MM.yyyy") + ".";

            Uri uri = new Uri(Nemanjici.Slika);
            imageSlika.Source = new BitmapImage(uri);

            TextRange textRange;
            FileStream fileStream;

            if (File.Exists(Nemanjici.File))
            {
                textRange = new TextRange(richTextBoxNemanjici.Document.ContentStart, richTextBoxNemanjici.Document.ContentEnd);
                using (fileStream = new FileStream(Nemanjici.File, FileMode.OpenOrCreate))
                {
                    textRange.Load(fileStream, DataFormats.Rtf);
                }
            }
        }
        private void buttonZatvori_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
