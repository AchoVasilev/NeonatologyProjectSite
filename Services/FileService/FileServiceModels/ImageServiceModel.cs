﻿namespace Services.FileService.FileServiceModels
{
    public class ImageServiceModel : IFileServiceModel
    {
        public string Extension { get; set; }

        public string Uri { get; set; }

        public string Name { get; set; }
    }
}
