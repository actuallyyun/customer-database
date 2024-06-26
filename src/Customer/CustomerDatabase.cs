using System.Collections;

namespace src.Customer
{
    public class CustomerDatabase : IEnumerable<Customer>
    {
        private static HashSet<Customer> _customers = new();

        private static Stack<Command> _undoStack = new();
        private static Stack<Command> _redoStack = new();

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

        public void ClearDB()
        {
            _customers.Clear();
        }

        // populate db with csv data
        public void PopulateDatabase(string path)
        {
            var fh = new FileHelper(path);
            var data = fh.ReadFile();
            if (data is not null)
            {
                // transform data to Customer instance and add to db
                foreach (var line in data)
                {
                    string[] customerData = line.Split(",");
                    Customer customer = Customer.CreateCustomerFromFile(customerData);
                    _customers.Add(customer);
                }
                return;
            }else{
                throw new Exception("Load data failed.");
            }
             
        }

        public void AddCustomer(CustomerCreateDTO customerToAdd)
        {
            if (FindCustomerByEmail(customerToAdd.Email) is not null)
            {
                throw new ExceptionHandler.InvalidEmailException("Invalid email");
            }
            if (hasNullField(customerToAdd))
            {
                throw new ExceptionHandler.NonNullableFieldException("Field must not be null.");
            }

            Customer newCustomer = Customer.CreateCustomer(customerToAdd);
            _customers.Add(newCustomer);
            Console.WriteLine(
                $"Customer added. {customerToAdd.FirstName} {customerToAdd.LastName}"
            );

            Action undo = () => _customers.Remove(newCustomer);
            Action redo = () => AddCustomer(customerToAdd);
            var command = new Command(undo, redo);
            _redoStack.Clear();
            _undoStack.Push(command);

            UpdateDB();
            return;
        }

        public void DeleteCustomer(Guid id)
        {
            var foundCustomer = SearchCustomerById(id);

            if (foundCustomer is null)
            {
                throw new ExceptionHandler.CustomerNotFoundException("Customer does not exit");
            }

            _customers.Remove(foundCustomer);
            Action undo = () => _customers.Add(foundCustomer);
            Action redo = () => _customers.Remove(foundCustomer);
            _redoStack.Clear();
            _undoStack.Push(new Command(undo, redo));

            UpdateDB();
            Console.WriteLine($"Customer with id:{id} removed.");
        }

        public bool UpdateCustomer(Guid id, CustomerUpdateDTO customerUpdate)
        {
            var foundCustomer = SearchCustomerById(id);
            // Make a copy of the original customer for undo action
            var originalCustomer = new Customer(
                foundCustomer.FirstName,
                foundCustomer.LastName,
                foundCustomer.Email,
                foundCustomer.Address,
                foundCustomer.Id
            );

            if (foundCustomer is null)
            {
                throw new ExceptionHandler.CustomerNotFoundException("Customer does not exit");
            }
            try
            {
                foundCustomer.UpdateCustomer(customerUpdate);

                Action undo = () =>
                    foundCustomer.UpdateCustomer(
                        new CustomerUpdateDTO(
                            originalCustomer.FirstName,
                            originalCustomer.LastName,
                            originalCustomer.Email,
                            originalCustomer.Address
                        )
                    );
                Action redo = () => foundCustomer.UpdateCustomer(customerUpdate);
                _redoStack.Clear();
                _undoStack.Push(new Command(undo, redo));

                UpdateDB();
                Console.WriteLine($"Updated successfully.");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            ;
        }

        public void Undo()
        {
            _undoStack.TryPop(out Command command);
            if (command is not null)
            {
                command.Undo();
                _redoStack.Push(command);
            }
        }

        public void Redo()
        {
            _redoStack.TryPop(out Command command);
            if (command is not null)
            {
                command.Redo();
                _undoStack.Push(command);
            }
        }

        // update db after each data munipulatation
        public static void UpdateDB()
        {
            try
            {
                FileHelper.WriteFileAsync(_customers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static Customer? SearchCustomerById(Guid id)
        {
            return _customers.FirstOrDefault(customer => customer.Id == id);
        }

        public static Customer? FindCustomerByEmail(string email)
        {
            return _customers.FirstOrDefault(customer =>
                customer.Email.ToLower() == email.ToLower()
            );
        }

        public static bool hasNullField(CustomerCreateDTO customerCreate)
        {
            return customerCreate.FirstName is null
                | customerCreate.LastName is null
                | customerCreate.Address is null
                | customerCreate.Email is null;
        }

        // method for debugging undo function
        public static void PrintUndoStack()
        {
            Console.WriteLine("\nUndo stack");
            foreach (var command in _undoStack)
            {
                Console.WriteLine(command);
            }
        }
    }
}
