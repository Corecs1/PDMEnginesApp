using Microsoft.EntityFrameworkCore;
using PDMEnginesApp.config;
using PDMEnginesApp.exception;
using PDMEnginesApp.model.entity;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PDMEnginesApp
{
    public partial class PDMApp : Form
    {
        private DataBaseConfig db = new DataBaseConfig();

        public PDMApp()
        {
            InitializeComponent();
            initializeData();
        }

        // Метод берёт строки из базы данных и инициализирует в ноды treeView
        private void initializeData()
        {
            //private TreeNode parentNode;

            //var engines = (from engine in db.engines.Include(e => e.components)
            //             select engine).ToList();

            //foreach (Engine engine in engines)
            //{
            //    parentNode = PdmTree.Nodes.Add(engine.name);
            //    initializeComponent(engine, parentNode);
            //}
        }
        
        // Метод проходится по компонентам двигателя и инициализирует их в childTreeView
        private void initializeComponent(Engine engine, TreeNode parentNode)
        {
        //    TreeNode childNode = null;

        //    foreach (EngineComponent component in engine.components)
        //    {
        //        var component_components = (from p in db.components where p.componentId == component.id
        //                     select p).ToList();
        //        childNode = parentNode.Nodes.Add(component.name + ", " + component.amount);

        //        foreach (EngineComponent component_component in component_components)
        //        {
        //            childNode.Nodes.Add(component_component.name + ", " + component_component.amount);
        //        }
        //    }
        }

        // Метод добавляет двигатель в базу данных и Node
        private void addEngine(object sender, EventArgs e)
        {
            var engineName = NameField.Text;

            try 
            {
                emptyNameFieldCheck(engineName);
                dublicateEngineChech(engineName);

                db.engines.Add(new Engine { name = engineName });
                db.SaveChanges();
                PdmTree.Nodes.Add(engineName);
                MessageBox.Show("Двигатель добавлен");
            }
            catch(Exception ex)
            {
                return;
            }

        }

        // Метод добавляет компонент в базу данных и Node
        private void addComponent(object sender, EventArgs e)
        {
            string componentName = NameField.Text;
            string amountOfComponents = AmountField.Text;

            try
            {
                selectedNodeCheck();
                emptyNameFieldCheck(componentName);
                emptyAmountFieldCheck(amountOfComponents);

                if (PdmTree.SelectedNode.Level == 0)
                {
                    var enginename = PdmTree.SelectedNode.Text;
                    var engine = (from en in db.engines
                                  where en.name == enginename
                                  select en).First();

                    addComponentToEngine(engine, componentName, amountOfComponents);
                }
                else
                {
                    var selectedCompName = PdmTree.SelectedNode.Text;
                    selectedCompName = selectedCompName.Split(',')[0];

                    var component = (from comp in db.components
                                     where comp.name == selectedCompName
                                     select comp).First();

                    addComponentToComponent(component, componentName, amountOfComponents);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }


        // Метод добавляет компонент к двигателю
        private void addComponentToEngine(Engine engine, string componentName, string amountOfComponents)
        {
            // Проверка на наличие компонента в БД
            //try
            //{
            //    dublicateComponentChech(componentName);

            //    // Добавляем новый компонент
            //    var newComponent = new EngineComponent
            //    {
            //        name = componentName,
            //        amount = Int32.Parse(amountOfComponents),
            //    };

            //    db.components.Add(newComponent);
            //    engine.Components.Add(newComponent);
            //    db.SaveChanges();

            //    PdmTree.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            //    MessageBox.Show("Компонент добавлен");
            //} catch(Exception ex)
            //{
            //    // Добавляем существующий компонент
            //    var exsistComponent = (from c in db.components
            //                        where c.name == componentName
            //                        select c).First();

            //    engine.Components.Add(exsistComponent);
            //    db.SaveChanges();

            //    PdmTree.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            //    MessageBox.Show("Компонент добавлен");
            //}
        }


        // Метод добавляет компонент к компоненту
        private void addComponentToComponent(EngineComponent component, string componentName, string amountOfComponents)
        {
            // Проверка на наличие компонента в БД
            try
            {
                dublicateComponentChech(componentName);

                // Добавляем новый компонент
                //var newComponent = new EngineComponent
                //{
                //    name = componentName,
                //    amount = Int32.Parse(amountOfComponents),
                //};

                //db.components.Add(newComponent);
                //component.Components.Add(newComponent);
                //db.SaveChanges();

                //PdmTree.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
                //MessageBox.Show("Компонент добавлен");
            }
            catch (Exception ex)
            {
                // Добавляем существующий компонент
                //var exsistComponent = (from c in db.components
                //                       where c.name == componentName
                //                       select c).First();

                //component.Components.Add(exsistComponent);
                //db.SaveChanges();

                //PdmTree.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
                //MessageBox.Show("Компонент добавлен");
            }
        }

        // Метод переименовывает двигатель или компонент из базы данных и Node
        private void rename(object sender, EventArgs e)
        {
            //if (PdmTree.SelectedNode != null)
            //{
            //    if (PdmTree.SelectedNode.Level == 0)
            //    {
            //        string engineName = NameField.Text;

            //        if (engineName == "")
            //        {
            //            MessageBox.Show("Введите название двигателя");
            //            return;
            //        }

            //        var engName = PdmTree.SelectedNode.Text;
            //        var engine = (from en in db.engines
            //                      where en.name == engName
            //                      select en).First();
            //        engine.name = engineName;

            //        db.engines.Update(engine);
            //        db.SaveChanges();
            //        PdmTree.SelectedNode.Text = (engineName);
            //        MessageBox.Show("Двигатель изменен");
            //    }    
            //    else
            //    {
            //        string componentName = NameField.Text;
            //        string amountOfComponents = AmountField.Text;

            //        if (componentName == "")
            //        {
            //            MessageBox.Show("Введите название компонента");
            //            return;
            //        }
            //        else if (amountOfComponents == "")
            //        {
            //            MessageBox.Show("Укажите количество компонентов");
            //            return;
            //        }

            //        var compName = PdmTree.SelectedNode.Text;
            //        compName = compName.Split(',')[0];
            //        var component = (from comp in db.components
            //                         where comp.name == compName
            //                         select comp).First();
            //        component.name = componentName;
            //        component.amount = Int32.Parse(amountOfComponents);

            //        db.components.Update(component);
            //        db.SaveChanges();
            //        PdmTree.SelectedNode.Text = (componentName + ", " + amountOfComponents);
            //        MessageBox.Show("Компонент изменен");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Выберите поле для добавления компонента");
            //}
        }

        // Метод удаляет двигатель или компонент из базы данных и Node
        private void delete(object sender, EventArgs e)
        {
            //if (PdmTree.SelectedNode != null)
            //{
            //    if (PdmTree.SelectedNode.Level == 0)
            //    {
            //        var engineName = PdmTree.SelectedNode.Text;
            //        var engine = (from en in db.engines
            //                      where en.name == engineName
            //                      select en).First();

            //        db.engines.Remove(engine);
            //        db.SaveChanges();
            //        PdmTree.SelectedNode.Remove();
            //        MessageBox.Show("Двигатель удалён");
            //    }
            //    else
            //    {
            //        var componentName = PdmTree.SelectedNode.Text;
            //        componentName = componentName.Split(',')[0];
            //        var component = (from comp in db.components
            //                         where comp.name == componentName
            //                         select comp).First();
                    
            //        db.components.Remove(component);
            //        db.SaveChanges();
            //        PdmTree.SelectedNode.Remove();
            //        MessageBox.Show("Компонент удалён");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Выберите поле для добавления компонента");
            //}
        }

        // Проверка на пустое поле наименования
        private void emptyNameFieldCheck(string name) 
        {
            if (name == "" || name == null)
            {
                throw new EmptyFieldException("Введите название");
            }
        }

        // Проверка на пустое поле количества
        private void emptyAmountFieldCheck(string amount)
        {
            if (amount == "" || amount == null)
            {
                throw new EmptyFieldException("Введите количество");
            }
        }

        // Проверка на наличие дубликата двигателей
        private void dublicateEngineChech(string name) 
        {
            var engine = (from e in db.engines 
                        where e.name == name
                        select e).ToList();
            
            if (engine.Count != 0) 
            {
                throw new DublicateException(name);
            }
        }

        // Проверка на наличие дубликата компонентов
        private void dublicateComponentChech(string name)
        {
            var component = (from c in db.components
                         where c.name == name
                         select c).ToList();

            if (component.Count != 0)
            {
                throw new DublicateException(name);
            }
        }

        // Проверка на наличие выбранной ноды
        private void selectedNodeCheck() 
        {
            if (PdmTree.SelectedNode == null) 
            {
                throw new NonSelectedNodeException();
            }
        }
    }
}
