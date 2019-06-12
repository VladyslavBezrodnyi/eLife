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
        [Required(ErrorMessage = "Заповніть поле ПІБ")]
        [DisplayName("ПІБ")]
        public string Name{ get; set; }

        [Required(ErrorMessage = "Заповніть поле Дата народження")]
        [DisplayName("Дата народження")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [DisplayName("Стать")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Заповніть поле Адреса")]
        [DisplayName("Адреса")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Заповніть поле Місце роботи")]
        [DisplayName("Місце роботи")]
        public string Activity { get; set; }

        [Required(ErrorMessage = "Заповніть поле Група крові")]
        [DisplayName("Група крові")]
        public string BloodGroup { get; set; }

        [Required(ErrorMessage = "Заповніть поле Алергії")]
        [DisplayName("Алергії")]
        public string Allergy { get; set; }

        [Required(ErrorMessage = "Заповніть поле Хірургічні втручання")]
        [DisplayName("Хірургічні втручання")]
        public string Operations { get; set; }

        [Required(ErrorMessage = "Заповніть поле Інфекціфні захворювання")]
        [DisplayName("Інфекціфні захворювання")]
        public string Infectious_diseases { get; set; }

        [Required(ErrorMessage = "Заповніть поле Цукровий діабет")]
        [DisplayName("Цукровий діабет")]
        public string Diabetes { get; set; }
                
    }
}