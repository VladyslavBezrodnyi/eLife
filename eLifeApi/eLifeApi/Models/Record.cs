namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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

        [DisplayName("�������")]
        public int Id_patient { get; set; }

        [DisplayName("˳���")]
        public int Id_doctor { get; set; }

        [DisplayName("����� �������")]
        public int Id_type { get; set; }

        [DisplayName("����")]
        public DateTime Date { get; set; }

        [DisplayName("������")]
        public string Complaints { get; set; }

        [DisplayName("��'������ ���")]
        public string ObjectiveData { get; set; }

        [DisplayName("ĳ�����")]
        public string Diagnoses { get; set; }

        [DisplayName("������ �������")]
        public string CourseOfTheDisease { get; set; }

        [DisplayName("����������� �����")]
        public string Appointment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        public virtual TypeOfService TypeOfService { get; set; }

        public virtual User User { get; set; }
    }
}
