using PDMEnginesApp.entity;
using PDMEnginesApp.exception;
using PDMEnginesApp.model.service;
using PDMEnginesApp.presentation;

namespace PDMEnginesApp
{
    public partial class PDMForm : Form, IPDMView
    {
        private PDMPresenter presenter;

        string IPDMView.NameField => NameField.Text;

        string IPDMView.AmountField => AmountField.Text;

        TreeView IPDMView.TreeView => PdmTree;

        public PDMForm()
        {
            InitializeComponent();
            presenter = new PDMPresenter(this, new DataBaseService());
            initializeData();
        }

        // Метод берёт строки из базы данных и инициализирует в ноды treeView
        private void initializeData()
        {
            //private TreeNode parentNode;

            //var engines = (from engine in db.engines.Include(e => e.components)
            //             select engine).ToList();

            //foreach (Engine engine in engines)
            //{
            //    parentNode = PdmTree.Nodes.Add(engine.name);
            //    initializeComponents(engine, parentNode);
            //}
        }

        // Метод проходится по компонентам двигателя и инициализирует их в childTreeView
        private void initializeComponents(Engine engine, TreeNode parentNode)
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

        // Метод добавляет двигатель в базу данных и Node
        public void btnAddEngine_Click(object sender, EventArgs e)
        {
            presenter.AddEngine();
        }

        // Метод добавляет компонент в базу данных и Node
        public void btnAddComponent_Click(object sender, EventArgs e)
        {
            presenter.AddComponent();
        }

        // Метод переименовывает двигатель или компонент из базы данных и Node
        public void btnRename_Click(object sender, EventArgs e)
        {
            presenter.Rename();
        }

        // Метод удаляет двигатель или компонент из базы данных и Node
        public void btnDelete_Click(object sender, EventArgs e)
        {
            presenter.Delete();
        }
    }
}
