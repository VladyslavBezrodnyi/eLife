using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLifeWEB.Models
{
    public class PatientInform
    {
        public PatientInform()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Адреса")]
        public string Adress { get; set; }

        [Required]
        [DisplayName("Місце роботи")]
        public string Activity { get; set; }

        [DisplayName("Група крові")]
        public string BloodGroup { get; set; }

        [DisplayName("Алергії")]
        public string Allergy { get; set; }

        [DisplayName("Хірургічні втручання")]
        public string Operations { get; set; }

        [Column("Infectious diseases")]
        [DisplayName("Інфекційні захворювання")]
        public string Infectious_diseases { get; set; }

        [DisplayName("Цукровий діабет")]
        public string Diabetes { get; set; }

        [Required]
        [DisplayName("Банківська картка")]
        public string BankCard { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}