using PDMEnginesApp.entity;
using PDMEnginesApp.exception;

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
                EmptyNameFieldCheck(engineName);
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
                SelectedNodeCheck();
                EmptyNameFieldCheck(componentName);
                EmptyAmountFieldCheck(amountOfComponents);

                if (view.TreeView.SelectedNode.Level == 0)
                {
                    var enginename = view.TreeView.SelectedNode.Text;
                    var engine = service.GetEngine(enginename);

                    AddComponentToEngine(engine, componentName, amountOfComponents);
                }
                else
                {
                    var selectedCompName = view.TreeView.SelectedNode.Text;
                    selectedCompName = selectedCompName.Split(',')[0];

                    var component = service.GetComponent(selectedCompName);

                    AddComponentToComponent(component, componentName, amountOfComponents);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        // Метод добавляет компонент к двигателю
        private void AddComponentToEngine(Engine engine, string componentName, string amountOfComponents)
        {
            service.AddComponentToEngine(engine, componentName, amountOfComponents);
            AddTreeNodeComponent(componentName, amountOfComponents);
        }

        // Метод добавляет компонент к компоненту
        private void AddComponentToComponent(EngineComponent component, string componentName, string amountOfComponents)
        {
            service.AddComponentToComponent(component, componentName, amountOfComponents);
            AddTreeNodeComponent(componentName, amountOfComponents);
        }

        private void AddTreeNodeComponent(string componentName, string amountOfComponents)
        {
            view.TreeView.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            MessageBox.Show("Компонент добавлен");
        }

        public void Rename()
        {
            try
            {
                string newName = view.NameField;
                string oldName = view.TreeView.SelectedNode.Text.Split(",")[0];
                
                SelectedNodeCheck();
                EmptyNameFieldCheck(newName);

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
                            ReplaceTreeNodeComponentName(newName, oldName, tree);
                        }
                        else if (view.TreeView.SelectedNode.Level == 2)
                        {
                            foreach (TreeNode nestedTree in tree.Nodes)
                            {
                                ReplaceTreeNodeComponentName(newName, oldName, nestedTree);
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

        private void ReplaceTreeNodeComponentName(string newName, string oldName, TreeNode nestedTree)
        {
            var node = (from n in nestedTree.Nodes.Cast<TreeNode>()
                        where n.Text.Split(",")[0].Equals(oldName)
                        select n).FirstOrDefault();
            if (node != null)
            {
                var nodeArray = node.Text.Split(',');
                node.Text = ($"{newName}, {nodeArray[1]}");
            }
        }

        //TODO Удаление компонента должно приводить к удалению всех вложенных компонентов, если они не включены в другие составы.
        public void Delete()
        {
            try
            {
                SelectedNodeCheck();
                string name = view.TreeView.SelectedNode.Text.Split(",")[0];

                if (view.TreeView.SelectedNode.Level == 0)
                {
                    service.DeleteEngine(name);

                    view.TreeView.SelectedNode.Remove();
                    MessageBox.Show("Двигатель удален");
                }
                else
                {
                    service.DeleteComponent(name);

                    foreach (TreeNode tree in view.TreeView.Nodes)
                    {
                        if (view.TreeView.SelectedNode.Level == 1)
                        {
                            DeleteTreeNodeComponent(name, tree);
                        }
                        else if (view.TreeView.SelectedNode.Level == 2)
                        {
                            foreach (TreeNode nestedTree in tree.Nodes)
                            {
                                DeleteTreeNodeComponent(name, tree);
                            }
                        }
                    }
                    MessageBox.Show("Компонент удалён");
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void DeleteTreeNodeComponent(string name, TreeNode tree)
        {
            var node = (from n in tree.Nodes.Cast<TreeNode>()
                        where n.Text.Split(",")[0].Equals(name)
                        select n).FirstOrDefault();
            if (node != null)
            {
                node.Remove();
            }
        }

        // Проверка на пустое поле наименования
        private void EmptyNameFieldCheck(string name)
        {
            if (name == "" || name == null)
            {
                throw new EmptyFieldException("Введите название");
            }
        }

        // Проверка на пустое поле количества
        private void EmptyAmountFieldCheck(string amount)
        {
            if (amount == "" || amount == null)
            {
                throw new EmptyFieldException("Введите количество");
            }
        }

        // Проверка на наличие выбранной ноды
        private void SelectedNodeCheck()
        {
            if (view.TreeView.SelectedNode == null)
            {
                throw new NonSelectedNodeException();
            }
        }
    }
}
