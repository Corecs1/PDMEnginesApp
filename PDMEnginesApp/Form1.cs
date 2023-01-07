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

        // ����� ����������� �� ���� ������ � ��������� Node �������� �� ��
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

        // ����� ����������� �� �����������
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

  
        // ����� ��������� ��������� � �� � Node
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
                MessageBox.Show("��������� ��������");
            } 
            else
            {
                MessageBox.Show("��������� �� ��������");
            }

            dataBase.closeConnection(); 
        }

        // ����� ��������� ��������� � �� � Node
        private void addComponent(object sender, EventArgs e)
        {
            
            /*
                ��������, �������� �� �� ��������� � ��������� ��� � ����������
                (�������� ���������� �� SelectedNode � RootNode)
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

                // SELECT id �� ���������� name � amount ��������� �� ��
                command = new MySqlCommand("SELECT * FROM `pdm_engines`.`engines` WHERE `pdm_engines`.`engines`.name = @name", dataBase.getConnection());
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = treeView1.SelectedNode.Nodes.ToString();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                DataRow rows = table.Rows[0];
                String engineId = rows["id"].ToString();

                // SELECT id �� ���������� name � amount ���������� �� ��
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
                // SELECT id �� ���������� name � amount ���������� �� ��
                // INSERT INTO engine_component engine_id & component_id
            }

            TreeNode node = new TreeNode(nameField.Text);
            try
            { 
                var xui = treeView1.SelectedNode.Nodes.Add(node);
            } catch (Exception ex) 
            { 
                MessageBox.Show("�������� ������ ��� ���������� ����������");
            }
        }

        // ����� ��������������� ��������� ��� ���������
        private void rename(object sender, EventArgs e)
        {
            try
            {
                treeView1.SelectedNode.Text = nameField.Text;
            } catch (Exception ex)
            {
                MessageBox.Show("�������� ������ ��� ��������� ��������");
            } 
        }

        // ����� ������� ��������� ��� ���������
        private void remove(object sender, EventArgs e)
        {
            try
            {
                treeView1.SelectedNode.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show("�������� ������ ��� ��������");
            }
        }
    }
}