using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Serializable]
    public class Nemanjici
    {
        private int godinaRodjenja;
        private string ime;
        private DateTime datum;
        private string slika;
        public string file;
        public bool IsSelected { get; set; }
        public int GodinaRodjenja { get => godinaRodjenja; set => godinaRodjenja = value; }
        public string Ime { get => ime; set => ime = value; }
        public DateTime Datum { get => datum; set => datum = value; }
        public string Slika { get => slika; set => slika = value; }
        public string File { get => file; set => file = value; }

        public Nemanjici() : this(0, "", new DateTime(), "", "") { }

        public Nemanjici(int godinaRodjenja, string ime, DateTime datum, string slika, string file)
        {
            this.GodinaRodjenja = godinaRodjenja;
            this.Ime = ime;
            this.Datum = datum;
            this.Slika = slika;
            this.File = file;
        }
    }
}
