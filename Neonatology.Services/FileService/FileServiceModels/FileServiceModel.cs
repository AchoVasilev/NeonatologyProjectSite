namespace Neonatology.Services.FileService.FileServiceModels;

public class FileServiceModel : IFileServiceModel
{
    public string Extension { get; set; }

    public string Uri { get; set; }

    public string Name { get; set; }
}