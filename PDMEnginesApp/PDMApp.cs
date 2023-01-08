using Microsoft.EntityFrameworkCore;
using PDMEnginesApp.config;
using PDMEnginesApp.entity;

namespace PDMEnginesApp
{
    public partial class PDMApp : Form
    {
        private DataBaseConfig db = new DataBaseConfig();
        private TreeNode parentNode = null;

        public PDMApp()
        {
            InitializeComponent();
            initializeData();
        }

        // Метод берёт строки из базы данных и инициализирует в ноды treeView
        private void initializeData()
        {
            var engines = (from engine in db.engines.Include(e => e.components)
                         select engine).ToList();

            foreach (Engine engine in engines)
            {
                parentNode = PdmTree.Nodes.Add(engine.name);
                initializeComponent(engine, parentNode);
            }
        }
        
        // Метод проходится по компонентам двигателя и инициализирует из в childTreeView
        private void initializeComponent(Engine engine, TreeNode parentNode)
        {
            TreeNode childNode = null;

            foreach (EngineComponent component in engine.components)
            {
                var component_components = (from p in db.components where p.componentId == component.id
                             select p).ToList();
                childNode = parentNode.Nodes.Add(component.name + ", " + component.amount);

                foreach (EngineComponent component_component in component_components)
                {
                    childNode.Nodes.Add(component_component.name + ", " + component_component.amount);
                }
            }
        }

        // Метод добавляет двигатель в базу данных и Node
        private void addEngine(object sender, EventArgs e)
        {
            string engineName = NameField.Text;
            
            if (engineName == "")
            {
                MessageBox.Show("Введите название двигателя");
                return;
            }

            db.engines.Add(new Engine { name = engineName });
            db.SaveChanges();
            PdmTree.Nodes.Add(engineName);
            MessageBox.Show("Двигатель добавлен");
        }

        // Метод добавляет компонент в базу данных и Node
        private void addComponent(object sender, EventArgs e)
        {
            if (PdmTree.SelectedNode != null)
            {
                string componentName = NameField.Text;
                string amountOfComponents = AmountField.Text;

                if (componentName == "")
                {
                    MessageBox.Show("Введите название компонента");
                    return;
                }
                else if (amountOfComponents == "")
                {
                    MessageBox.Show("Укажите колличество компонентов");
                    return;
                }

                if (PdmTree.SelectedNode.Level == 0)
                {
                    var engineName = PdmTree.SelectedNode.Text;
                    var engine = (from en in db.engines
                                 where en.name == engineName
                                 select en).First();

                    db.components.Add(new EngineComponent
                    {
                        name = componentName,
                        amount = Int32.Parse(amountOfComponents),
                        engineId = engine.id
                    });
                    db.SaveChanges();
                    PdmTree.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
                    MessageBox.Show("Компонент добавлен");
                }
                else
                {
                    var compName = PdmTree.SelectedNode.Text;
                    compName = compName.Split(',')[0];

                    var component = (from comp in db.components
                                     where comp.name == compName
                                     select comp).First();

                    db.components.Add(new EngineComponent
                    {
                        name = componentName,
                        amount = Int32.Parse(amountOfComponents),
                        componentId = component.id
                    });
                    db.SaveChanges();
                    PdmTree.SelectedNode.Nodes.Add(componentName + ", " + amountOfComponents);
                    MessageBox.Show("Компонент добавлен");
                }
            } 
            else
            {
                MessageBox.Show("Выберите поле для добавления компонента");
            }
        }

        // Метод переименовывает компонент из базы данных и Node
        private void rename(object sender, EventArgs e)
        {
            if (PdmTree.SelectedNode != null)
            {
                if (PdmTree.SelectedNode.Level == 0)
                {
                    string engineName = NameField.Text;

                    if (engineName == "")
                    {
                        MessageBox.Show("Введите название двигателя");
                        return;
                    }

                    var engName = PdmTree.SelectedNode.Text;
                    var engine = (from en in db.engines
                                  where en.name == engName
                                  select en).First();
                    engine.name = engineName;

                    db.engines.Update(engine);
                    db.SaveChanges();
                    PdmTree.SelectedNode.Text = (engineName);
                    MessageBox.Show("Двигатель изменен");
                }    
                else
                {
                    string componentName = NameField.Text;
                    string amountOfComponents = AmountField.Text;

                    if (componentName == "")
                    {
                        MessageBox.Show("Введите название компонента");
                        return;
                    }
                    else if (amountOfComponents == "")
                    {
                        MessageBox.Show("Укажите колличество компонентов");
                        return;
                    }

                    var compName = PdmTree.SelectedNode.Text;
                    compName = compName.Split(',')[0];
                    var component = (from comp in db.components
                                     where comp.name == compName
                                     select comp).First();
                    component.name = componentName;
                    component.amount = Int32.Parse(amountOfComponents);

                    db.components.Update(component);
                    db.SaveChanges();
                    PdmTree.SelectedNode.Text = (componentName + ", " + amountOfComponents);
                    MessageBox.Show("Компонент изменен");
                }
            }
            else
            {
                MessageBox.Show("Выберите поле для добавления компонента");
            }
        }

        // Метод удаляет компонент из базы данных и Node
        private void delete(object sender, EventArgs e)
        {

        }
    }
}
