namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class DoctorInform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DoctorInform()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("Спеціалізація")]
        public string Specialization { get; set; }

        [Column("Category ")]
        [Required]
        [DisplayName("Категорія")]
        public string Category_ { get; set; }

        [Required]
        [DisplayName("Стаж")]
        public string Guardian { get; set; }

        [DisplayName("Клініка")]
        public int Id_clinic { get; set; }

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
        public byte[] Photo { get; set; }

        public virtual Clinic Clinic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
