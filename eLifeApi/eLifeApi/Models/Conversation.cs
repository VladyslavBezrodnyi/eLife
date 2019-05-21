namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class Conversation
    {
        [Key]
        public int Id_conversation { get; set; }

        [DisplayName("Пацієнт")]
        public int Id_patient { get; set; }

        [DisplayName("Лікар")]
        public int Id_doctor { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Статус")]
        public bool Status { get; set; }

        public virtual User Patient { get; set; }

        public virtual User Doctor { get; set; }
    }
}
