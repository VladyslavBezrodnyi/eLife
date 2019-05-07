namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ConversationReply
    {
        [Key]
        public int Id_reply { get; set; }

        public int Id_conversation { get; set; }

        [Required]
        public string ReplyText { get; set; }

        public DateTime Time { get; set; }

        public int? Id_sender { get; set; }

        public virtual User User { get; set; }
    }
}
