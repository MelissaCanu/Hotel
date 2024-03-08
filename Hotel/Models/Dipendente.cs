using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Dipendente
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Nome deve essere lungo al massimo 50 caratteri")] 
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Username è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Username deve essere lungo al massimo 50 caratteri")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo Password deve essere lungo al massimo 50 caratteri")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}