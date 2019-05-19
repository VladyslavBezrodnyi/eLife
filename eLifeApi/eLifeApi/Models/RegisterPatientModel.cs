using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLifeApi.Models
{
    public class RegisterPatientModel
    {
      
        public string Adress { get; set; }
        
        public string Activity { get; set; }

        public string BloodGroup { get; set; }
        public string Allergy { get; set; }

        public string Operations { get; set; }
        
        public string Infectious_diseases { get; set; }

        public string Diabetes { get; set; }
        public string BankCard { get; set; }
    }
}