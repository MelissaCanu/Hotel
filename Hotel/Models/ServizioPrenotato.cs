using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class ServizioPrenotato
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo Data è obbligatorio")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Il campo Quantità è obbligatorio")]
        [Display(Name = "Quantità")]
        public int Quantita { get; set; }

        [Required(ErrorMessage = "Il campo Prezzo è obbligatorio")]
        [Display(Name = "Prezzo")]
        public decimal Prezzo { get; set; }

        public int IDPrenotazione { get; set; }
        public int IDServizio { get; set; }
    }
}