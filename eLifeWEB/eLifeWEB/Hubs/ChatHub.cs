using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using eLifeWEB.Models;

namespace eLifeWEB.Hubs
{
    public class ChatHub : Hub
    {
        ApplicationDbContext db = new ApplicationDbContext();
        static Conversation conversation;
        static List<ConversationReply> messages;

        public void Hello()
        {
            Clients.All.hello();
        }

        // Отправка сообщений
        public void Send(string name, string message)
        {
            ConversationReply mess = new ConversationReply
            {
                Conversation = conversation,
                ReplyText = message,
                Time = DateTime.Now
            };
            db.ConversationReplies.Add(mess);
            db.SaveChangesAsync();
            messages.Add(mess);
            Clients.All.addMessage(name, message);
        }

        // Подключение нового пользователя
        public void Connect(string userPatient, string userDoctor)
        {
            var id = Context.ConnectionId;
            conversation = db.Conversations.FirstOrDefault(e => e.DoctorId == userDoctor && e.PatientId == userPatient);
            messages = db.ConversationReplies.Where(e => e.ConversationId == conversation.Id).OrderBy(e => e.Time).ToList();
            // Посылаем сообщение текущему пользователю
            Clients.Caller.onConnected(id,conversation.Patient.Id, conversation.Patient.Name, conversation.Patient);

            // Посылаем сообщение всем пользователям, кроме текущего
            Clients.AllExcept(id).onNewUserConnected(conversation.Patient.Id, conversation.Patient.Name);
        }

        //// Отключение пользователя
        //public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        //{
        //    var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
        //    if (item != null)
        //    {
        //        Users.Remove(item);
        //        var id = Context.ConnectionId;
        //        Clients.All.onUserDisconnected(id, item.Name);
        //    }

        //    return base.OnDisconnected(stopCalled);
        //}
    }
}