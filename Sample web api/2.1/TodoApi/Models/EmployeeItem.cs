using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class EmployeeItem
    {
        public long Id { get; set; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string Gender { set; get; }

        public DateTime Birth { set; get; }

        public string Email { set; get; }

        public string Address { set; get; }

        public long Phone { set; get; }

        public string Department { set; get; }

        
    }
}