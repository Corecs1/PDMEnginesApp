﻿using Microsoft.EntityFrameworkCore;
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
                DublicateEngineCheck(engineName);
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
                DublicateEngineCheck(componentName);
                DublicateComponentCheck(componentName);
                
                // Добавляем новый компонент
                var newComponent = AddComponent(componentName);

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
            try
            {
                DublicateEngineCheck(componentName);
                DublicateComponentCheck(componentName);
                //TODO Когда добавляем существующий компонент верхнего уровня в компонент нижнего уровня

                // Добавляем новый компонент
                var newComponent = AddComponent(componentName);

                // Добавляем связь компонента и компонента
                component.ComponentComponentAmounts.Add(new ComponentComponentAmount { firstComponentId = component.id, secondComponentId = newComponent.id, amount = Int32.Parse(amountOfComponents) });
                SaveChanges();
            }
            catch (Exception ex)
            {
                // Берем существующий компонент
                var exsistComponent = GetComponent(componentName);

                // Добавляем связь компонента и компонента
                component.ComponentComponentAmounts.Add(new ComponentComponentAmount { firstComponentId = component.id, secondComponentId = exsistComponent.id, amount = Int32.Parse(amountOfComponents) });
                SaveChanges();
            }
        }

        private EngineComponent AddComponent(string componentName)
        {
            var comp = components.Add(new EngineComponent { name = componentName }).Entity;
            SaveChanges();
            return comp;
        }

        public void RenameEngine(string oldEngineName, string newEngineName)
        {
            try
            {
                DublicateEngineCheck(newEngineName);
                DublicateComponentCheck(newEngineName);

                var engine = GetEngine(oldEngineName);
                engine.name = newEngineName;

                engines.Update(engine);
                SaveChanges();
            } catch (Exception ex)
            {
                throw new InvalidOperationException();
            }
        }

        public void RenameComponent(string oldComponentName, string newComponentName)
        {
            try
            {
                DublicateEngineCheck(newComponentName);
                DublicateComponentCheck(newComponentName);

                var component = GetComponent(oldComponentName);
                component.name = newComponentName;

                components.Update(component);
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException();
            }

        }

        // TODO Сделать каскадное удаление, если его компоенты нигде больше не используются
        public void DeleteEngine(string engineName)
        {
            var engine = GetEngine(engineName);
            engines.Remove(engine);
            SaveChanges();
        }

        // TODO Сделать каскадное удаление, если его компоенты нигде больше не используются
        public void DeleteComponent(string componentName)
        {
            var component = GetComponent(componentName);
            components.Remove(component);
            SaveChanges();
        }

        // Проверка на наличие дубликата двигателя
        private void DublicateEngineCheck(string engineName)
        {
            var engine = GetEngine(engineName);

            if (engine != null)
            {
                throw new DublicateException(engineName);
            }
        }

        // Проверка на наличие дубликата компонента
        private void DublicateComponentCheck(string componentName)
        {
            var component = GetComponent(componentName);

            if (component != null)
            {
                throw new DublicateException(componentName);
            }
        }
    }
}
