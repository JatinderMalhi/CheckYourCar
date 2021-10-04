using System;
using System.ComponentModel.DataAnnotations;

namespace CheckYourCar.Models
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
