namespace PDMEnginesApp.exception
{
    class EmptyFieldException : Exception
    {
        public EmptyFieldException(string message)
            : base(message)
        {
            MessageBox.Show(message);
        }
    }
}
