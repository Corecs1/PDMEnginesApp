using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.entity
{
    public class EngineComponent
    {
        public int id { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public List<EngineComponent> components { get; set; } = new ();

        public int? engineId { get; set; }
        public Engine? engine { get; set; }

        public int? componentId { get; set; }
        public EngineComponent? component { get; set; }
    }
}
