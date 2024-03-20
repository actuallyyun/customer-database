namespace src.Customer
{
    public class Customer
    {
        public Customer(Guid id, string lastName, string address) 
        {
            this.Id = id;
    this.LastName = lastName;
    this.Address = address;
   
        }
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

        public void UpdateCustomer(CustomerCreateDTO customerUpdate)
        {
            if (customerUpdate.FirstName is not null)
                FirstName = customerUpdate.FirstName;
            if (customerUpdate.LastName is not null)
                LastName = customerUpdate.LastName;
            
            if (customerUpdate.Address is not null)
                Address = customerUpdate.Address;
                
            if (customerUpdate.Email is not null && customerUpdate.Email.ToLower()!=Email.ToLower()&&CustomerDatabase.FindCustomerByEmail(customerUpdate.Email) is null)
            {
                Email = customerUpdate.Email;
                return;
            }else{
                throw new Exception("Invalid email.");
            }
        }

        public override string ToString()
        {
            return $"Customer {Id}, FirstName {FirstName}, LastName {LastName}, Email {Email}, Address {Address}";
        }
    }
}
