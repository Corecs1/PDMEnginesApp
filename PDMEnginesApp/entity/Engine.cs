using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.entity
{
    public class Engine
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<EngineComponent> components { get; set; } = new ();
    }
}
