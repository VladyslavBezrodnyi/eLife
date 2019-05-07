namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Conversation
    {
        [Key]
        public int Id_conversation { get; set; }

        public int Id_patient { get; set; }

        public int Id_doctor { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        public virtual User Patient { get; set; }

        public virtual User Doctor { get; set; }
    }
}
