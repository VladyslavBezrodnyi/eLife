using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLifeApi.Models
{
    public class RegisterPatientModel
    {
        [DisplayName("Адреса")]
        public string Adress { get; set; }

        [DisplayName("Місце роботи")]
        public string Activity { get; set; }

        [DisplayName("Група крові")]
        public string BloodGroup { get; set; }

        [DisplayName("Алергії")]
        public string Allergy { get; set; }

        [DisplayName("Хірургічні втручання")]
        public string Operations { get; set; }

        [DisplayName("Інфекціфні захворювання")]
        public string Infectious_diseases { get; set; }

        [DisplayName("Цукровий діабет")]
        public string Diabetes { get; set; }

        [DisplayName("Банківська картка")]
        public string BankCard { get; set; }
    }
}