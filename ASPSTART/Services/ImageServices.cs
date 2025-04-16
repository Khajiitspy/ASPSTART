using ASPSTART.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ASPSTART.Services;

public class ImageServices(IConfiguration configuration) : IImageService
{
    public async Task<string> SaveImageAsync(IFormFile file)
    {
        using MemoryStream ms = new();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var imageName = await SaveImageAsync(bytes);
        return imageName;
    }

    private async Task<string> SaveImageAsync(byte[] bytes)
    {
        //string imageName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
        string imageName = $"{Path.GetRandomFileName()}.webp";
        var sizes = configuration.GetSection("ImageSizes").Get<List<int>>();
        Task[] tasks = sizes.AsParallel().Select(size => SaveImageAsync(bytes, imageName, size)).ToArray();
        await Task.WhenAll(tasks);
        return imageName;
    }

    private async Task SaveImageAsync(byte[] bytes, string name, int size)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!, $"{size}_{name}");
        using var image = Image.Load(bytes);
        image.Mutate(async x =>
        {
            x.Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max
            });
            await image.SaveAsync(path, new WebpEncoder());
        });
    }
}
