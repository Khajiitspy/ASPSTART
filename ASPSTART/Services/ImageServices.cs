using ASPSTART.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ASPSTART.Services;

public class ImageServices(IConfiguration configuration) : IImageService
{
    public async Task DeleteImageAsync(string name)
    {
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();
        var dir = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!);

        Task[] tasks = sizes
            .AsParallel()
            .Select(size =>
            {
                return Task.Run(() =>
                {
                    var path = Path.Combine(dir, $"{size}_{name}");
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                });
            })
            .ToArray();

        await Task.WhenAll(tasks);
    }

    public async Task<string> SaveImageFromUrlAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        return await SaveImageAsync(imageBytes);
    }


    public async Task<string> SaveImageAsync(IFormFile file)
    {
        using MemoryStream ms = new();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();

        var imageName = await SaveImageAsync(bytes);
        return imageName;
    }

    public async Task<string> SaveImageAsync(byte[] bytes, bool dynamic = true)
    {
        string imageName = $"{Path.GetRandomFileName()}.webp";

        if (dynamic)
        {
            var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>();

            Task[] tasks = sizes
                .AsParallel()
                .Select(s => SaveImageAsync(bytes, imageName, s))
                .ToArray();

            await Task.WhenAll(tasks);
        }
        else
        {
            await SaveImageAsync(bytes, imageName);
        }

        return imageName;
    }

    public async Task<string> SaveImageFromBase64Async(string input, bool dynamic = true)
    {
        var base64Data = input.Contains(",")
           ? input.Substring(input.IndexOf(",") + 1)
           : input;

        byte[] imageBytes = Convert.FromBase64String(base64Data);

        return await SaveImageAsync(imageBytes, dynamic);
    }

    private async Task SaveImageAsync(byte[] bytes, string name, int size)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!,
            $"{size}_{name}");
        using var image = Image.Load(bytes);
        image.Mutate(async imgConext =>
        {
            imgConext.Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max
            });
            await image.SaveAsync(path, new WebpEncoder());
        });
    }

    private async Task SaveImageAsync(byte[] bytes, string name)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!,
            name);
        using var image = Image.Load(bytes);
        await image.SaveAsync(path, new WebpEncoder());
    }
}
