using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eLifeWEB.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Пацієнт")]
        public int PatientId { get; set; }

        [DisplayName("Лікар")]
        public int DoctorId { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Статус")]
        public bool Status { get; set; }

        public virtual ApplicationUser Patient { get; set; }

        public virtual ApplicationUser Doctor { get; set; }
    }
}