using src.Customer;

class Program{
    public static void Main(){

        // read csv file and populate database
        var fh=new FileHelper();
        string path="src/customers.csv";
        fh.ReadFileAsync(path);

        var customerDB=new CustomerDatabase();
        foreach(var c in customerDB){
            Console.WriteLine(c);
        }
    }
}