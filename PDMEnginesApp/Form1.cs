using MySql.Data.MySqlClient;
using PDMEnginesApp.config;
using System.Data;
using System.Xml.Linq;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace PDMEnginesApp
{
    public partial class Form1 : Form
    {
        private DataBase dataBase = new DataBase();
        private DataTable table = new DataTable();
        private MySqlDataAdapter adapter = new MySqlDataAdapter();
        private MySqlCommand command;
        private TreeNode parentNode = null;

        public Form1()
        {
            InitializeComponent();
            initializeData();

            var root = treeView1.Nodes[0];
            var collection = root.Nodes;
            collection[0].Text = "gjkdjfghdfkjg";
        }

        // Метод пробегается по базе данных и заполняет Node строками из БД
        private void initializeData()
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            command = new MySqlCommand("SELECT * FROM pdm_engines.engines;", dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            foreach (DataRow row in table.Rows)
            {
                parentNode = treeView1.Nodes.Add(row["name"].ToString());
                populateTreeView(Convert.ToInt32(row["id"].ToString()), parentNode);
            }
        }

        // Метод пробегается по компонентам
        private void populateTreeView(int parentId, TreeNode parentNode)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            String query = "SELECT (name) FROM pdm_engines.components AS comp JOIN pdm_engines.engine_component AS eng_comp ON eng_comp.engine_id =" + parentId + " WHERE comp.id = eng_comp.component_id;";
            adapter = new MySqlDataAdapter(query, dataBase.getConnection());
            adapter.Fill(table);
            TreeNode childNode;
            foreach (DataRow row in table.Rows)
            {
                if (parentNode == null)
                    childNode = treeView1.Nodes.Add(row["name"].ToString());
                else
                    childNode = parentNode.Nodes.Add(row["name"].ToString());
            }
        }

  
        // Метод добавляет двигатель в БД и Node
        private void addEngine(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            command = new MySqlCommand("INSERT INTO `pdm_engines`.`engines` (name) VALUES (@name);", dataBase.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameField.Text;
            adapter.SelectCommand = command;

            dataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                treeView1.Nodes.Add(nameField.Text);
                MessageBox.Show("Двигатель добавлен");
            } 
            else
            {
                MessageBox.Show("Двигатель не добавлен");
            }

            dataBase.closeConnection(); 
        }

        // Метод добавляет компонент в БД и Node
        private void addComponent(object sender, EventArgs e)
        {
            
            /*
                Проверка, добавлем ли мы компонент к двигателю или к компоненту
                (Проверка содержится ли SelectedNode в RootNode)
            */

            if (treeView1.SelectedNode.Nodes != null)
            {
                var xui = treeView1.SelectedNode.FullPath;
                String[] path = xui.Split('\\');

                MessageBox.Show(path.Length.ToString());

                foreach (String lol in path)
                {
                    MessageBox.Show(lol.ToString());
                }

     

                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                command = new MySqlCommand("INSERT INTO `pdm_engines`.`components` (name, amount) VALUES (@name, @amount);", dataBase.getConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameField.Text;
                command.Parameters.Add("@amount", MySqlDbType.VarChar).Value = amountField.Text;
                adapter.SelectCommand = command;

                // SELECT id по параметрам name и amount двигателя из БД
                command = new MySqlCommand("SELECT * FROM `pdm_engines`.`engines` WHERE `pdm_engines`.`engines`.name = @name", dataBase.getConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = treeView1.SelectedNode.Nodes.ToString();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                DataRow rows = table.Rows[0];
                String engineId = rows["id"].ToString();

                // SELECT id по параметрам name и amount компонента из БД
                command = new MySqlCommand("SELECT * FROM `pdm_engines`.`components` WHERE `pdm_engines`.`components`.name = @name", dataBase.getConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameField.Text;
                adapter.SelectCommand = command;
                adapter.Fill(table);
                DataRow rows1 = table.Rows[0];
                String componentId = rows1["id"].ToString();

                // INSERT INTO engine_component engine_id & component_id
                command = new MySqlCommand("INSERT INTO `pdm_engines`.`engine_component` VALUES (@engine_id, @component_id);", dataBase.getConnection());
                //command.Parameters.Add("@engine_id", MySqlDbType.Int64).Value;
                command.Parameters.Add("@component_id", MySqlDbType.Int64).Value = componentId;
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            else
            {
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                command = new MySqlCommand("INSERT INTO `pdm_engines`.`components` (name, amount) VALUES (@name, @amount);", dataBase.getConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameField.Text;
                command.Parameters.Add("@amount", MySqlDbType.VarChar).Value = amountField.Text;
                adapter.SelectCommand = command;
                // SELECT id по параметрам name и amount компонента из БД
                // INSERT INTO engine_component engine_id & component_id
            }

            TreeNode node = new TreeNode(nameField.Text);
            try
            { 
                var xui = treeView1.SelectedNode.Nodes.Add(node);
            } catch (Exception ex) 
            { 
                MessageBox.Show("Выберите строку для добавления компонента");
            }
        }

        // Метод переименовывает двигатель или компонент
        private void rename(object sender, EventArgs e)
        {
            try
            {
                treeView1.SelectedNode.Text = nameField.Text;
            } catch (Exception ex)
            {
                MessageBox.Show("Выберите строку для изменения названия");
            } 
        }

        // Метод удаляет двигатель или компонент
        private void remove(object sender, EventArgs e)
        {
            try
            {
                treeView1.SelectedNode.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Выберите строку для удаления");
            }
        }
    }
}