
namespace src.Customer
{
    public class FileHelper
    {
        private readonly string _initialDataPath;

        public FileHelper(string path){
            _initialDataPath=path;
        }
        public List<string>? ReadFile()
        {
            try
            {
                using (var reader = new StreamReader(_initialDataPath))
                {
                    // first line is header, parse header
                    var header=  reader.ReadLine();
                    
                    // start reading data
                    string line;
                    List<string> data=[];
                    while ((line = reader.ReadLine()) is not null)
                    {
                        data.Add(line);
                        
                    }
                    return data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async static void WriteFileAsync(IEnumerable<Customer> customers){
            string path=Constants.GetDbPath();

            // if it doesn't exit, create the file and write data
            if(!File.Exists(path)){
                using (StreamWriter sw=File.CreateText(path)){
               
                    sw.WriteLine("Id,FirstName,LastName,Email,Address"); //write header. 
                    foreach(var customer in customers){
                        await sw.WriteLineAsync($"{customer.Id},{customer.FirstName},{customer.LastName},{customer.Email},{customer.Address}");
                    }
                }
            }
        }
    }
}
