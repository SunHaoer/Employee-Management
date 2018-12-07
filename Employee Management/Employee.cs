using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management
{
    class Employee
    {
        private int id;
        private string firstName;
        private string lastName;
        private Gender gender;
        private DateTime birth;
        private string address;
        private long phone;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        internal Gender Gender { get; set; }
        public DateTime Birth { get; set; }
        public string Address { get; set; }
        public long Phone { get; set; }

        public Employee()
        {
        }

        override
        public string ToString()
        {
            string result = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", Id, FirstName, LastName, Gender, Birth, Address, Phone);
            return result;
        }
    }
}
