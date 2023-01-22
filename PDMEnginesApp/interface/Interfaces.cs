using PDMEnginesApp.entity;

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

    void AddEngine(string engineName);
    void AddComponentToEngine(Engine engine, string componentName, string amountOfComponents);
    void AddComponentToComponent(EngineComponent component, string componentName, string amountOfComponents);

    void RenameEngine(string oldEngineName, string newEngineName);
    void RenameComponent(string oldComponentName, string newComponentName);

    void DeleteEngine(string engineName);
    void DeleteComponent(string componentName, string amount);
}

