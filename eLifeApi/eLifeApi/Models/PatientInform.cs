namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        public string Adress { get; set; }

        [Required]
        public string Activity { get; set; }

        public string BloodGroup { get; set; }

        public string Allergy { get; set; }

        public string Operations { get; set; }

        [Column("Infectious diseases")]
        public string Infectious_diseases { get; set; }

        public string Diabetes { get; set; }

        [Required]
        public string BankCard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
