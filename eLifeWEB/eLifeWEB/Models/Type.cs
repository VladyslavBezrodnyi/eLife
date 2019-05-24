using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLifeWEB.Models
{
    public class Type
    {
        public Type()
        {
            TypeOfServices = new HashSet<TypeOfService>();
        }

        [Key]
        public int Id { get; set; }

        [Column("Type")]
        [Required]
        [DisplayName("Назва послуги")]
        public string Type1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TypeOfService> TypeOfServices { get; set; }
    }
}