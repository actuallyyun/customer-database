using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Customer
{
    public class CustomerCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
         public string Email { get; set; }
        public string Address { get; set; }

        public CustomerCreateDTO(string firstName, string lastName, string email, string address)
        {
            FirstName=firstName;
            LastName=lastName;
            Email=email;
            Address=address;
        }
    }
    public class CustomerUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
         public string Email { get; set; }
        public string Address { get; set; }

        public CustomerUpdateDTO(string firstName, string lastName, string email, string address)
        {
            FirstName=firstName;
            LastName=lastName;
            Email=email;
            Address=address;
        }
    }
}