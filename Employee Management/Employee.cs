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

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        internal Gender Gender { get => gender; set => gender = value; }
        public DateTime Birth { get => birth; set => birth = value; }
        public string Address { get => address; set => address = value; }
        public long Phone { get => phone; set => phone = value; }

        public Employee(int id, string firstName, string lastName, Gender gender, DateTime birth, string address, long phone)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.Birth = birth;
            this.Address = address;
            this.Phone = phone;
        }

        public Employee()
        {
        }

        override
        public string ToString()
        {
            string result = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", Id, FirstName, LastName, Gender, Birth, Address, phone);
            return result;
        }
    }
}
