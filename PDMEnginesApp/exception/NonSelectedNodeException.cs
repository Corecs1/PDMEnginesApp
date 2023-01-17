namespace PDMEnginesApp.exception
{
    class NonSelectedNodeException : Exception
    {
        public NonSelectedNodeException()
            : base()
        {
            MessageBox.Show("Выберите двигатель или компонент для добавления");
        }
    }
}
