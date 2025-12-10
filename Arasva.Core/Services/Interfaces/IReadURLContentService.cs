namespace Arasva.Core.Services.Interfaces
{
    public interface IReadURLContentService
    {
        Task<GlobalResponse> ReadURLContent(string filepath, string path);
    }
}
