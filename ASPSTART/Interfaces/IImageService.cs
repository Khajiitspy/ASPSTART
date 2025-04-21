namespace ASPSTART.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
        Task<string> SaveImageAsync(byte[] bytes);
        Task DeleteImageAsync(string name);
    }
}
