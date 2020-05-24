using FluentValidation;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidation : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductType).NotEmpty();
        }
    }
}