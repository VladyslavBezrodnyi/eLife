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
            _public_key = "i43342429875";     // Public Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
            _private_key = "flbcNuWijYAQb827jepw5C28uZVSwNrSE0eRXobi";    // Private Key компанії, який можна знайти в особистому кабінеті на сайті liqpay.ua
        }

        /// <summary>
        /// Сформувати дані для LiqPay (data, signature)
        /// </summary>
        /// <param name="order_id">Номер замовлення</param>
        /// <returns></returns>
        static public LiqPayP2PFormModel GetLiqPayModel(Record record, TypeOfService typeOfService, ApplicationUser Patient/*,  string card_cvv, int card_exp_month, int card_exp_year*/)
        {
            // Заповнюю дані для їх передачі для LiqPay
            var signature_source = new LiqPayP2P()
            {
                public_key = _public_key,
                version = 3,
                action = "p2p",
                amount = typeOfService.Price,
                //phone = Patient.PhoneNumber,
                //card = Patient.PatientInform.BankCard,
                receiver_card = new ApplicationDbContext().Users.FirstOrDefault(u => u.Id == record.DoctorId).DoctorInform.Clinic.BankCard,
                currency = "UAH",
                description = "Оплата замовлення",
                ip = HttpContext.Current.Request.UserHostAddress, 
                order_id = "151588",//record.Id.ToString(),
                sandbox = 0,
                result_url = "http://localhost:44300/AppointentResult"
                //card_cvv = card_cvv,
                //card_exp_month = card_exp_month,
                //card_exp_year = card_exp_year
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