using PDMEnginesApp.entity;

public interface IPDMView
{
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

    void AddEngine(Engine engine);
    void AddComponent(EngineComponent component);

    void RenameEngine(Engine engine);
    void RenameComponent(EngineComponent component);

    void DeleteEngine(Engine engine);
    void DeleteComponent(EngineComponent component);

}

