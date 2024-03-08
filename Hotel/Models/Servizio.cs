using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Servizio
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo Descrizione deve essere lungo al massimo 100 caratteri")]
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "Il campo Prezzo è obbligatorio")]
        [Display(Name = "Prezzo")]
        public decimal Prezzo { get; set; }
    }
}