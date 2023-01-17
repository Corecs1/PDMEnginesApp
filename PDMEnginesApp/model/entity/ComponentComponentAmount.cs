using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.model.entity
{
    public class ComponentComponentAmount
    {
        public int id { get; set; }
        public int firstComponentId { get; set; }
        public int secondComponentId { get; set; }
        public int amount { get; set; }

        //public virtual ICollection<EngineComponent> firstComponent { get; set; }
        //public virtual ICollection<EngineComponent> secondComponent { get; set; }

    }
}
