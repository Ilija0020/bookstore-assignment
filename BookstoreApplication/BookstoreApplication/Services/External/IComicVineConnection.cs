namespace BookstoreApplication.Services.External
{
    public interface IComicVineConnection
    {
        Task<string> Get(string url);
    }
}
