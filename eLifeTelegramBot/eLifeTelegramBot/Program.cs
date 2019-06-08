using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace eLifeTelegramBot
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("796199482:AAGf3DdpBw4oKV7uoPxGjAB0Ottv3Bd9-RI");
        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;


            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                var listOfParameters = e.Message.Text.Split(' ');

                if (e.Message.Text == "/help")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Для авторизації через пошту введіть команду /login <email> <password>");

                }

                if (listOfParameters[0] == "/login")
                {
                    if (listOfParameters.Length == 3)
                    {
                        var answer = LoginEmail(listOfParameters[1], listOfParameters[2]);
                        if (answer)
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Успішна авторизація.");
                            CheckTime(listOfParameters[1], e);
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(e.Message.Chat.Id, "Email або password були введені невірно.");
                        }
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(e.Message.Chat.Id, "Для того щоб переглянуті команди введіть /help");
                    }
                }
            }
        }

        private static bool LoginEmail(string email, string password)
        {
            var values = new Dictionary<string, string>
                          {
                             { "email", email },
                             { "password", password }
                          };

            var valuesJson = JsonConvert.SerializeObject(values);

            byte[] byteArray = Encoding.ASCII.GetBytes(valuesJson);

            try
            {
                WebRequest request = WebRequest.Create("https://localhost:44300/Telegram/Login");
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "POST";
                request.ContentLength = byteArray.Length;
                request.ContentType = "application/json";
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();

                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }

            return false;
        }


        private static void CheckTime(string email, MessageEventArgs e)
        {
                var data = email.Replace(".","!");

            try
            {
                string url = string.Format("https://localhost:44300/Telegram/GetRecords?model={0}", data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    if (response.StatusDescription == "OK")
                    {
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            data = reader.ReadToEnd();
                        }

                        var records = JsonConvert.DeserializeObject<List<RecordsData>>(FixApiResponseString(data));
                        string messege = "Ваші записи на завтрішній день: \n";

                        for (int i = 0; i < records.Count; i++)
                        {
                            int numberOfRecord = i + 1;
                            messege += numberOfRecord.ToString() + ") Час: " + records[i].Time.ToShortTimeString() + " " + "Клініка: " + records[i].ClinicName + " " + "Доктор: " + records[i].DoctorName + "\n";
                        }

                        Bot.SendTextMessageAsync(e.Message.Chat.Id, messege);
                        TimeDelay(e, 5, email);
                    }
            }
            catch
            {
                TimeDelay(e, 5, email);
            }
        }

        private static string FixApiResponseString(string input)
        {
            input = input.Replace("\\", string.Empty);
            input = input.Trim('"');
            return input;
        }

        private  static async void TimeDelay(MessageEventArgs e, int minutesToWait, string email)
        {
            await Task.Delay(minutesToWait * 60000);
            CheckTime(email, e);
        }

    }

    [Serializable]
    public class RecordsData
    {
        public DateTime Time { get; set; }
        public string Service { get; set; }
        public string DoctorName { get; set; }
        public string ClinicName { get; set; }
    }
}
