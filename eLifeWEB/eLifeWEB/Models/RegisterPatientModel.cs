using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLifeWEB.Models
{
    public class RegisterPatientModel
    {
        [Required]
        [DisplayName("ПІБ")]
        public string Name{ get; set; }
        [Required]
        [DisplayName("День народження")]
        public DateTime Birthday { get; set; }
        [Required]
        [DisplayName("Стать")]
        public string Gender { get; set; }
        [Required]
        [DisplayName("Адреса")]
        public string Adress { get; set; }
        [Required]
        [DisplayName("Місце роботи")]
        public string Activity { get; set; }

        [DisplayName("Група крові")]
        public string BloodGroup { get; set; }
        [Required]
        [DisplayName("Алергії")]
        public string Allergy { get; set; }

        [DisplayName("Хірургічні втручання")]
        public string Operations { get; set; }

        [DisplayName("Інфекціфні захворювання")]
        public string Infectious_diseases { get; set; }

        [DisplayName("Цукровий діабет")]
        public string Diabetes { get; set; }
        [Required]
        [DisplayName("Банківська картка")]
        public string BankCard { get; set; }
    }
}