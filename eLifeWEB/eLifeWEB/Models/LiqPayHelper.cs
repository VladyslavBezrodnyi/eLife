using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace eLifeWEB.Models
{
    public class LiqPayHelper
    {
        static private readonly string _private_key;
        static private readonly string _public_key;

        static LiqPayHelper()
        {
            _public_key = "sandbox_i34944618324";     // Public Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
            _private_key = "sandbox_MWpqfhZRpkwWJqE0AuDJbMM7GTQoMoys09NUf6Oc";    // Private Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
        }

        /// <summary>
        /// Сформувати дані для LiqPay (data, signature)
        /// </summary>
        /// <param name="order_id">Номер замовлення</param>
        /// <returns></returns>
        static public LiqPayP2PFormModel GetLiqPayModel(Payment payment, TypeOfService typeOfService, ApplicationUser Patient)
        {
            // Заповнюю дані для їх передачі для LiqPay
            var signature_source = new LiqPayP2P()
            {
                public_key = _public_key,
                version = 3,
                action = "p2p",
                amount = typeOfService.Price,
                receiver_card = new ApplicationDbContext().Users.FirstOrDefault(u => u.Id == payment.Record.DoctorId).DoctorInform.Clinic.BankCard,
                currency = "UAH",
                description = "Оплата замовлення",
                ip = HttpContext.Current.Request.UserHostAddress, 
                order_id = payment.order_id,
                sandbox = 1,
                language = "uk",
                result_url = "http://localhost:44300/DoctorInforms/AppointmentResult"
            };
            var json_string = JsonConvert.SerializeObject(signature_source);
            var data_hash = Convert.ToBase64String(Encoding.UTF8.GetBytes(json_string));
            var signature_hash = GetLiqPaySignature(data_hash);

            // Данні для передачі у в'ю
            var model = new LiqPayP2PFormModel();
            model.Data = data_hash;
            model.Signature = signature_hash;
            return model;
        }

        /// <summary>
        /// Формування сигнатури
        /// </summary>
        /// <param name="data">Json string з параметрами для LiqPay</param>
        /// <returns></returns>
        static public string GetLiqPaySignature(string data)
        {
            return Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(_private_key + data + _private_key)));
        }
    }
}