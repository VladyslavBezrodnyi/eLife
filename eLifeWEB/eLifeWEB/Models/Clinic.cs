using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eLifeWEB.Models
{
    public class Clinic
    {
        public Clinic()
        {
            ClinicAdmins = new HashSet<ClinicAdmin>();
            DoctorInforms = new HashSet<DoctorInform>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Назва клініки")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Адреса")]
        public string Adress { get; set; }

        [Required]
        [DisplayName("Опис")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Банківська картка")]
        public string BankCard { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        public virtual ICollection<ClinicAdmin> ClinicAdmins { get; set; }

        public virtual ICollection<DoctorInform> DoctorInforms { get; set; }
    }
}