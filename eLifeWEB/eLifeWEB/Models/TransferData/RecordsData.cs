using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLifeWEB.Models.TransferData
{
    [Serializable]
    public class RecordsData
    {
        public DateTime Time { get; set; }
        public string Service { get; set; }
        public string DoctorName { get; set; }
        public string ClinicName { get; set; }
    }
}