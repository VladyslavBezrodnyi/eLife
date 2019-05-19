using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;

namespace eLifeApi.Models
{
    public class RegisterDoctorModel
    {
        [Required]
        public string Specialization { get; set; }
        
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

    }
}