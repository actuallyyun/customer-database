
namespace src.Customer
{
    public class Constants
    {
        private static readonly string DB_FILE_PATH="src/customers_db.csv";

        public static string GetDbPath(){
            return DB_FILE_PATH;
        }
    }
}