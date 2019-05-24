using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLifeWEB.Models
{
    public class Feedback
    {
        [Key]
        [Column(Order = 0)]
        public string DoctorId { get; set; }

        [Key]
        [Column(Order = 1)]

        [DisplayName("Пацієнт")]
        public string PatientId { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Оцінка")]
        public int Stars { get; set; }

        [DisplayName("Коментар")]
        public string Comment { get; set; }

        public virtual ApplicationUser Doctor { get; set; }

        public virtual ApplicationUser Patient { get; set; }
    }
}