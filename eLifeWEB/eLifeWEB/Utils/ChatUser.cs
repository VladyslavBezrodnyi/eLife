using eLifeWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLifeWEB.Utils
{
    public class ChatUser
    {
        public string ConnectionId { get; set; }
        public Conversation ConversationUser { get; set; }
    }
}