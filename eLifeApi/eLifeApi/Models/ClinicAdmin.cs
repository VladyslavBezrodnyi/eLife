namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class ClinicAdmin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClinicAdmin()
        {
            Users = new HashSet<User>();
        }

        [Key]
        public int Id_clinic_admin { get; set; }

        [DisplayName(" Î≥Ì≥Í‡")]
        public int Id_clinic { get; set; }

        public bool ClinicConfimed { get; set; }

        public virtual Clinic Clinic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
