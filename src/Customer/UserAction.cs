using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Customer
{
    public class UserAction
    {
        public string Type {get;set;}
        public object Parameter {get;set;}

        public UserAction(string type,object parameter){
            Type=type;
            Parameter=parameter;
        }

    }
}