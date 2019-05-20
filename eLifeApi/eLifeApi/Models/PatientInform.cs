namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class PatientInform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PatientInform()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int PatientInfoId { get; set; }

        [Required]
        [DisplayName("������")]
        public string Adress { get; set; }

        [Required]
        [DisplayName("̳��� ������")]
        public string Activity { get; set; }

        [DisplayName("����� ����")]
        public string BloodGroup { get; set; }

        [DisplayName("����㳿")]
        public string Allergy { get; set; }

        [DisplayName("ճ������ ���������")]
        public string Operations { get; set; }

        [Column("Infectious diseases")]
        [DisplayName("��������� ������������")]
        public string Infectious_diseases { get; set; }

        [DisplayName("�������� �����")]
        public string Diabetes { get; set; }

        [Required]
        [DisplayName("��������� ������")]
        public string BankCard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
