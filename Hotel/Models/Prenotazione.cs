using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel.Models
{
    public class Prenotazione
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Il campo Data Prenotazione è obbligatorio")]
        [Display(Name = "Data Prenotazione")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataPrenotazione { get; set; }
        public int NumeroProgressivo { get; set; }

        [Required(ErrorMessage = "Il campo Anno è obbligatorio")]
        [Display(Name = "Anno")]
        
        public int Anno { get; set; }

        [Required(ErrorMessage = "Il campo Periodo Soggiorno Dal è obbligatorio")]
        [Display(Name = "Periodo Soggiorno Dal")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PeriodoSoggiornoDal { get; set; }

        [Required(ErrorMessage = "Il campo Periodo Soggiorno Al è obbligatorio")]
        [Display(Name = "Periodo Soggiorno Al")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PeriodoSoggiornoAl { get; set; }

        [Required(ErrorMessage = "Il campo Caparra è obbligatorio")]
        [Display(Name = "Caparra")]
        public decimal Caparra { get; set; }

        [Required(ErrorMessage = "Il campo Tariffa è obbligatorio")]
        [Display(Name = "Tariffa")]
        public decimal Tariffa { get; set; }

        [Required(ErrorMessage = "Il campo Dettagli è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo Dettagli deve essere lungo al massimo 100 caratteri")]
        [Display(Name = "Dettagli")]    
        public string Dettagli { get; set; }

        public string CodiceFiscaleCliente { get; set; }

        public int NumeroCamera { get; set; }

        //gestione servizi
        public List<Servizio> Servizi { get; set; } //aggiungo la lista Servizi 
        public List<ServizioPrenotato> ServiziPrenotati { get; set; } //aggiungo lista Servizi Prenotati

    }

}