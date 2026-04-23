using ECommerce.Common;
using FluentValidation;
using FluentValidation.Results;

namespace ECommerce.BLL
{
    public interface IErrorMapper 
    {
        Dictionary<string, List<Error>> MapError(ValidationResult validationResult);
    }
}
