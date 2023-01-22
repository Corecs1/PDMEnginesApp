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
            try
            {
                string newName = view.NameField;
                string oldName = view.TreeView.SelectedNode.Text.Split(",")[0];
                
                selectedNodeCheck();
                emptyNameFieldCheck(newName);

                if (view.TreeView.SelectedNode.Level == 0)
                {
                    service.RenameEngine(oldName, newName);

                    view.TreeView.SelectedNode.Text = (newName);
                    MessageBox.Show("Двигатель изменен");
                }
                else
                {
                    service.RenameComponent(oldName, newName);

                    foreach (TreeNode tree in view.TreeView.Nodes)
                    {
                        if (view.TreeView.SelectedNode.Level == 1)
                        {
                            var node = (from n in tree.Nodes.Cast<TreeNode>()
                                        where n.Text.Split(",")[0].Equals(oldName)
                                        select n).FirstOrDefault();
                            var nodeArray = node.Text.Split(',');
                            node.Text = ($"{newName}, {nodeArray[1]}");

                        }
                        else if (view.TreeView.SelectedNode.Level == 2)
                        {
                            foreach (TreeNode nestedTree in tree.Nodes)
                            {
                                var node = (from n in nestedTree.Nodes.Cast<TreeNode>()
                                            where n.Text.Split(",")[0].Equals(oldName)
                                            select n).FirstOrDefault();
                                var nodeArray = node.Text.Split(',');
                                node.Text = ($"{newName}, {nodeArray[1]}");
                            }
                        }
                    }
                    MessageBox.Show("Компонент изменен");
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
    
        //TODO Сделать каскадное удаление
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
