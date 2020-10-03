namespace FvpWebAppWorker.Services.Interfaces
{
    public interface IDataService
    {
        async List<Document> Documents(Source source);
    }
}