using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Camera
    {
        public int Numero { get; set; }

        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo Descrizione deve essere lungo al massimo 100 caratteri")]
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "Il campo Tipologia è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Tipologia deve essere lungo al massimo 50 caratteri")]
        [Display(Name = "Tipologia")]
        public string Tipologia { get; set; }
    }
}