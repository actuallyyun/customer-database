namespace src.Customer
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Customer(string firstName, string lastName, string email, string address,Guid? id=null  )
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;            
            Id=id??Guid.NewGuid();
            
        }

        public void UpdateCustomer(CustomerUpdateDTO customerUpdate)
        {
            if (customerUpdate.FirstName is not null)
                FirstName = customerUpdate.FirstName;
            if (customerUpdate.LastName is not null)
                LastName = customerUpdate.LastName;
            if (customerUpdate.Email is not null)
                Email = customerUpdate.Email;
            if (customerUpdate.Address is not null)
                Address = customerUpdate.Address;
        }

        public override string ToString()
        {
            return $"Customer {Id}, FirstName {FirstName}, LastName {LastName}, Email {Email}, Address {Address}";
        }
    }
}