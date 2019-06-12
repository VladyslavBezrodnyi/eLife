using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eLifeWEB.Models
{
    public class ConversationReply
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Діалог")]
        public int ConversationId { get; set; }

        [Required]
        [DisplayName("Текст")]
        public string ReplyText { get; set; }

        [DisplayName("Дата")]
        public DateTime Time { get; set; }

        [DisplayName("Відправник")]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}