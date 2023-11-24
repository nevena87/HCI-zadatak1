using Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Izmena.xaml
    /// </summary>
    public partial class Izmena : Window
    {
        private int index = 0;
        private string slika = "";
        private string fajl_pomocni = "";
        private string slika_pomocna = "";
        List<string> ChosenColors = new List<string>();
        public Izmena(int idx)
        {
            InitializeComponent();
            Nemanjici Nemanjici = Prikaz.listaNemanjici[idx];
            index = idx;

            textBoxIme.Text = Nemanjici.Ime;
            textBoxGodinaRodjenja.Text = Convert.ToString(Nemanjici.GodinaRodjenja);

            slika_pomocna = Nemanjici.Slika;
            Uri uri = new Uri(Nemanjici.Slika);
            imageSlika.Source = new BitmapImage(uri);

            fajl_pomocni = Nemanjici.File;

            TextRange textRange;
            FileStream fileStream;

            if (File.Exists(fajl_pomocni))
            {
                textRange = new TextRange(RichTextBoxNemanjici.Document.ContentStart, RichTextBoxNemanjici.Document.ContentEnd);
                using (fileStream = new FileStream(fajl_pomocni, FileMode.OpenOrCreate))
                {
                    textRange.Load(fileStream, DataFormats.Rtf);
                }
            }

            ComboBoxFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            for (int i = 1; i <= 72; i++) // popuni ComboBox s dostupnim veličinama fonta
            {
                ComboBoxSize.Items.Add(i.ToString());
            }
            object temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontSizeProperty);

            ComboBoxFamily.SelectedItem = "Black";
            for (int i = 0; i < typeof(Colors).GetProperties().Count(); i++)
            {
                ComboBoxColor.Items.Add(typeof(Colors).GetProperties()[i].ToString().Split(' ')[1]);
            }
            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.ForegroundProperty);
            ComboBoxColor.SelectedItem = GetColor((SolidColorBrush)new BrushConverter().ConvertFrom(temp.ToString()));
        }
        private bool validate()
        {
            bool result = true;

            if (textBoxIme.Text.Trim().Equals("") || textBoxIme.Text.Trim().Equals("Unesite ime vladara"))
            {
                result = false;
                textBoxIme.Foreground = Brushes.Red;
                textBoxIme.BorderBrush = Brushes.Red;
                textBoxIme.BorderThickness = new Thickness(1);
                textBoxGreskaIme.Text = "Obavezno popuniti!";
                textBoxGreskaIme.Foreground = Brushes.Red;
                textBoxGreskaIme.BorderThickness = new Thickness(0);
                buttonIzmeni.Background = Brushes.Red;
            }
            else
            {
                textBoxIme.Foreground = Brushes.Black;
                textBoxIme.BorderBrush = Brushes.Black;
                textBoxIme.BorderThickness = new Thickness(1);
                textBoxGreskaIme.BorderThickness = new Thickness(0);
                textBoxGreskaIme.Text = "";
            }
            if (textBoxGodinaRodjenja.Text.Trim().Equals("") || textBoxGodinaRodjenja.Text.Trim().Equals("Unesite godinu rodjenja vladara"))
            {
                result = false;
                textBoxGodinaRodjenja.Foreground = Brushes.Red;
                textBoxGodinaRodjenja.BorderBrush = Brushes.Red;
                textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                textBoxGreskaGodinaRodjenja.Text = "Obavezno popuniti!";
                textBoxGreskaGodinaRodjenja.BorderThickness = new Thickness(0);
                textBoxGreskaGodinaRodjenja.Foreground = Brushes.Red;
                buttonIzmeni.Background = Brushes.Red;
            }
            else
            {
                bool isNumeric = int.TryParse(textBoxGodinaRodjenja.Text, out _);
                if (isNumeric)
                {
                    if (Int32.Parse(textBoxGodinaRodjenja.Text) >= 0)
                    {
                        textBoxGodinaRodjenja.Foreground = Brushes.Black;
                        textBoxGodinaRodjenja.BorderBrush = Brushes.Black;
                        textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                        textBoxGreskaGodinaRodjenja.BorderThickness = new Thickness(0);
                        textBoxGreskaGodinaRodjenja.Text = "";
                    }
                    else
                    {
                        result = false;
                        textBoxGodinaRodjenja.Foreground = Brushes.Red;
                        textBoxGodinaRodjenja.BorderBrush = Brushes.Red;
                        textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                        textBoxGreskaGodinaRodjenja.Text = "Unesite pozitivan broj!";
                        textBoxGreskaGodinaRodjenja.BorderThickness = new Thickness(1);
                        textBoxGreskaGodinaRodjenja.BorderBrush = Brushes.Red;
                    }
                }
                else
                {
                    result = false;
                    textBoxGodinaRodjenja.Foreground = Brushes.Red;
                    textBoxGodinaRodjenja.BorderBrush = Brushes.Red;
                    textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                    textBoxGreskaGodinaRodjenja.Text = "Unesite broj!";
                    textBoxGreskaGodinaRodjenja.BorderThickness = new Thickness(1);
                    textBoxGreskaGodinaRodjenja.BorderBrush = Brushes.Red;
                }
            }
            return result;
        }
        private void buttonIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                if (slika == "")
                {
                    slika = slika_pomocna;
                }
                Prikaz.listaNemanjici[index] = new Nemanjici(Int32.Parse(textBoxGodinaRodjenja.Text), textBoxIme.Text, DateTime.Now, slika, fajl_pomocni);

                TextRange textRange;
                FileStream fileStream;
                textRange = new TextRange(RichTextBoxNemanjici.Document.ContentStart, RichTextBoxNemanjici.Document.ContentEnd);
                fileStream = new FileStream(fajl_pomocni, FileMode.Open);
                textRange.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
                this.Close();
            }
            else
            {
                MessageBox.Show("Popunite polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                textBoxIme.Foreground = Brushes.Black;
                textBoxGodinaRodjenja.Foreground = Brushes.Black;
            }
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            textBoxSlika.Text = "";
            if (openFileDialog.ShowDialog() == true)
            {
                slika = openFileDialog.FileName;
                Uri uri = new Uri(slika);
                imageSlika.Source = new BitmapImage(uri);
                textBoxSlika.Text = "";
            }
        }
        private void buttonIzađi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ComboBoxFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFamily.SelectedItem != null)
            {
                RichTextBoxNemanjici.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, ComboBoxFamily.SelectedItem);
            }
        }
        private void ComboBoxSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSize.SelectedValue != null)
            {
                RichTextBoxNemanjici.Selection.ApplyPropertyValue(Inline.FontSizeProperty, ComboBoxSize.SelectedValue);
            }
        }
        private string GetColor(SolidColorBrush brush)
        {
            string result = string.Empty;
            foreach (string s in ChosenColors)
            {
                SolidColorBrush stringBrush = (SolidColorBrush)new BrushConverter().ConvertFrom(s);
                if ((stringBrush.Color.A == brush.Color.A) && (stringBrush.Color.R == brush.Color.R) && (stringBrush.Color.G == brush.Color.G) && (stringBrush.Color.B == brush.Color.B))
                {
                    result = s;
                }
            }
            return result;
        }
        private void ComboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxColor.SelectedItem != null)
            {
                RichTextBoxNemanjici.Selection.ApplyPropertyValue(Inline.ForegroundProperty, (SolidColorBrush)new BrushConverter().ConvertFrom(ComboBoxColor.SelectedValue.ToString()));
                if (!ChosenColors.Contains(ComboBoxColor.SelectedValue.ToString()))
                {
                    ChosenColors.Add(ComboBoxColor.SelectedValue.ToString());
                }
            }
        }
        private void izbrojReci()
        {
            string tekst = new TextRange(RichTextBoxNemanjici.Document.ContentStart, RichTextBoxNemanjici.Document.ContentEnd).Text;
            string[] reci = tekst.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int brojReci = reci.Length;
            TextBlockBrojReci.Text = "Broj reči: " + brojReci.ToString();
        }
        private void RichTextBoxNemanjici_TextChanged(object sender, TextChangedEventArgs e)
        {
            izbrojReci();
        }

        private void RichTextBoxNemanjici_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontStyleProperty);
            tglButtonItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && temp.Equals(FontStyles.Italic);

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontWeightProperty);
            tglButtonBold.IsChecked = (temp != DependencyProperty.UnsetValue) && temp.Equals(FontWeights.Bold);

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            tglButtonUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && temp.Equals(TextDecorations.Underline);

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            ComboBoxFamily.SelectedItem = temp;

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontSizeProperty);
            ComboBoxSize.Text = temp.ToString();

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.ForegroundProperty);
        }
        
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
