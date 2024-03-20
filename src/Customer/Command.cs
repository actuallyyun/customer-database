using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Customer
{
    public class Command
    {
        public Action Execute{get;set;}
        public Action UnExecute{get;set;}

        public Command(Action execute,Action unExecute){
            Execute=execute;
            UnExecute=unExecute;
        }
        public void undo(){
            Execute.Invoke();
        }

        public void redo(){
            UnExecute.Invoke();
        }
    }
}