namespace BookstoreApplication.Services.Exceptions
{
    public class ApiCommunicationException : Exception
    {
        public ApiCommunicationException(string message) : base(message)
        {
        }
    }
}
