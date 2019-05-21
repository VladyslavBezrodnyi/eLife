using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eLifeApi.Models
{
    public class RegisterDoctorModel
    {
        [Required]
        [DisplayName("Спеціалізація")]
        public string Specialization { get; set; }
        
        [Required]
        [DisplayName("Категорія")]
        public string Category_ { get; set; }

        [Required]
        [DisplayName("Стаж")]
        public string Guardian { get; set; }

        [DisplayName("Клініка")]
        public int Id_clinic { get; set; }

        [Required]
        [DisplayName("Освіта")]
        public string Education { get; set; }
       
        [Required]
        [DisplayName("Вміння")]
        public string Skills { get; set; }

        [DisplayName("Практикуючий")]
        public bool Practiced { get; set; }

    }
}