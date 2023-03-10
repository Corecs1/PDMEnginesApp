using Microsoft.IdentityModel.Tokens;
using PDMEnginesApp.entity;
using PDMEnginesApp.model.entity;

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
            var engines = service.InitEngines();

            if (!engines.IsNullOrEmpty())
            {
                foreach (Engine engine in engines)
                {
                    var parentNode = view.TreeView.Nodes.Add(engine.name);
                    InitializeComponents(engine, parentNode);
                }
            }
        }

        private void InitializeComponents(Engine engine, TreeNode parentNode)
        {
            var engCompAmount = service.GetEngineComponentAmountsByEngine(engine);

            if (!engCompAmount.IsNullOrEmpty())
            {
                foreach (EngineComponentAmount eca in engCompAmount)
                {
                    var engComponent = service.GetComponentByEngineComponentAmount(eca);
                    var childNode = parentNode.Nodes.Add($"{engComponent.name}, {eca.amount}");
                    var compCompAmounts = service.GetComponentComponentAmountsByComponent(engComponent);

                    if (!compCompAmounts.IsNullOrEmpty())
                    {
                        foreach (ComponentComponentAmount cca in compCompAmounts)
                        {
                            var compComponent = service.GetComponentByComponentComponentAmount(cca, engine);
                            if (compComponent != null)
                            {
                                childNode.Nodes.Add($"{compComponent.name}, {cca.amount}");
                            }
                        }
                    }
                }
            }
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
                    AddComponentToEngine(engineName, componentName, amountOfComponents);
                }
                else if (view.TreeView.SelectedNode.Level == 1)
                {
                    var selectedCompName = view.TreeView.SelectedNode.Text;
                    var engineName = view.TreeView.SelectedNode.Parent.ToString().Split(":")[1].Trim();
                    selectedCompName = selectedCompName.Split(',')[0];
                    AddComponentToComponent(selectedCompName, componentName, amountOfComponents, engineName);
                }
                else
                {
                    MessageBox.Show("Превышен лимит уровня вложенности компонентов");
                }
            }
            else
            {
                MessageBox.Show("Некорректное добавление компонента");
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
        private void AddComponentToComponent(string componentName, string nestedComponentName, string amountOfComponents, string engineName)
        {
            if (service.AddComponentToComponent(componentName, nestedComponentName, amountOfComponents, engineName))
            {
                AddTreeNodeComponent(nestedComponentName, amountOfComponents);
            }
        }

        private void AddTreeNodeComponent(string componentName, string amountOfComponents)
        {
            view.TreeView.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
            MessageBox.Show("Компонент добавлен");
        }

        public void Rename()
        {
            if (SelectedNodeCheck())
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

        public void Delete()
        {
            if (SelectedNodeCheck())
            {
                string name = view.TreeView.SelectedNode.Text.Split(",")[0];

                if (view.TreeView.SelectedNode.Level == 0)
                {
                    service.DeleteEngine(name);
                    DeleteTreeNode("Двигатель");
                }
                else if (view.TreeView.SelectedNode.Level == 1)
                {
                    service.DeleteComponent(name, 1);
                    DeleteTreeNode("Компонент");
                }
                else if (view.TreeView.SelectedNode.Level == 2)
                {
                    service.DeleteComponent(name, 2);
                    DeleteTreeNode("Компонент");
                }
            }
        }

        private void DeleteTreeNode(string message)
        {
            view.TreeView.SelectedNode.Remove();
            MessageBox.Show($"{message} удалён");
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
            if (amount == "" || amount == null || !int.TryParse(amount, out int number))
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
