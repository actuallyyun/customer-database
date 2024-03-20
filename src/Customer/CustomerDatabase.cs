
using System.Collections;

namespace src.Customer
{
    public class CustomerDatabase : IEnumerable<Customer>
    {
        private static HashSet<Customer> _customers = new();   

        private Stack<Action>_undoStack=new();
        private Stack<Action>_redoStack=new();

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
            
            Action undo=()=>_customers.Remove(newCustomer);
            _undoStack.Push(undo);
            //UserAction.AddAction(new UserAction("add",newCustomer));
            
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
            Action undo=()=>_customers.Add(foundCustomer);
            _undoStack.Push(undo);
            //UserAction.AddAction(new UserAction("delete",id));

            Console.WriteLine($"Customer with id:{id} removed.");
        }


        public bool UpdateCustomer(Guid id, CustomerCreateDTO customerUpdate)
        {
            var foundCustomer = SearchCustomerById(id);
            var originalCustomer=foundCustomer;
            if (foundCustomer is null)
            {
                Console.WriteLine("Cannot update.");
                return false;
            }
            try{
                foundCustomer.UpdateCustomer(customerUpdate);
                            //UserAction.AddAction(new UserAction("update",id));
                Action undo=()=>foundCustomer.UpdateCustomer(new CustomerCreateDTO(originalCustomer.FirstName,
                originalCustomer.LastName,originalCustomer.Email,originalCustomer.Address));
                _undoStack.Push(undo);

            Console.WriteLine($"Updated successfully.\n{customerUpdate}");
            return true;
                }catch(Exception e){
                    Console.WriteLine(e.Message);
                    return false;
            };

        }

        public void Undo(){
            _undoStack.TryPop(out Action action);
            if(action is not null){
                _redoStack.Push(action);
                action.Invoke();
            }
           
        }

        public void Redo(){
            // implement redo
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
