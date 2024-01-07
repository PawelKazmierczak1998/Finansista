using System.ComponentModel.DataAnnotations;

namespace Finansista.Models
{
    public class Calculators
    {
        public  int Id { get; set; }       
        public decimal KwotaBrutto { get; set; }
        public decimal StawkaVAT { get; set; }
        public decimal KwotaNetto => KwotaBrutto / (1 + StawkaVAT / 100);
        public decimal Wynik { get; set; }

        public Calculators() 
        {
        this.KwotaBrutto = 0;  
        this.StawkaVAT = 23;
        this.Wynik = 0;
        }
    }
}
