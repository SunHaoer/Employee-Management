using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVC.Models
{
    public class Employee
    {
        //[Remote(action: "Verify", controller: "Employees", AdditionalFields = "Id")]
        public int Id { get; set; }
        
        [StringLength(30)]
        [Required]
        public string FirstName { set; get; }

        [StringLength(30)]
        [Required]
        public string LastName { set; get; }

        [StringLength(30)]
        [Required]
        public string Gender { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Birth { set; get; }

        [StringLength(100)]
        public string Address { set; get; }

        [RegularExpression(@"^[1]\d{10,10}$", ErrorMessage = "抱歉，请填写正确Phone地址")]
        public long Phone { set; get; }
        
        [RegularExpression(@"^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$", ErrorMessage = "抱歉，请填写正确Email地址")]
        [Remote(action: "VerifyEmail", controller: "EmployeesVerify", AdditionalFields = "Id")]
        public string Email { set; get; }

        [StringLength(30)]
        public string Department { set; get; }
    }
}
