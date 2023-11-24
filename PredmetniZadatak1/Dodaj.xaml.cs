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
using Classes;
using Microsoft.Win32;

namespace PredmetniZadatak1
{
    /// <summary>
    /// Interaction logic for Dodaj.xaml
    /// </summary>
    public partial class Dodaj : Window
    {
        private string slika = "";
        List<string> ChosenColors = new List<string>();
        public Dodaj()
        {
            InitializeComponent();

            textBoxIme.Text = "Unesite ime vladara";
            textBoxIme.Foreground = Brushes.Red;

            textBoxGodinaRodjenja.Text = "Unesite godinu rodjenja vladara";
            textBoxGodinaRodjenja.Foreground = Brushes.Red;

            ComboBoxFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            for (int i = 1; i <= 72; i++) //popuni ComboBox s dostupnim veličinama fonta
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
                buttonDodaj.Background = Brushes.Red;
            }
            else
            {
                textBoxIme.Foreground = Brushes.Black;
                textBoxIme.BorderBrush = Brushes.Black;
                textBoxIme.BorderThickness = new Thickness(1);
                textBoxGreskaIme.Text = "";
            }
            if (textBoxGodinaRodjenja.Text.Trim().Equals("") || textBoxGodinaRodjenja.Text.Trim().Equals("Unesite godinu rodjenja vladara"))
            {
                result = false;
                textBoxGodinaRodjenja.Foreground = Brushes.Red;
                textBoxGodinaRodjenja.BorderBrush = Brushes.Red;
                textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                textBoxGreskaGodinaRodjenja.Text = "Obavezno popuniti!";
                textBoxGreskaGodinaRodjenja.Foreground = Brushes.Red;
                buttonDodaj.Background = Brushes.Red;
            }
            else
            {
                bool isNumeric = int.TryParse(textBoxGodinaRodjenja.Text, out _);
                if (isNumeric)
                {
                    if (Int32.Parse(textBoxGodinaRodjenja.Text) > 0)
                    {
                        textBoxGodinaRodjenja.Foreground = Brushes.Black;
                        textBoxGodinaRodjenja.BorderBrush = Brushes.Black;
                        textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                        textBoxGreskaGodinaRodjenja.Text = "";
                    }
                    else
                    {
                        result = false;
                        textBoxGodinaRodjenja.Foreground = Brushes.Red;
                        textBoxGodinaRodjenja.BorderBrush = Brushes.Red;
                        textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                        textBoxGreskaGodinaRodjenja.Text = "Unesite pozitivan broj!";
                    }
                }
                else
                {
                    result = false;
                    textBoxGodinaRodjenja.Foreground = Brushes.Red;
                    textBoxGodinaRodjenja.BorderBrush = Brushes.Red;
                    textBoxGodinaRodjenja.BorderThickness = new Thickness(1);
                    textBoxGreskaGodinaRodjenja.Text = "Unesite broj!";
                }
            }
            if (textBoxSlika.Text.Trim().Equals("Slika"))
            {
                result = false;
                borderSlika.BorderBrush = Brushes.Red;
                borderSlika.BorderThickness = new Thickness(1);
                labelaGreskaSlika.Content = "Obavezno!";
                labelaGreskaSlika.Background = Brushes.LightGray;
                labelaGreskaSlika.Foreground = Brushes.Red;
                labelaGreskaSlika.BorderThickness = new Thickness(1);
            }
            else
            {
                borderSlika.BorderBrush = Brushes.Black;
                borderSlika.BorderThickness = new Thickness(0);
                labelaGreskaSlika.Content = "";
                labelaGreskaSlika.BorderThickness = new Thickness(0);
                textBoxSlika.Text = "";
            }
            return result;
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                if (buttonDodaj.Content.Equals("Dodaj"))
                {
                    string naziv = textBoxIme.Text + ".rtf";

                    TextRange textRange; //istanca klase TextRange, predstavlja opseg teksta u RichTextBox kontroli, da se obuhvati ceo tekst
                    FileStream fileStream; //za citanje i pisanje u fajl
                    textRange = new TextRange(RichTextBoxNemanjici.Document.ContentStart, RichTextBoxNemanjici.Document.ContentEnd);
                    fileStream = new FileStream(naziv, FileMode.Create);
                    textRange.Save(fileStream, DataFormats.Rtf); //cuvanje fajla
                    fileStream.Close(); //zatvara se fileStream
                    this.Close(); //zatvara se prozor

                    Prikaz.listaNemanjici.Add(new Nemanjici(Int32.Parse(textBoxGodinaRodjenja.Text), textBoxIme.Text, DateTime.Now, slika, naziv));
                }
            }
            else
            {
                MessageBox.Show("Popunite sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                slika = openFileDialog.FileName; //putanja datoteke
                Uri uri = new Uri(slika); //konvertuje putanju slike u Uri
                imageSlika.Source = new BitmapImage(uri); //prikazuje sliku
                textBoxSlika.Text = "";
            }
        }

        private void buttonIzađi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFamily.SelectedItem != null && !RichTextBoxNemanjici.Selection.IsEmpty)
            {
                RichTextBoxNemanjici.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, ComboBoxFamily.SelectedItem);
            }
        }

        private void ComboBoxSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSize.SelectedValue != null && !RichTextBoxNemanjici.Selection.IsEmpty)
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

        private void RichTextBoxNemanjici_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontWeightProperty);
            tglButtonBold.IsChecked = (temp != DependencyProperty.UnsetValue) && temp.Equals(FontWeights.Bold);

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontStyleProperty);
            tglButtonItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && temp.Equals(FontStyles.Italic);

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            tglButtonUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && temp.Equals(TextDecorations.Underline);

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            ComboBoxFamily.SelectedItem = temp;

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.FontSizeProperty);
            ComboBoxSize.Text = temp.ToString();

            temp = RichTextBoxNemanjici.Selection.GetPropertyValue(Inline.ForegroundProperty);
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

        private void textBoxIme_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Trim().Equals("Unesite ime vladara"))
            {
                textBoxIme.Text = "";
                textBoxIme.Foreground = Brushes.Black;
            }
        }
        private void textBoxGodinaRodjenja_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxGodinaRodjenja.Text.Trim().Equals("Unesite godinu rodjenja vladara"))
            {
                textBoxGodinaRodjenja.Text = "";
                textBoxGodinaRodjenja.Foreground = Brushes.Black;
            }
        }

        private void textBoxIme_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxIme.Text.Trim().Equals(string.Empty))
            {
                textBoxIme.Text = "Unesite ime vladara";
                textBoxIme.Foreground = Brushes.Red;
            }
        }
        private void textBoxGodinaRodjenja_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxGodinaRodjenja.Text.Trim().Equals(string.Empty))
            {
                textBoxGodinaRodjenja.Text = "Unesite godinu rodjenja vladara";
                textBoxGodinaRodjenja.Foreground = Brushes.Red;
            }
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

