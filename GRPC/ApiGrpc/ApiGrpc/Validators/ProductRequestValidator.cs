using FluentValidation;
using GrpcProduct;
namespace ApiGrpc.Validators
{
    public class ProductRequestValidator: AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El nombre no puede ser nulo");
            RuleFor(x => x.Price).NotEmpty().WithMessage("El precio no puede ser nulo");
            RuleFor(x => x.Stock).NotEmpty().WithMessage("El stock no puede ser nulo");
        }
    }
}
