using PDMEnginesApp.entity;
using PDMEnginesApp.exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDMEnginesApp.presentation
{
    internal class PDMPresenter : IPDMPresenter
    {
        private readonly IPDMView view;
        private readonly IPDMService service;

        public PDMPresenter(IPDMView view, IPDMService service)
        {
            this.view = view;
            this.service = service;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void AddEngine()
        {
            var engineName = view.NameField;
            
            try
            {
                emptyNameFieldCheck(engineName);
                service.AddEngine(engineName);
                view.TreeView.Nodes.Add(engineName);

                MessageBox.Show("Двигатель добавлен");
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void AddComponent()
        {
            string componentName = view.NameField;
            string amountOfComponents = view.AmountField;

            try
            {
                selectedNodeCheck();
                emptyNameFieldCheck(componentName);
                emptyAmountFieldCheck(amountOfComponents);

                if (view.TreeView.SelectedNode.Level == 0)
                {
                    var enginename = view.TreeView.SelectedNode.Text;
                    var engine = service.GetEngine(enginename);

                    addComponentToEngine(engine, componentName, amountOfComponents);
                }
                else
                {
                    var selectedCompName = view.TreeView.SelectedNode.Text;
                    selectedCompName = selectedCompName.Split(',')[0];

                    var component = service.GetComponent(selectedCompName);

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
            service.AddComponentToEngine(engine, componentName, amountOfComponents);
            view.TreeView.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            MessageBox.Show("Компонент добавлен");
        }

        // Метод добавляет компонент к компоненту
        private void addComponentToComponent(EngineComponent component, string componentName, string amountOfComponents)
        {
            service.AddComponentToComponent(component, componentName, amountOfComponents);
            view.TreeView.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            MessageBox.Show("Компонент добавлен");
        }

        public void Rename()
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

        public void Delete()
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

        // Проверка на наличие выбранной ноды
        private void selectedNodeCheck()
        {
            if (view.TreeView.SelectedNode == null)
            {
                throw new NonSelectedNodeException();
            }
        }
    }
}
