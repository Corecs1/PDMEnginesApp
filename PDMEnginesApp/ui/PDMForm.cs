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
            presenter.InitData();
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
