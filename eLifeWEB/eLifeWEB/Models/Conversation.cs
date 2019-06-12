using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eLifeWEB.Models
{
    public class Conversation
    {
        public Conversation()
        {
            ConversationReplies = new HashSet<ConversationReply>();
        }

        [Key]
        public int Id { get; set; }

        [DisplayName("Пацієнт")]
        public string PatientId { get; set; }

        [DisplayName("Лікар")]
        public string DoctorId { get; set; }

        [DisplayName("Дата")]
        public DateTime Date { get; set; }

        [DisplayName("Статус")]
        public bool Status { get; set; }

        public virtual ApplicationUser Patient { get; set; }

        public virtual ApplicationUser Doctor { get; set; }


        public virtual ICollection<ConversationReply> ConversationReplies { get; set; }
    }
}