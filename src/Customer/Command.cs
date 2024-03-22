using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace src.Customer
{
 // Command for undo function. execute undo, and unexecute undo.
    public class Command
    {
        public Action Execute{get;set;}
        public Action UnExecute{get;set;}

        public Command(Action execute,Action unExecute){
            Execute=execute;
            UnExecute=unExecute;
        }
        public void Undo(){
            Execute.Invoke();
        }

        public void Redo(){
            UnExecute.Invoke();
        }


    }
}