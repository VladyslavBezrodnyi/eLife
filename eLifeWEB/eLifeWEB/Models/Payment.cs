using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eLifeWEB.Models
{
    public class Payment
    {
        [Key]
        public string order_id { get; set; }

        public int RecordId { get; set; }

        public int payment_id { get; set; }

        public string liqpay_order_id { get; set; }

        public string currency { get; set; }

        [Column(TypeName = "money")]
        public decimal amount { get; set; }
        
        public string status { get; set; }

        public virtual Record Record { get; set; }
    }
}