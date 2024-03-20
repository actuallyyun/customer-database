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
        public async void ReadFileAsync(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
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
    }
}
