namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Payment
    {
        [Key]
        public int Id_payment { get; set; }

        public int Id_record { get; set; }

        public int Id { get; set; }

        [Required]
        public string Signature { get; set; }

        public bool State { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Ref { get; set; }

        [Column(TypeName = "money")]
        public decimal Amf { get; set; }

        [Required]
        public string Ccy { get; set; }

        [Column(TypeName = "money")]
        public decimal Comis { get; set; }

        [Required]
        public string Code { get; set; }

        public virtual Record Record { get; set; }
    }
}
