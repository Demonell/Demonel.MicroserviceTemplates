namespace Application.Common.Exceptions
{
    public class ProductNameAlreadyInUseException : AppException
    {
        public ProductNameAlreadyInUseException(string message) : base(message)
        {
        }
    }
}
