using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.exception
{
    class NonSelectedNodeException : Exception
    {
        public NonSelectedNodeException() 
            : base()
        { 
            MessageBox.Show("Выберите двигатель или компонент для добавления");
        }
    }
}
