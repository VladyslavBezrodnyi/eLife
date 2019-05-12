namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Record()
        {
            Payments = new HashSet<Payment>();
        }

        [Key]
        public int Id_record { get; set; }

        public int Id_patient { get; set; }

        public int Id_doctor { get; set; }

        public int Id_type { get; set; }

        public DateTime Date { get; set; }

        public string Complaints { get; set; }

        public string ObjectiveData { get; set; }

        public string Diagnoses { get; set; }

        public string CourseOfTheDisease { get; set; }

        public string Appointment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        public virtual TypeOfService TypeOfService { get; set; }

        public virtual User User { get; set; }
    }
}
