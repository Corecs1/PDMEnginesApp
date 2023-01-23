﻿using Microsoft.EntityFrameworkCore;
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
                    // Добавляем связь компонента и двигателя
                    AddComponentEngineAmount(engine, amountOfComponents, exsistComponent);
                    return true;
                }
            }
            return false;
        }

        private void AddComponentEngineAmount(Engine engine, string amountOfComponents, EngineComponent newComponent)
        {
            engine.EngineComponentAmount.Add(new EngineComponentAmount { engine = engine, component = newComponent, amount = Int32.Parse(amountOfComponents) });
            SaveChanges();
        }

        public bool AddComponentToComponent(string componentName, string nesterdComponentName, string amountOfComponents)
        {
            var component = GetComponent(componentName);

            //Проверка на наличие компонента в БД
            if (!EngineExsistCheck(nesterdComponentName) && !ComponentExsistCheck(nesterdComponentName))
            {
                //TODO Когда добавляем существующий компонент верхнего уровня в компонент нижнего уровня
                // Добавляем новый компонент
                var newComponent = AddComponent(nesterdComponentName);

                // Добавляем связь компонента и компонента
                AddComponentComponentAmount(component, amountOfComponents, newComponent);
                return true;
            }
            else
            {
                // Берем существующий компонент
                var exsistComponent = GetComponent(nesterdComponentName);

                if (exsistComponent != null)
                {
                    // Добавляем связь компонента и компонента
                    AddComponentComponentAmount(component, amountOfComponents, exsistComponent);
                    return true;
                }
            }
            return false;
        }

        private void AddComponentComponentAmount(EngineComponent component, string amountOfComponents, EngineComponent newComponent)
        {
            component.ComponentComponentAmounts.Add(new ComponentComponentAmount
            {
                firstComponentId = component.id,
                secondComponentId = newComponent.id,
                amount = Int32.Parse(amountOfComponents)
            });
            SaveChanges();
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

        // TODO Сделать каскадное удаление, если его компоенты нигде больше не используются
        public void DeleteEngine(string engineName)
        {
            var engine = GetEngine(engineName);
            if (engine != null)
            {
                engines.Remove(engine);
                SaveChanges();
            }
        }

        // TODO Сделать каскадное удаление, если его компоенты нигде больше не используются
        public void DeleteComponent(string componentName)
        {
            var component = GetComponent(componentName);
            if (component != null)
            {
                components.Remove(component);
                SaveChanges();
            }
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
