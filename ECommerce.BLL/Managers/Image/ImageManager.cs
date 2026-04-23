using ECommerce.Common;
using FluentValidation;

namespace ECommerce.BLL
{
    public class ImageManager : IImageManager
    {
        private readonly IValidator<ImageUploadDto> _imageValidator;
        private readonly IErrorMapper _errorMapper;


        public ImageManager(IErrorMapper errorMapper, IValidator<ImageUploadDto> imageValidator)
        {
            _imageValidator = imageValidator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<ImageUploadResultDto>> UploadAsync(
            ImageUploadDto imageUploadDto,
            string basePath,
            string? schema,
            string? host)
        {
            if (string.IsNullOrWhiteSpace(schema) || string.IsNullOrWhiteSpace(host)) 
            {
                return GeneralResult<ImageUploadResultDto>.FailResult("Missing schema or host");
            }

            var validationResult = await _imageValidator.ValidateAsync(imageUploadDto);

            if(!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<ImageUploadResultDto>.FailResult(errors);
            }

            var file  = imageUploadDto.File;
            var extension = Path.GetExtension(file.FileName).ToLower();
            var cleanName = Path.GetFileNameWithoutExtension(file.FileName)
                                                            .Replace(" ", "-")
                                                            .ToLower();
            var newFileName = $"{cleanName}-{Guid.NewGuid()}{extension}";
            var directoryPath = Path.Combine(basePath, "Files");

            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var fullFilePath = Path.Combine(directoryPath,newFileName);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{schema}://{host}/Files/{newFileName}";
            var imageUploadResult = new ImageUploadResultDto(url);
            return GeneralResult<ImageUploadResultDto>.SuccessResult(imageUploadResult);

        }
    }
}
