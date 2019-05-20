namespace eLifeApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Serializable]
    public partial class ConversationReply
    {
        [Key]
        public int Id_reply { get; set; }

        [DisplayName("ĳ����")]
        public int Id_conversation { get; set; }

        [Required]
        [DisplayName("�����")]
        public string ReplyText { get; set; }

        [DisplayName("����")]
        public DateTime Time { get; set; }

        [DisplayName("³��������")]
        public int? Id_sender { get; set; }

        public virtual User User { get; set; }
    }
}
