using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PDMEnginesApp.entity;
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
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PDMEngines;Trusted_Connection=True; MultipleActiveResultSets=True;");
        }

        public ICollection<Engine> GetEngines()
        {
            if (engines == null)
            {
                var enginesList = (from e in engines.Include(ca => ca.EngineComponentAmount)
                                   select e).ToList();
                if (!enginesList.IsNullOrEmpty())
                {
                    return enginesList;
                }
            }
            return engines.ToList<Engine>();
        }

        public ICollection<EngineComponent> GetComponents()
        {
            if (components == null)
            {
                var componentsList = (from e in components
                                      select e).ToList();
                if (!componentsList.IsNullOrEmpty())
                {
                    return componentsList;
                }
            }
            return components.ToList<EngineComponent>();
        }
        public ICollection<EngineComponentAmount> GetEngineComponentAmountsByEngine(Engine engine)
        {
            var engComAmounts = (from eca in engineComponentAmounts
                                 where eca.engineId == engine.id
                                 select eca).ToList();
            return engComAmounts;
        }

        public EngineComponent GetComponentByEngineComponentAmount(EngineComponentAmount eca)
        {
            var engComponent = (from c in components
                                where c.id == eca.componentId
                                select c).FirstOrDefault();
            return engComponent;
        }

        public ICollection<ComponentComponentAmount> GetComponentComponentAmountsByComponent(EngineComponent engComponent)
        {
            var compCompAmounts = (from cca in componentComponentAmounts
                                   where cca.firstComponentId == engComponent.id
                                   select cca).ToList();
            return compCompAmounts;
        }

        public EngineComponent GetComponentByComponentComponentAmount(ComponentComponentAmount cca, Engine engine)
        {
            var compComp = (from cc in components
                            where cc.id == cca.secondComponentId
                            && cca.engineId == engine.id
                            select cc).FirstOrDefault();
            return compComp;
        }

        public ICollection<Engine> InitEngines()
        {
            var enginesList = (from e in engines.Include(ca => ca.EngineComponentAmount)
                               select e).ToList();
            return enginesList;
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
            catch (InvalidOperationException)
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
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public bool AddEngine(string engineName)
        {
            //Проверка на наличие компонента в БД
            if (!EngineExsistCheck(engineName) && !ComponentExsistCheck(engineName))
            {
                engines.Add(new Engine { name = engineName });
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool AddComponentToEngine(string engineName, string componentName, string amountOfComponents)
        {
            var engine = GetEngine(engineName);

            //Проверка на наличие компонента в БД
            if (!EngineExsistCheck(componentName) && !ComponentExsistCheck(componentName))
            {
                // Добавляем новый компонент
                var newComponent = AddComponent(componentName);

                // Добавляем связь компонента и двигателя
                AddComponentEngineAmount(engine, amountOfComponents, newComponent);
                return true;
            }
            else
            {
                // Добавляем существующий компонент
                var exsistComponent = GetComponent(componentName);

                if (exsistComponent != null)
                {
                    var engCompAmount = (from eca in engineComponentAmounts
                                         where eca.engineId == engine.id
                                         && eca.componentId == exsistComponent.id
                                         select eca).FirstOrDefault();
                    if (engCompAmount == null)
                    {
                        // Добавляем связь компонента и двигателя
                        AddComponentEngineAmount(engine, amountOfComponents, exsistComponent);
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddComponentEngineAmount(Engine engine, string amountOfComponents, EngineComponent newComponent)
        {
            engine.EngineComponentAmount.Add(new EngineComponentAmount { engine = engine, component = newComponent, amount = Int32.Parse(amountOfComponents) });
            SaveChanges();
        }

        public bool AddComponentToComponent(string componentName, string nesterdComponentName, string amountOfComponents, string engineName)
        {
            var component = GetComponent(componentName);

            //Проверка на наличие компонента в БД
            if (!EngineExsistCheck(nesterdComponentName) && !ComponentExsistCheck(nesterdComponentName))
            {
                //TODO Когда добавляем существующий компонент верхнего уровня в компонент нижнего уровня
                // Добавляем новый компонент
                var newComponent = AddComponent(nesterdComponentName);

                // Добавляем связь компонента и компонента
                AddComponentComponentAmount(component, amountOfComponents, newComponent, engineName);
                return true;
            }
            else
            {
                var engine = GetEngine(engineName);
                // Берем существующий компонент
                var exsistComponent = GetComponent(nesterdComponentName);

                if (exsistComponent != null)
                {
                    var compCompAmount = (from cca in componentComponentAmounts
                                          where cca.engineId == engine.id
                                          && cca.firstComponentId == component.id
                                          && cca.secondComponentId == exsistComponent.id
                                          select cca).FirstOrDefault();
                    if (compCompAmount == null)
                    {
                        // Добавляем связь компонента и компонента
                        AddComponentComponentAmount(component, amountOfComponents, exsistComponent, engineName);
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddComponentComponentAmount(EngineComponent component, string amountOfComponents, EngineComponent newComponent, string engineName)
        {
            var engine = (from eng in engines
                          where eng.name == engineName
                          select eng).FirstOrDefault();

            if (engine != null)
            {
                component.ComponentComponentAmounts.Add(new ComponentComponentAmount
                {
                    firstComponentId = component.id,
                    secondComponentId = newComponent.id,
                    amount = Int32.Parse(amountOfComponents),
                    engineId = engine.id
                });
                SaveChanges();
            }
        }

        private EngineComponent AddComponent(string componentName)
        {
            var comp = components.Add(new EngineComponent { name = componentName }).Entity;
            SaveChanges();
            return comp;
        }

        public bool RenameEngine(string oldEngineName, string newEngineName)
        {
            if (!EngineExsistCheck(newEngineName) && !ComponentExsistCheck(newEngineName))
            {
                var engine = GetEngine(oldEngineName);
                engine.name = newEngineName;

                engines.Update(engine);
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool RenameComponent(string oldComponentName, string newComponentName)
        {
            if (!EngineExsistCheck(newComponentName) && !ComponentExsistCheck(newComponentName))
            {
                var component = GetComponent(oldComponentName);
                component.name = newComponentName;

                components.Update(component);
                SaveChanges();
                return true;
            }
            return false;
        }

        public void DeleteEngine(string engineName)
        {
            var engine = GetEngine(engineName);

            if (engine != null)
            {
                var engCompAmount = (from eca in engineComponentAmounts
                                     where eca.engineId == engine.id
                                     select eca).ToList();

                if (engCompAmount.Count > 0)
                {
                    foreach (EngineComponentAmount eca in engCompAmount)
                    {
                        var engineComponents = (from comp in components
                                                where eca.componentId == comp.id
                                                select comp).ToList();

                        if (engineComponents.Count > 0)
                        {
                            foreach (EngineComponent engComp in engineComponents)
                            {
                                DeleteComponent(engComp.name, 1);
                            }
                        }
                    }
                }
                engines.Remove(engine);
                SaveChanges();
            }
        }

        public void DeleteComponent(string componentName, int nestingLevel)
        {
            var component = GetComponent(componentName);

            if (nestingLevel == 1)
            {
                DeleteNestedToEngineComponent(component);
            }
            else if (nestingLevel == 2)
            {
                DeleteNestedToComponentComponent(component);
            }
        }

        private void DeleteNestedToEngineComponent(EngineComponent component)
        {
            var engCompAmountList = (from eca in engineComponentAmounts
                                     where eca.componentId == component.id
                                     select eca).ToList();

            var nestedComponentComponentAmountList = (from cca in componentComponentAmounts
                                                      where cca.firstComponentId == component.id
                                                      select cca).ToList();

            if (nestedComponentComponentAmountList.Count > 0)
            {
                foreach (ComponentComponentAmount cca in nestedComponentComponentAmountList)
                {
                    var nestedComponents = (from comp in components
                                            where comp.id == cca.secondComponentId
                                            select comp).ToList();

                    if (nestedComponents != null)
                    {
                        foreach (EngineComponent nestedCompoent in nestedComponents)
                        {
                            DeleteNestedToComponentComponent(nestedCompoent);
                        }
                    }

                }
            }

            // Если у компонента еще есть связи на вложенные компоненты, то не удаялем компонент, а просто удаляем его связь с двигателем
            if (engCompAmountList.Count() == 1)
            {
                components.Remove(component);
                SaveChanges();
            }
        }

        private void DeleteNestedToComponentComponent(EngineComponent component)
        {
            var compCompAmountList = (from cca in componentComponentAmounts
                                      where cca.secondComponentId == component.id
                                      select cca).ToList();

            if (compCompAmountList.Count() == 1)
            {
                components.Remove(component);
                SaveChanges();
            }

            componentComponentAmounts.Remove(compCompAmountList[0]);
            SaveChanges();
        }

        // Проверка на наличие дубликата двигателя
        private bool EngineExsistCheck(string engineName)
        {
            var engine = GetEngine(engineName);

            if (engine != null)
            {
                MessageBox.Show($"Элемент {engineName} уже существует в базе данных");
                return true;
            }
            return false;
        }

        // Проверка на наличие дубликата компонента
        private bool ComponentExsistCheck(string componentName)
        {
            var component = GetComponent(componentName);

            if (component != null)
            {
                MessageBox.Show($"Элемент {componentName} уже существует в базе данных");
                return true;
            }
            return false;
        }
    }
}
