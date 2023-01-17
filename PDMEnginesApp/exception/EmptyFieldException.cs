using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.exception
{
    class EmptyFieldException : Exception
    {
        public EmptyFieldException(string message) 
            : base(message)
        { 
            MessageBox.Show(message);
        }
    }
}
