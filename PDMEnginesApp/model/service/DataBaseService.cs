using Microsoft.EntityFrameworkCore;
using PDMEnginesApp.entity;
using PDMEnginesApp.exception;
using PDMEnginesApp.model.entity;

namespace PDMEnginesApp.model.service
{
    public class DataBaseService : DbContext, IPDMService
    {
        public DbSet<Engine> engines { get; set; } = null!;
        public DbSet<EngineComponent> components { get; set; } = null!;
        public DbSet<ComponentComponentAmount> componentComponentAmounts { get; set; } = null!;
        public DbSet<EngineComponentAmount> engineComponentAmounts { get; set; } = null!;

        public DataBaseService()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PDMEngines;Trusted_Connection=True; MultipleActiveResultSets=True;");
        }

        public ICollection<Engine> GetEngines()
        {
            throw new NotImplementedException();
        }

        public ICollection<EngineComponent> GetComponents()
        {
            throw new NotImplementedException();
        }
        public Engine GetEngine(string engineName)
        {
            try
            {
                var engine = (from e in engines
                              where e.name == engineName
                              select e).First();
                return engine;
            } 
            catch (InvalidOperationException ex) 
            {
                return null;
            }
        }

        public EngineComponent GetComponent(string componentName)
        {
            try
            {
                var component = (from c in components
                                 where c.name == componentName
                                 select c).First();
                return component;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        public void AddEngine(string engineName)
        {
            try
            {
                dublicateEngineCheck(engineName);
                engines.Add(new Engine { name = engineName });
                SaveChanges();
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException();
            }
        }
        public void AddComponentToEngine(Engine engine, string componentName, string amountOfComponents)
        {
            //Проверка на наличие компонента в БД
            try
            {
                dublicateComponentCheck(componentName);
                
                // Добавляем новый компонент
                var newComponent = AddComponent(componentName);
                components.Add(newComponent);

                // Добавляем связь компонента и двигателя
                engine.EngineComponentAmount.Add(new EngineComponentAmount { engine= engine, component=newComponent, amount = Int32.Parse(amountOfComponents) });
                SaveChanges();
            }
            catch (Exception ex)
            {
                // Добавляем существующий компонент
                var exsistComponent = GetComponent(componentName);

                // Добавляем связь компонента и двигателя
                engine.EngineComponentAmount.Add(new EngineComponentAmount { engine = engine, component = exsistComponent, amount = Int32.Parse(amountOfComponents) });
                SaveChanges();
            }
        }

        public void AddComponentToComponent(EngineComponent component, string componentName, string amountOfComponents)
        {
            //Проверка на наличие компонента в БД
            //TODO Сделать маппинг по объектам firstComponent, secondComponent
            try
            {
                dublicateComponentCheck(componentName);

                // Добавляем новый компонент
                var newComponent = AddComponent(componentName);
                components.Add(newComponent);

                // Добавляем связь компонента и компонента
                component.ComponentComponentAmounts.Add(new ComponentComponentAmount { firstComponentId = component.id, secondComponent = newComponent, amount = Int32.Parse(amountOfComponents) });
                SaveChanges();
            }
            catch (Exception ex)
            {
                // Добавляем существующий компонент
                var exsistComponent = GetComponent(componentName);

                // Добавляем связь компонента и двигателя
                component.ComponentComponentAmounts.Add(new ComponentComponentAmount { firstComponentId = component.id, secondComponent = exsistComponent, amount = Int32.Parse(amountOfComponents) });
                SaveChanges();
            }
        }

        private EngineComponent AddComponent(string componentName)
        {
            return components.Add(new EngineComponent { name = componentName}).Entity;
        }

        public void RenameEngine(string engineName)
        {
            throw new NotImplementedException();
        }

        public void RenameComponent(string componentName, string amount)
        {
            throw new NotImplementedException();
        }

        public void DeleteEngine(string engineName)
        {
            throw new NotImplementedException();
        }

        public void DeleteComponent(string componentName, string amount)
        {
            throw new NotImplementedException();
        }

        // Проверка на наличие дубликата двигателя
        private void dublicateEngineCheck(string engineName)
        {
            var engine = GetEngine(engineName);

            if (engine != null)
            {
                throw new DublicateException(engineName);
            }
        }

        // Проверка на наличие дубликата компонента
        private void dublicateComponentCheck(string componentName)
        {
            var component = GetComponent(componentName);

            if (component != null)
            {
                throw new DublicateException(componentName);
            }
        }
    }
}
