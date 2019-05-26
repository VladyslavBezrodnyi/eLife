using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eLifeWEB.Models
{
    public class RegisterDoctorModel
    {
        [DisplayName("ПІБ")]
        public string Name { get; set; }

        [DisplayName("День народження")]
        public DateTime Birthday { get; set; }

        [DisplayName("Стать")]
        public string Gender { get; set; }

        [Required]
        [DisplayName("Спеціалізація")]
        public string Specialization { get; set; }

        [Required]
        [DisplayName("Категорія")]
        public string Category { get; set; }

        [Required]
        [DisplayName("Стаж")]
        public string Guardian { get; set; }

        [DisplayName("Клініка")]
        public int СlinicId { get; set; }

        [Required]
        [DisplayName("Освіта")]
        public string Education { get; set; }

        [Required]
        [DisplayName("Вміння")]
        public string Skills { get; set; }
    }
}