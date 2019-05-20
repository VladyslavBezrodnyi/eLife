namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Clinic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clinic()
        {
            ClinicAdmins = new HashSet<ClinicAdmin>();
            DoctorInforms = new HashSet<DoctorInform>();
        }

        [Key]
        public int Id_clinic { get; set; }

        [Required]
        [DisplayName("Назва")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClinicAdmin> ClinicAdmins { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DoctorInform> DoctorInforms { get; set; }
    }
}
