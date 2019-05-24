using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eLifeWEB.Models
{
    public class ClinicAdmin
    {
        public ClinicAdmin()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id{ get; set; }

        [DisplayName("Клініка")]
        public int ClinicId { get; set; }

        public bool ClinicConfirmed { get; set; }

        public virtual Clinic Clinic { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}