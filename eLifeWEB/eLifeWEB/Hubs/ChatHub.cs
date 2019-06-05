using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using eLifeWEB.Models;
using eLifeWEB.Utils;

namespace eLifeWEB.Hubs
{
    public class ChatHub : Hub
    {
        ApplicationDbContext db = new ApplicationDbContext();
        static List<ChatUser> conversations = new List<ChatUser>();

        public void Hello()
        {
            Clients.All.hello();
        }

        // Отправка сообщений
        public void Send(int conversationId, string senderId, string message)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                ConversationReply mess = new ConversationReply
                {
                    SenderId = senderId,
                    ReplyText = message,
                    Time = DateTime.Now,
                    ConversationId = conversationId
                };
                db.ConversationReplies.Add(mess);
                db.SaveChanges();
                var sender = db.Users.FirstOrDefault(e => e.Id == senderId);
                Clients.Caller.addMessage(mess.Time.ToString("dd.MM.yy hh:mm"), sender.Name, message);
            }
        }

        // Подключение нового пользователя
        public void Connect(string userPatient, string userDoctor)
        {
            var id = Context.ConnectionId;

            if (!conversations.Any(x => x.ConnectionId == id))
            {
                conversations.Add(new ChatUser{
                    ConnectionId = id,
                    ConversationUser = db.Conversations.FirstOrDefault(e => e.DoctorId == userDoctor && e.PatientId == userPatient)
                });

                // Посылаем сообщение текущему пользователю
                //Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                //Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
            
            // Посылаем сообщение текущему пользователю
           // Clients.Caller.onConnected(id,conversation.Patient.Id, conversation.Patient.Name, conversation.Patient);

            // Посылаем сообщение всем пользователям, кроме текущего
           // Clients.AllExcept(id).onNewUserConnected(conversation.Patient.Id, conversation.Patient.Name);
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = conversations.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                conversations.Remove(item);
                //var id = Context.ConnectionId;
                //Clients.All.onUserDisconnected(id, item.);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}