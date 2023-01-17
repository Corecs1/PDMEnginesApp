namespace PDMEnginesApp.exception
{
    class DublicateException : Exception
    {
        public DublicateException(string message)
            : base(message)
        {
            MessageBox.Show($"Элемент {message} уже содержится в базе данных");
        }
    }
}
