using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVC.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        public string FirstName { set; get; }

        [Required]
        public string LastName { set; get; }

        [Required]
        public string Gender { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birth { set; get; }

        public string Address { set; get; }

        [RegularExpression(@"^[1]\d{10,10}$")]
        public long Phone { set; get; }
    }
}
