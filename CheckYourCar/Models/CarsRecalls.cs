using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBusTransportation.Models
{
    public class CarsRecall
    {
        [Key]
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int ModelID { get; set; }
        public DateTime RecallDate { get; set; }
        public string Issues { get; set; }



    }
}
