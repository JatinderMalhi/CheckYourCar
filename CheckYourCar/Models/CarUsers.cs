using System.ComponentModel.DataAnnotations;

namespace CheckYourCar.Models
{
    public class CarUsers
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CarMake { get; set; }
        public int CarModel { get; set; }



    }
}
