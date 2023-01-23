using PDMEnginesApp.entity;

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

        public void InitData()
        {
            //private TreeNode parentNode;

            //var engines = (from engine in db.engines.Include(e => e.components)
            //             select engine).ToList();

            //foreach (Engine engine in engines)
            //{
            //    parentNode = PdmTree.Nodes.Add(engine.name);
            //    initializeComponents(engine, parentNode);
            //}
            throw new NotImplementedException();
        }

        private void InitializeComponents(Engine engine, TreeNode parentNode)
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

        public void AddEngine()
        {
            var engineName = view.NameField;

            if (EmptyNameFieldCheck(engineName))
            {
                if (service.AddEngine(engineName))
                {
                    view.TreeView.Nodes.Add(engineName);
                    MessageBox.Show("Двигатель добавлен");
                }
            }
        }

        public void AddComponent()
        {
            string componentName = view.NameField;
            string amountOfComponents = view.AmountField;

            if (EmptyNameFieldCheck(componentName) && EmptyAmountFieldCheck(amountOfComponents) && SelectedNodeCheck())
            {
                if (view.TreeView.SelectedNode.Level == 0)
                {
                    var engineName = view.TreeView.SelectedNode.Text;
                    //var engine = service.GetEngine(enginename);

                    AddComponentToEngine(engineName, componentName, amountOfComponents);
                }
                else
                {
                    var selectedCompName = view.TreeView.SelectedNode.Text;
                    selectedCompName = selectedCompName.Split(',')[0];

                    //var component = service.GetComponent(selectedCompName);
                    AddComponentToComponent(selectedCompName, componentName, amountOfComponents);
                }
            }
        }

        // Метод добавляет компонент к двигателю
        private void AddComponentToEngine(string engineName, string componentName, string amountOfComponents)
        {
            if (service.AddComponentToEngine(engineName, componentName, amountOfComponents))
            {
                AddTreeNodeComponent(componentName, amountOfComponents);
            }
        }

        // Метод добавляет компонент к компоненту
        private void AddComponentToComponent(string componentName, string nestedComponentName, string amountOfComponents)
        {
            if (service.AddComponentToComponent(componentName, nestedComponentName, amountOfComponents))
            {
                AddTreeNodeComponent(componentName, amountOfComponents);
            }
        }

        private void AddTreeNodeComponent(string componentName, string amountOfComponents)
        {
            view.TreeView.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            MessageBox.Show("Компонент добавлен");
        }

        public void Rename()
        {
            string newName = view.NameField;
            string oldName = view.TreeView.SelectedNode.Text.Split(",")[0];

            if (SelectedNodeCheck() && EmptyNameFieldCheck(newName))
            {
                if (view.TreeView.SelectedNode.Level == 0)
                {
                    if (service.RenameEngine(oldName, newName))
                    {
                        view.TreeView.SelectedNode.Text = (newName);
                        MessageBox.Show("Двигатель изменен");
                    }
                }
                else
                {
                    if (service.RenameComponent(oldName, newName))
                    {
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
            if (SelectedNodeCheck())
            {
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
        private bool EmptyNameFieldCheck(string name)
        {
            if (name == "" || name == null)
            {
                MessageBox.Show("Введите название");
                return false;
            }
            return true;
        }

        // Проверка на пустое поле количества
        private bool EmptyAmountFieldCheck(string amount)
        {
            if (amount == "" || amount == null)
            {
                MessageBox.Show("Введите количество");
                return false;
            }
            return true;
        }

        // Проверка на наличие выбранной ноды
        private bool SelectedNodeCheck()
        {
            if (view.TreeView.SelectedNode == null)
            {
                MessageBox.Show("Выберите двигатель или компонент для добавления");
                return false;
            }
            return true;
        }
    }
}
