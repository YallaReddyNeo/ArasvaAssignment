namespace Arasva.Core.Interface
{
    public interface IReadURLContentRepository
    {
        Task<GlobalResponse> ReadURLContent(string filepath, string path);
    }
}
