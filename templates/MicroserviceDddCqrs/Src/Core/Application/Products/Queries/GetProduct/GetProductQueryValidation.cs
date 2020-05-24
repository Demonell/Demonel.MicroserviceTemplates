using FluentValidation;

namespace Application.Products.Queries.GetProduct
{
    public class GetProductQueryValidation : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidation()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
