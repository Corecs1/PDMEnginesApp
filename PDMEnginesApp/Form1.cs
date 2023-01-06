using MySql.Data.MySqlClient;
using PDMEnginesApp.config;
using System.Data;
using System.Xml.Linq;

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
        }

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

        private void addComponent(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            command = new MySqlCommand("INSERT INTO `pdm_engines`.`components` (name, amount) VALUES (@name, @amount);", dataBase.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameField.Text;
            command.Parameters.Add("@amount", MySqlDbType.VarChar).Value = amountField.Text;
            adapter.SelectCommand = command;

            TreeNode node = new TreeNode(nameField.Text);
            try
            { 
                treeView1.SelectedNode.Nodes.Add(node);
            } catch (Exception ex) 
            { 
                MessageBox.Show("Выберите строку для добавления компонента");
            }
        }

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