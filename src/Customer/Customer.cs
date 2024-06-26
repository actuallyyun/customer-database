namespace src.Customer
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Customer(
            string firstName,
            string lastName,
            string email,
            string address,
            Guid? id = null
        )
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            Id = id ?? Guid.NewGuid();
        }

        public static Customer CreateCustomerFromFile(string[] customerData)
        {
            Guid id = Guid.Parse(customerData[0]);
            Customer customer = new Customer(
                customerData[1],
                customerData[2],
                customerData[3],
                customerData[4],
                id
            );
            return customer;
        }

        public static Customer CreateCustomer(CustomerCreateDTO customerCreate)
        {
            return new Customer(
                customerCreate.FirstName,
                customerCreate.LastName,
                customerCreate.Email,
                customerCreate.Address
            );
        }

        public void UpdateCustomer(CustomerUpdateDTO customerUpdate)
        {
            if (customerUpdate.FirstName is not null)
                FirstName = customerUpdate.FirstName;
            if (customerUpdate.LastName is not null)
                LastName = customerUpdate.LastName;

            if (customerUpdate.Address is not null)
                Address = customerUpdate.Address;

            if (customerUpdate.Email is not null)
            {
                if (
                    customerUpdate.Email.ToLower() == Email.ToLower()
                    || CustomerDatabase.FindCustomerByEmail(customerUpdate.Email) is null
                )
                {
                    Email = customerUpdate.Email;
                    return;
                }
                else
                {
                    throw new ExceptionHandler.InvalidEmailException("Invalid email.");
                }
            }
        }

        public override string ToString()
        {
            return $"Customer {Id}, FirstName {FirstName}, LastName {LastName}, Email {Email}, Address {Address}";
        }
    }
}
