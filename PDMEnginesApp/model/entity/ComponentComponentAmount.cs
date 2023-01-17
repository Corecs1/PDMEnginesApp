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
