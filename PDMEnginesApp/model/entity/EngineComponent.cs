using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.entity
{
    public class EngineComponent
    {
        public EngineComponent()
        {
            this.Components = new HashSet<ComponentComponentAmount>();
            this.Engines = new HashSet<EngineComponentAmount>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<ComponentComponentAmount> Components { get; set; }
        public virtual ICollection<EngineComponentAmount> Engines { get; set; }
    }
}
