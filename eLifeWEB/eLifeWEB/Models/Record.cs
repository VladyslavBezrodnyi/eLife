using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eLifeWEB.Models
{
    public class Record
    {
        public Record()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        public int Id{ get; set; }

        [DisplayName("Пацієнт")]
        public string PatientId { get; set; }

        [DisplayName("Лікар")]
        public string DoctorId { get; set; }

        [DisplayName("Назва послуги")]
        public int TypeId { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Дата")]
        public DateTime EndDate { get; set; }

        [DisplayName("Скарги")]
        public string Complaints { get; set; }

        [DisplayName("Об'єктивні дані")]
        public string ObjectiveData { get; set; }

        [DisplayName("Діагноз")]
        public string Diagnoses { get; set; }

        [DisplayName("Перебіг хвороби")]
        public string CourseOfTheDisease { get; set; }

        [DisplayName("Призначення лікаря")]
        public string Appointment { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual TypeOfService TypeOfService { get; set; }

        public virtual ApplicationUser Patient { get; set; }
    }
}