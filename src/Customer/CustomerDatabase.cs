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

        private static Stack <UserAction>_userActions=new();
        

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
        public  void AddCustomer(CustomerCreateDTO customerToAdd)
        {
            if (FindCustomerByEmail(customerToAdd.Email) is not null)
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
            _userActions.Push(new UserAction("AddCustomer",newCustomer));
            
            return;
        }

        public void DeleteCustomer(Guid id)
        {
            var foundCustomer = SearchCustomerById(id);
            
            if (foundCustomer is null)
            {
                Console.WriteLine("Cannot remove.");
                return;
            }

            _customers.Remove(foundCustomer);
            _userActions.Push(new UserAction("DeleteCustomer",id));

            Console.WriteLine($"Customer with id:{id} removed.");
        }


        public bool UpdateCustomer(Guid id, CustomerCreateDTO customerUpdate)
        {
            var foundCustomer = SearchCustomerById(id);
            if (foundCustomer is null)
            {
                Console.WriteLine("Cannot update.");
                return false;
            }
            try{
                foundCustomer.UpdateCustomer(customerUpdate);
                            _userActions.Push(new UserAction("UpdateCustomer",id));

            Console.WriteLine($"Updated successfully.\n{customerUpdate}");
            return true;
                }catch(Exception e){
                    Console.WriteLine(e.Message);
                    return false;
            };

        }
        public static Customer? SearchCustomerById(Guid id)
        {
            return _customers.FirstOrDefault(customer => customer.Id == id);
        }

        public static Customer? FindCustomerByEmail(string email)
        {
            return   _customers.FirstOrDefault(customer =>
                customer.Email.ToLower() == email.ToLower());

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
