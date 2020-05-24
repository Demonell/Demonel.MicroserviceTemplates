using FluentValidation;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidation : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ProductType).NotEmpty();
        }
    }
}