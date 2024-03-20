using src.Customer;

class Program{
    public static void Main(){

        // read csv file and populate database
        var fh=new FileHelper();
        string path="src/customers.csv";
        fh.ReadFileAsync(path);

        var customerDB=new CustomerDatabase();
        Console.WriteLine($"Should have 1000 records: {customerDB.Count()}");
        //foreach(var c in customerDB){
        //    Console.WriteLine(c);
        //}

        var customer1 = new CustomerCreateDTO("John", "Doe", "johndoe@example.com", "123 Main Street");
        var customer2 = new CustomerCreateDTO("Jane", "Smith", "janesmith@example.com", "456 Elm Street");
        customerDB.AddCustomer(customer1);
        var customer1Found=CustomerDatabase.FindCustomerByEmail("johndoe@example.com");

        Console.WriteLine($"customer 1 should be added: {customer1Found}");
        customerDB.AddCustomer(customer2); //should print Can not add customer.

        // delete customer 1 in db
        Console.WriteLine($"Before deleting,should have 1001 records: {customerDB.Count()==1001}"); 
        customerDB.DeleteCustomer(customer1Found.Id);
        Console.WriteLine($"After deleting, should have 1000 records: {customerDB.Count()==1000}");
        var customer1afterDeleting=CustomerDatabase.FindCustomerByEmail("johndoe@example.com");
        Console.WriteLine($"After deleting, customer1 should not be found: {customer1afterDeleting is null}");
        // try to delete again, it should print error
        customerDB.DeleteCustomer(customer1Found.Id);//should print Cannot remove.

        customerDB.AddCustomer(customer1);
        var customerUpdateId=CustomerDatabase.FindCustomerByEmail("johndoe@example.com").Id;
        customerDB.UpdateCustomer(customerUpdateId,
            customer2
        );
        var customerAfterUpdate=CustomerDatabase.SearchCustomerById(customerUpdateId);
        Console.WriteLine($"Customer1 should be updated to customer 2:\n{customerAfterUpdate}");

        //try to update with an existing email that belongs to another user should fail
        // Creating another instance of CustomerUpdateDTO
        var customer3 = new CustomerCreateDTO("David", "Johnson", "davidjohnson@example.com", "789 Oak Avenue");
        customerDB.AddCustomer(customer3);

        var customer3Id=CustomerDatabase.FindCustomerByEmail("davidjohnson@example.com").Id;
        Console.WriteLine("Should print: invalid email.");
        customerDB.UpdateCustomer(customer3Id,customer2);
        
        



        






    }
}