using MySql.Data.MySqlClient;
using PDMEnginesApp.config;
using PDMEnginesApp.entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;

namespace PDMEnginesApp
{
    public partial class PDMApp : Form
    {
        private DataBaseConfig db = new DataBaseConfig();
        private TreeNode parentNode = null;


        public PDMApp()
        {
            InitializeComponent();
            initializeDataBase();
        }

        // Метод инициализирует базу данных
        private void initializeDataBase()
        {
            Engine engine1 = new Engine { name = "engine_1" };
            Engine engine2 = new Engine { name = "engine_2" };
            Engine engine3 = new Engine { name = "engine_3" };

            EngineComponent component1 = new EngineComponent { name = "component_1", amount = 2, engine = engine1 };
            EngineComponent component2 = new EngineComponent { name = "component_2", amount = 5, engine = engine2 };
            EngineComponent component3 = new EngineComponent { name = "component_3", amount = 1, engine = engine2 };

            EngineComponent component_component1 = new EngineComponent { name = "component_component_1", amount = 4, component = component2};
            EngineComponent component_component2 = new EngineComponent { name = "component_component_2", amount = 8, component = component2};

            db.engines.AddRange(engine1, engine2, engine3);
            db.components.AddRange(component1, component2, component3, component_component1, component_component2);
            db.SaveChanges();

            initializeData();
        }

        // Метод берёт строки из базы данных и инициализирует в ноды treeView
        private void initializeData()
        {
            foreach (Engine engine in db.engines)
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
                if (parentNode != null)
                {
                    childNode = parentNode.Nodes.Add(component.name + ", " + component.amount);
                    if (component.components != null) 
                    {
                        foreach (EngineComponent childComponent in component.components)
                        {
                            if (childComponent != null)
                            {
                                childNode.Nodes.Add(childComponent.name + ", " + childComponent.amount);
                            }
                        }
                    }
                }
                else
                {
                    childNode = PdmTree.Nodes.Add(engine.name);
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

                // Добавление мне сразу вернет объект компонента и я могу дёрнуть у него id
                //db.components.Add(new EngineComponent { name = componentName, amount = Int32.Parse(amountOfComponents) });

                //string nodeName = PdmTree.SelectedNode.Name;

                // 0 - это двигатели, 1 - компоненты двигателей и т.д
                // по вложенности можно определить: Если 0, то ищем объект по названию в PdmTree, если 1 и выше то...
                MessageBox.Show(PdmTree.SelectedNode.Level.ToString());

            } 
            else
            {
                MessageBox.Show("Выберите поле для добавления компонента");
            }
        }

        // Метод переименовывает компонент из базы данных и Node
        private void rename(object sender, EventArgs e)
        {

        }

        // Метод удаляет компонент из базы данных и Node
        private void delete(object sender, EventArgs e)
        {

        }
    }
}
