namespace PDMEnginesApp.entity
{
    public class Engine
    {
        public Engine()
        {
            this.EngineComponentAmount = new HashSet<EngineComponentAmount>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public virtual ICollection<EngineComponentAmount> EngineComponentAmount { get; set; }
    }
}
