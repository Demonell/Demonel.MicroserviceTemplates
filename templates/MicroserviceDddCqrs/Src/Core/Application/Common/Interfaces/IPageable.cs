namespace Application.Common.Interfaces
{
    public interface IPageable
    {
        int? Skip { get; set; }
        int? Take { get; set; }
    }
}
