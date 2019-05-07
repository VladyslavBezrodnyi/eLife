namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DoctorInform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DoctorInform()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        public string Specialization { get; set; }

        [Column("Category ")]
        [Required]
        public string Category_ { get; set; }

        [Required]
        public string Guardian { get; set; }

        public int Id_clinic { get; set; }

        [Required]
        public string Education { get; set; }

        [Required]
        public string Skills { get; set; }

        public bool Practiced { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] Photo { get; set; }

        public virtual Clinic Clinic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
