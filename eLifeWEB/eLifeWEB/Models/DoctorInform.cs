using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLifeWEB.Models
{
    public class DoctorInform
    {
        public DoctorInform()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("Спеціалізація")]
        public string Specialization { get; set; }

        [Column("Category ")]
        [Required]
        [DisplayName("Категорія")]
        public string Category { get; set; }

        [Required]
        [DisplayName("Стаж")]
        public string Guardian { get; set; }

        [DisplayName("Клініка")]
        public int? ClinicId { get; set; }

        [Required]
        [DisplayName("Освіта")]
        public string Education { get; set; }

        [Required]
        [DisplayName("Вміння")]
        public string Skills { get; set; }

        [DisplayName("Практикуючий")]
        public bool Practiced { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] Image { get; set; }

        public virtual Clinic Clinic { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}