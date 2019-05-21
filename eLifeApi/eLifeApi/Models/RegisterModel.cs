using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace eLifeApi.Models
{
    [Serializable]
    public class RegisterModel
    {

        [Required]
        [DisplayName("ПІБ")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Електронна пошта")]
        public string Email { get; set; }

       

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public string Password { get; set; }

        
        [Required]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [DisplayName("Підтвердження паролю")]
        public string PasswordConfirm { get; set; }
        
    }
}