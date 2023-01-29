namespace PDMEnginesApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new PDMForm());

            //var presenter = new PDMPresenter(new PMDApp(), new PDMService());
            //presenter.run();
        }
    }
}