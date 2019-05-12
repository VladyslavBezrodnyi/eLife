namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Feedback
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_doctor { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_patient { get; set; }

        public DateTime Date { get; set; }

        public int Stars { get; set; }

        public string Comment { get; set; }

        public virtual User Doctor { get; set; }

        public virtual User Patient { get; set; }
    }
}
