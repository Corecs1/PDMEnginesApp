using PDMEnginesApp.entity;
using PDMEnginesApp.model.entity;

public interface IPDMView
{
    string NameField { get; }
    string AmountField { get; }
    TreeView TreeView { get; }
    void btnAddEngine_Click(object sender, EventArgs e);

    void btnAddComponent_Click(object sender, EventArgs e);

    void btnRename_Click(object sender, EventArgs e);

    void btnDelete_Click(object sender, EventArgs e);
}

public interface IPDMPresenter
{
    void InitData();
    void AddEngine();
    void AddComponent();
    void Rename();
    void Delete();
    void Run();
}

public interface IPDMService
{
    ICollection<Engine> GetEngines();
    ICollection<EngineComponent> GetComponents();

    Engine GetEngine(string engineName);
    EngineComponent GetComponent(string componentName);

    ICollection<Engine> InitEngines();
    ICollection<EngineComponentAmount> GetEngineComponentAmountsByEngine(Engine engine);
    EngineComponent GetComponentByEngineComponentAmount(EngineComponentAmount eca);
    ICollection<ComponentComponentAmount> GetComponentComponentAmountsByComponent(EngineComponent engComponent);
    EngineComponent GetComponentByComponentComponentAmount(ComponentComponentAmount cca, Engine engine);

    bool AddEngine(string engineName);
    bool AddComponentToEngine(string engineName, string componentName, string amountOfComponents);
    bool AddComponentToComponent(string componentName, string nestedComponentName, string amountOfComponents);

    bool RenameEngine(string oldEngineName, string newEngineName);
    bool RenameComponent(string oldComponentName, string newComponentName);

    void DeleteEngine(string engineName);
    void DeleteComponent(string componentName);
}

