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

        public string DoctorId { get; set; }

        [DisplayName("Вид прийому")]
        public string Name { get; set; }

        [Column(TypeName = "money")]
        [DisplayName("Ціна")]
        public decimal Price { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        public virtual ApplicationUser Doctor { get; set; }
        
    }
}