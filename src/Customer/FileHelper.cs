using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;

namespace src.Customer
{
    public class FileHelper
    {
        private readonly string _initialDataPath;

        public FileHelper(string path){
            _initialDataPath=path;
        }
        public async void ReadFileAsync()
        {
            try
            {
                using (var reader = new StreamReader(_initialDataPath))
                {
                    // first line is header, parse header
                    var header= reader.ReadLine();
                    
                    string line;
                    while ((line = reader.ReadLine()) is not null)
                    {

                        string[] args = line.Split(",");
                        Guid id= Guid.Parse(args[0]);
                        Customer customer = new Customer(args[1], args[2], args[3],args[4],id);
                        CustomerDatabase.InitiateDatabase(customer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public async static void WriteFileAsync(IEnumerable<Customer> customers){
            string path="src/customers_db.csv";

            // if it doesn't exit, create the file and write data
            if(!File.Exists(path)){
                using (StreamWriter sw=File.CreateText(path)){
                    string header=$"";
                    sw.WriteLine("Id,FirstName,LastName,Email,Address"); //write header. hardcoded, should get it from customers
                    foreach(var customer in customers){
                        await sw.WriteLineAsync($"{customer.Id},{customer.FirstName},{customer.LastName},{customer.Email},{customer.Address}");
                    }
                }
            }
        }
    }
}
