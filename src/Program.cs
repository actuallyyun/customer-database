using src.Customer;

class Program
{
    public static void Main()
    {
        // read csv file and populate database
        var fh = new FileHelper();
        string path = "src/customers.csv";
        fh.ReadFileAsync(path);

        Console.WriteLine("\n########Test read csv file.########\n");
        var customerDB = new CustomerDatabase();
        Console.WriteLine($"Should have 1000 records: {customerDB.Count()}");
        //foreach(var c in customerDB){
        //    Console.WriteLine(c);
        //}

        Console.WriteLine("\n########Test AddCustomer#########\n");

        var customer1 = new CustomerCreateDTO(
            "John",
            "Doe",
            "johndoe@example.com",
            "123 Main Street"
        );
        var customer2 = new CustomerCreateDTO(
            "Jane",
            "Smith",
            "janesmith@example.com",
            "456 Elm Street"
        );
        try
        {
            customerDB.AddCustomer(customer1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        var customer1Found = CustomerDatabase.FindCustomerByEmail("johndoe@example.com");

        Console.WriteLine($"customer 1 should be added: {customer1Found}");

        try
        {
            customerDB.AddCustomer(customer2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        } //should print Can not add customer.

        Console.WriteLine("\n########Test DeleteCustomer#########\n");

        // delete customer 1 in db
        Console.WriteLine(
            $"Before deleting,should have 1001 records: {customerDB.Count() == 1001}"
        );
        try
        {
            customerDB.DeleteCustomer(customer1Found.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine(
            $"After deleting, should have 1000 records: {customerDB.Count() == 1000}"
        );
        var customer1afterDeleting = CustomerDatabase.FindCustomerByEmail("johndoe@example.com");
        Console.WriteLine(
            $"After deleting, customer1 should not be found: {customer1afterDeleting is null}"
        );
        // try to delete again, it should print error
        try
        {
            customerDB.DeleteCustomer(customer1Found.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        //should print Cannot remove.


        Console.WriteLine("\n########Test UpdateCustomer#########\n");

        customerDB.AddCustomer(customer1);
        var customerUpdateId = CustomerDatabase.FindCustomerByEmail("johndoe@example.com").Id;
        customerDB.UpdateCustomer(customerUpdateId, customer2);
        var customerAfterUpdate = CustomerDatabase.SearchCustomerById(customerUpdateId);
        Console.WriteLine($"Customer1 should be updated to customer 2:\n{customerAfterUpdate}");

        //try to update with an existing email that belongs to another user should fail
        // Creating another instance of CustomerUpdateDTO
        var customer3 = new CustomerCreateDTO(
            "David",
            "Johnson",
            "davidjohnson@example.com",
            "789 Oak Avenue"
        );

        customerDB.AddCustomer(customer3);

        var customer3Id = CustomerDatabase.FindCustomerByEmail("davidjohnson@example.com").Id;
        Console.WriteLine("Should print: invalid email.");
        try
        {
            customerDB.UpdateCustomer(customer3Id, customer2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("\n########Test Undo #########\n");

        // undo add action
        customerDB.Undo();
        var customer3Found = CustomerDatabase.FindCustomerByEmail("davidjohnson@example.com");
        Console.WriteLine($"Cutomer 3 should not be found:{customer3Found is null}");
    }
}
