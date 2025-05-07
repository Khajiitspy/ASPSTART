namespace ASPSTART.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
        Task<string> SaveImageAsync(byte[] bytes, bool dynamic = true);
        Task<string> SaveImageFromBase64Async(string input, bool dynamic = true);
        Task DeleteImageAsync(string name);
        Task<string> SaveImageFromUrlAsync(string imageUrl);
    }
}
