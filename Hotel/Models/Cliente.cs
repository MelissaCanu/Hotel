using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Cliente
    {
        [Required(ErrorMessage = "Il campo Codice Fiscale è obbligatorio")]
        [StringLength(16, ErrorMessage = "Il campo Codice Fiscale deve essere lungo 16 caratteri")]
        [Display(Name = "Codice Fiscale")]
        public string CodiceFiscale { get; set; }

        [Required(ErrorMessage = "Il campo Cognome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Cognome deve essere lungo al massimo 50 caratteri")]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Nome deve essere lungo al massimo 50 caratteri")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Città è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Città deve essere lungo al massimo 50 caratteri")]
        [Display(Name = "Città")]
        public string Citta { get; set; }

        [Required(ErrorMessage = "Il campo Provincia è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo Provincia deve essere lungo 2 caratteri")]
        [Display(Name = "Provincia")]
        public string Provincia { get; set; }

        [Required(ErrorMessage = "Il campo E-mail è obbligatorio")]
        [EmailAddress(ErrorMessage = "Il campo E-mail non è un indirizzo valido")]
        [StringLength(50, ErrorMessage = "Il campo E-mail deve essere lungo al massimo 50 caratteri")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "Il campo Telefono deve essere lungo al massimo 20 caratteri")]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [StringLength(20, ErrorMessage = "Il campo Cellulare deve essere lungo al massimo 20 caratteri")]
        [Display(Name = "Cellulare")]
        public string Cellulare { get; set; }
    }
}
