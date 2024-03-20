using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Customer
{
    public class CustomerDatabase : IEnumerable<Customer>
    {
        private static HashSet<Customer> _customers = new();

        public IEnumerator<Customer> GetEnumerator()
        {
            foreach (var customer in _customers)
            {
                yield return customer;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static void InitiateDatabase(Customer customer){
            _customers.Add(customer);
        }
        public static void AddCustomer(CustomerCreateDTO customerToAdd)
        {
            if (isEmailFound(customerToAdd))
            {
                Console.WriteLine("Can not add customer.");
                return;
            }
            if (hasNullField(customerToAdd))
            {
                Console.WriteLine("Can not add customer. All fields must be provied.");
                return;
            }

            Customer newCustomer = new Customer(
                customerToAdd.FirstName,
                customerToAdd.LastName,
                customerToAdd.Email,
                customerToAdd.Address
            );
            _customers.Add(newCustomer);
            Console.WriteLine(
                $"Customer added. {customerToAdd.FirstName} {customerToAdd.LastName}"
            );
            return;
        }

        public static void DeleteCustomer(Guid id)
        {
            var foundCustomer = SearchCustomerById(id);
            
            if (foundCustomer is null)
            {
                Console.WriteLine("Cannot remove.");
            }

            _customers.Remove(foundCustomer);
            Console.WriteLine($"Customer with id:{id} removed.");
        }

        public static Customer? SearchCustomerById(Guid id)
        {
            return _customers.FirstOrDefault(customer => customer.Id == id);
        }

        public static bool isEmailFound(CustomerCreateDTO customerCreate)
        {
            var foundEmail = _customers.Select(customer =>
                customer.Email.ToLower() == customerCreate.Email.ToLower()
            );
            if (foundEmail is null)
                return false;
            return true;
        }

        public static bool hasNullField(CustomerCreateDTO customerCreate)
        {
            return customerCreate.FirstName is null
                | customerCreate.LastName is null
                | customerCreate.Address is null
                | customerCreate.Email is null;
        }


    }
}
