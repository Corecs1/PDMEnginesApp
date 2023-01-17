namespace PDMEnginesApp.entity
{
    public class EngineComponentAmount
    {
        public int id { get; set; }
        public int engineId { get; set; }
        public int componentId { get; set; }
        public int amount { get; set; }
        public virtual Engine engine { get; set; }
        public virtual EngineComponent component { get; set; }
    }
}
