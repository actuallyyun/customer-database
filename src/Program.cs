using src.Customer;

class Program
{
    public static CustomerDatabase SetUpTest()
    {
        string path = "src/customers.csv";
        var customerDB = new CustomerDatabase();
        customerDB.PopulateDatabase(path);
        Console.WriteLine($"customer db should have 1000 records: {customerDB.Count()}\n");
        return customerDB;
    }

    public static void TearDownDB(CustomerDatabase customerDB)
    {
        customerDB.ClearDB();
    }

    public static void TestAddCustomer()
    {
        Console.WriteLine("\n########Test AddCustomer#########\n");

        var customerDB = SetUpTest();

        var customer1 = new CustomerCreateDTO(
            "John",
            "Doe",
            "johndoe@example.com",
            "123 Main Street"
        );
        var customer2 = new CustomerCreateDTO(
            "Jane",
            "Smith",
            "johndoe@example.com",
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

        Console.WriteLine($"customer 1 should be found:\n {customer1Found}");

        Console.WriteLine(
            "\nCustomer 2 with the same email should not be added. Should print message: invalid email."
        );
        try
        {
            customerDB.AddCustomer(customer2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("\n########Test undo and redo AddCustomer#########\n");
        Console.WriteLine("Call Undo() customer 1 should be removed");
        customerDB.Undo();
        var c1Id = customer1Found.Id;
        Console.WriteLine(
            $"Find customer1 by id result should be null:{CustomerDatabase.SearchCustomerById(c1Id) is null}"
        );

        Console.WriteLine("\nCall Redo() should add customer 1 back");
        customerDB.Redo();
        Console.WriteLine(
            $"Find customer 1 by Email should return customer 1:\n{CustomerDatabase.FindCustomerByEmail(customer1.Email)}"
        );

        Console.WriteLine("\nCall Undo() again customer 1 should be removed");
        customerDB.Undo();
        Console.WriteLine(
            $"Find customer1 by id result should be null:{CustomerDatabase.SearchCustomerById(c1Id) is null}"
        );
        TearDownDB(customerDB);
    }

    public static void TestDeleteCustomer()
    {
        Console.WriteLine("\n########Test DeleteCustomer#########\n");
        var customerDB = SetUpTest();
        var customer1 = new CustomerCreateDTO(
            "John",
            "Doe",
            "johndoe@example.com",
            "123 Main Street"
        );

        customerDB.AddCustomer(customer1);
        Console.WriteLine(
            $"Before deleting,should have 1001 records: {customerDB.Count() == 1001}"
        );
        var customer1Found = CustomerDatabase.FindCustomerByEmail("johndoe@example.com");
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
        Console.WriteLine("Try to delete again, it should print error");
        try
        {
            customerDB.DeleteCustomer(customer1Found.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine($"\nTest Undo() remove. customer 1 should be added back");
        customerDB.Undo();
        Console.WriteLine(
            $"Customer 1 should be found by email:\n{CustomerDatabase.FindCustomerByEmail(customer1.Email)}"
        );
        Console.WriteLine($"\nTest Redo() remove. customer 1 should removed again");
        customerDB.Redo();
        Console.WriteLine(
            $"Customer 1 should not be found by email:\n{CustomerDatabase.FindCustomerByEmail(customer1.Email) is null}"
        );

        TearDownDB(customerDB);
    }

    public static void TestUpdateCustomer()
    {
        Console.WriteLine("\n########Test UpdateCustomer#########\n");
        var customerDB = SetUpTest();
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
            "123 Main Street"
        );
        var customerUpdate1 = new CustomerUpdateDTO("Jane", "Smith", null, null);

        customerDB.AddCustomer(customer1);

        var customerUpdateId = CustomerDatabase.FindCustomerByEmail("johndoe@example.com").Id;
        customerDB.UpdateCustomer(customerUpdateId, customerUpdate1);

        var customerAfterUpdate = CustomerDatabase.SearchCustomerById(customerUpdateId);
        Console.WriteLine(
            $"Customer1 should be updated:\nFirstName is Jane:{customerAfterUpdate.FirstName == "Jane"}"
        );

        Console.WriteLine("\nUndo Update should reverse customer1");
        customerDB.Undo();
        var customerAfterUndo = CustomerDatabase.SearchCustomerById(customerUpdateId);
        Console.WriteLine(
            $"Customer1 should be reversed:\nFirstName is John:{customerAfterUndo.FirstName == "John"}"
        );

        Console.WriteLine("\nRedo Update should change customer1 name to Jane");
        customerDB.Redo();
        var customerAfterRedo = CustomerDatabase.SearchCustomerById(customerUpdateId);
        Console.WriteLine(
            $"Customer1 should be updated again:\nFirstName is Jane:{customerAfterRedo.FirstName == "Jane"}"
        );

        Console.WriteLine(
            "\nUpdate with an exisitng email that belongs to another user should fail."
        );

        customerDB.AddCustomer(customer2);
        var customerUpdate2 = new CustomerUpdateDTO(null, null, "janesmith@example.com", null);
        Console.WriteLine("Should print: invalid email.");
        try
        {
            customerDB.UpdateCustomer(customerUpdateId, customerUpdate2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        TearDownDB(customerDB);
    }

    public static void Main()
    {
        TestAddCustomer();
        TestDeleteCustomer();
        TestUpdateCustomer();
    }
}
