using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLifeWEB.Utils
{
    public class ChatView
    {
        public string Date { set; get; }

        public int  Count { set; get; }

        public List<MessageDate> Messages { set; get; }
    }

    public class MessageDate
    {
        public string Name { set; get; }
        public string SenderId { set; get; }
        public string Text { set; get; }
        public string Time { set; get; }
    }
}