using PDMEnginesApp.model.entity;

namespace PDMEnginesApp.entity
{
    public class EngineComponent
    {
        public EngineComponent()
        {
            this.ComponentComponentAmounts = new HashSet<ComponentComponentAmount>();
            this.EngineComponentAmounts = new HashSet<EngineComponentAmount>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<ComponentComponentAmount> ComponentComponentAmounts { get; set; }
        public virtual ICollection<EngineComponentAmount> EngineComponentAmounts { get; set; }
    }
}
