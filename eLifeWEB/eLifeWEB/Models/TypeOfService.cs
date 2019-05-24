using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLifeWEB.Models
{
    public class TypeOfService
    {
        public TypeOfService()
        {
            Records = new HashSet<Record>();
        }

        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string DoctorId { get; set; }

        [Column(TypeName = "money")]
        [DisplayName("Ціна")]
        public decimal Price { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        public virtual ApplicationUser Doctor { get; set; }

        public virtual Type Type { get; set; }
    }
}