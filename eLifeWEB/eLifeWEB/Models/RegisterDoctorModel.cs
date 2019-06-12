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
        [Required(ErrorMessage = "Заповніть поле ПІБ")]
        [DisplayName("ПІБ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Заповніть поле Дата народження")]
        [DisplayName("Дата народження")]
        public DateTime Birthday { get; set; }

        [Required]
        [DisplayName("Стать")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Заповніть поле Спеціалазіація")]
        [DisplayName("Спеціалізація")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Заповніть поле Категорія")]
        [DisplayName("Категорія")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Заповніть поле Стаж")]
        [DisplayName("Стаж")]
        public string Guardian { get; set; }

        [DisplayName("Клініка")]
        public int СlinicId { get; set; }

        [Required(ErrorMessage = "Заповніть поле Освіта")]
        [DisplayName("Освіта")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Заповніть поле Вміння")]
        [DisplayName("Вміння")]
        public string Skills { get; set; }
    }
}