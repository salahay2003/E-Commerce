using FluentValidation;

namespace ECommerce.BLL
{
    public class ImageValidator : AbstractValidator<ImageUploadDto>
    {
        private static readonly string[] AllowedExtensions = [".png", ".jpg", ".jpeg"];

        public ImageValidator()
        {
            RuleFor(i => i.File)
                .NotNull()
                .WithMessage("Image is required")
                .WithErrorCode("ERR-08")
                .WithName("Image");
            When(i => i.File != null, () =>
            {
                RuleFor(i => i.File.Length)
                .GreaterThan(0)
                .WithMessage("File must not be empty")
                .WithErrorCode("ERR-08")
                .WithName("FileSize")

                .LessThanOrEqualTo(5_000_000)
                .WithMessage("File must not exceed 5MG")
                .WithErrorCode("ERR-08")
                .WithName("FileSize");

                RuleFor(i => Path.GetExtension(i.File.FileName).ToLower())
                    .Must(ext => AllowedExtensions.Contains(ext))
                    .WithMessage("Unsupported file extention")
                    .WithErrorCode("ERR-01")
                    .WithName("FileExtention");
            });

        }
    }
}
